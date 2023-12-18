using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

public class Game
{
    public int size { get; private set; }
    GameItemService gameItemService;
    int selectedIndex = -1;
    Random rand = new Random();
    public DateTime startTime { get; private set; } = DateTime.Now;
    Guid[] solution;
    Guid[] state;
    Guid[] previousState;

    int widthWindow;

    TimeSpan refreshRate;

    public Game(int size, int widthWindow, TimeSpan refreshRate)
    {
        this.size = size;
        this.widthWindow = widthWindow;
        this.refreshRate = refreshRate;
        initState();
    }

    private void initState()
    {
        gameItemService = new GameItemService(new ColorSimple(), size, widthWindow);
        state = previousState = gameItemService.ToArray();
        solution = gameItemService.ToArray();
        while (GetScore() != 0)
            rand.Shuffle(solution);
    }

    public int GetScore()
    {
        int score = 0;
        for (int i = 0; i < size; i++)
        {
            if (state[i] == solution[i])
                score++;
        }
        return score;
    }

    public string GetScoreString()
    {
        return GetScore() + "/" + size;
    }

    public bool LevelUp()
    {
        if(GetScore() == size)
        {
            startTime = DateTime.Now;
            size++;
            initState();
            gameItemService.GenerateGameItems(size);
            return true;
        }
        return false;
    }

    public void Draw(Grid grid)
    {
        grid.Children.Clear();
        grid.ColumnDefinitions.Clear();
        int columnCount = 0;

        foreach (var id in state)
        {
            var item = gameItemService.GetByID(id);
            var ellipse = item.GetEllipse(selectedIndex == columnCount);
            ellipse.Tag = item.id;
            ellipse.MouseLeftButtonDown -= Elipse_MouseLeftButtonDown;
            ellipse.MouseLeftButtonDown += Elipse_MouseLeftButtonDown;

            ColumnDefinition columnDefinition = new ColumnDefinition();
            grid.ColumnDefinitions.Add(columnDefinition);
            Grid.SetColumn(ellipse, columnCount);
            grid.Children.Add(ellipse);

            if (previousState[columnCount] != state[columnCount])
            {
                var colorAnimation = new ColorAnimation();
                colorAnimation.From = Colors.White;
                colorAnimation.Duration = refreshRate;
                SolidColorBrush newBrush = new SolidColorBrush(((SolidColorBrush)ellipse.Fill).Color);
                if (newBrush.IsFrozen)
                    newBrush.Freeze(); // Unfreeze the brush
                ellipse.Fill = newBrush;
                newBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
            }

            columnCount++;
        }

        previousState = state.ToArray();
    }

    public void DrawSolution(Grid grid)
    {
        grid.Children.Clear();
        grid.ColumnDefinitions.Clear();
        int columnCount = 0;
        foreach (var id in solution)
        {
            var item = gameItemService.GetByID(id);
            var elipse = item.GetEllipse();
            grid.Children.Add(elipse);
            ColumnDefinition columnDefinition = new ColumnDefinition();
            grid.ColumnDefinitions.Add(columnDefinition);
            Grid.SetColumn(elipse, columnCount);
            columnCount++;
        }
    }

    public void HideSolution(Grid grid)
    {
        grid.Children.Clear();
        grid.ColumnDefinitions.Clear();
    }

    private void Elipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is Ellipse clickedEllipse)
        {
            var cur = Grid.GetColumn(clickedEllipse);

            if (selectedIndex == -1)
            {
                selectedIndex = cur;
            }
            else
            {
                var save = state[cur];
                state[cur] = state[selectedIndex];
                state[selectedIndex] = save;
                selectedIndex = -1;
            }
        }
    }
}
