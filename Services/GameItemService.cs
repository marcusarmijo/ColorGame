using System;
using System.Collections.Generic;
using System.Linq;

public class GameItemService
{
    public Dictionary<Guid, GameItem> items = new Dictionary<Guid, GameItem>();
    int size;
    IColorService colorService;

    public GameItemService(IColorService colorService, int size, int widthWindow)
    {
        this.size = size;
        this.colorService = colorService;
        GenerateGameItems(widthWindow / size);
    }

    public Guid[] ToArray()
    {
        return items.Select(o => o.Value.id).ToArray();
    }

    public void GenerateGameItems(int height)
    {
        while (items.Count < size)
        {
            var item = new GameItem(height, colorService.GetSolidColorBrush());
            items.Add(item.id, item);
        }
    }

    public GameItem GetByID(Guid id)
    {
        return items[id];
    }
}
