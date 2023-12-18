using System;
using System.Windows.Media;
using System.Windows.Shapes;

public class GameItem
{
    Brush fill;
    double height;

    public Guid id { get; private set; }

    public GameItem(double height, Brush fill)
    {
        id = Guid.NewGuid();
        this.height = height;
        this.fill = fill;
    }

    public Ellipse GetEllipse(bool selected = false)
    {
        return new Ellipse
        {
            Width = height,
            Height = height,
            Fill = fill,
            Stroke = selected ? Brushes.Yellow : Brushes.White,
            StrokeThickness = 5
        };
    }
}
