using System.Windows.Media;

public class ColorBlind : IColorService
{
    private int steps = 10; // Number of steps between white and black
    private int currentIndex = 0;

    public SolidColorBrush GetSolidColorBrush()
    {
        byte lightness = (byte)((currentIndex * 255) / steps);
        Color color = Color.FromRgb(lightness, lightness, lightness);
        currentIndex = (currentIndex + 1) % steps;
        return new SolidColorBrush(color);
    }
}