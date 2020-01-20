using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapSelectionControllerBase
{
    enum Type
    {
        Area,
        Count
    }

    public MapSelectionControllerBase(TileCoord initialCoord)
    {
        InitialCoord = initialCoord;
    }

    public TileCoord InitialCoord = new TileCoord();

    public List<TileCoord> SelectedTiles = new List<TileCoord>();

    public virtual void update()
    {

    }

    public virtual void enable()
    {

    }

    public virtual void disable()
    {

    }

    public virtual void clear()
    {
        SelectedTiles.Clear();
        GlobalManagers.mapManager.clearTilesSelection();
    }
}

public class AreaMapSelectionController : MapSelectionControllerBase
{
    public enum AreaType
    {
        Cross,
        Circle,
        Square
    }

    public int Range { get; set; } = 2;

    public AreaType ShapeType { get; set; } = AreaType.Cross;

    public AreaMapSelectionController(TileCoord initialCoord)
        : base(initialCoord)
    {
    }

    public override void update()
    {
        base.update();

        var type = GlobalManagers.mapManager.TileType;

        List<TileCoord> selectedCoord = new List<TileCoord>();

        if (type == TileType.Cube)
        {
            for (int x = -Range; x <= Range; ++x)
            {
                for (int y = -Range; y <= Range; ++y)
                {
                    if (ShapeType == AreaType.Circle)
                    {
                        if (Mathf.Abs(x) + Mathf.Abs(y) > Range)
                        {
                            continue;
                        }
                    }

                    if (ShapeType == AreaType.Cross)
                    {
                        if (x != 0 && y != 0)
                        {
                            continue;
                        }
                    }

                    selectedCoord.Add(new TileCoord(InitialCoord.Square.x + x, InitialCoord.Square.y + y));
                }
            }
        }

        GlobalManagers.mapManager.setTilesSelected(selectedCoord);
    }
}
