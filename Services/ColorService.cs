using System.Collections.Generic;
using System.Windows.Media;

public class ColorService : IColorService
{
    Stack<SolidColorBrush> brushes = new Stack<SolidColorBrush>();

    public ColorService(int size)
    {
        int i = 0;

        foreach (var property in typeof(Brushes).GetProperties())
        {
            if (property.PropertyType == typeof(SolidColorBrush))
            {
                SolidColorBrush brush = (SolidColorBrush)property.GetValue(null, null);
                brushes.Push(brush);
            }
            i++;

            if (i == size)
                break;
        }
    }
    public SolidColorBrush GetSolidColorBrush()
    {
        if (brushes.Count == 0)
            return Brushes.Transparent;
        return brushes.Pop();
    }
}
