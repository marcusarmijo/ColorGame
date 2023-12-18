using System.Collections.Generic;
using System.Windows.Media;

public class ColorSimple : IColorService
{
    private int currentIndex = 0;

    List<SolidColorBrush> brushes = new List<SolidColorBrush>
            {
                Brushes.Red,
                Brushes.Blue,
                Brushes.Orange,
                Brushes.Green,
                Brushes.Yellow,
                Brushes.Purple,
                Brushes.Pink
            };

    public SolidColorBrush GetSolidColorBrush()
    {
        currentIndex = (currentIndex + 1) % brushes.Count;
        return brushes[currentIndex];
    }
}
