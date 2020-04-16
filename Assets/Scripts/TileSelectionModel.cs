using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public abstract class TileSelectionModelBase
    {
        enum Type
        {
            Area,
            Count
        }

        public abstract List<TileCoord> activeTiles(Unit unit);
    }

    public class SelfTileSelection : TileSelectionModelBase
    {
        public override List<TileCoord> activeTiles(Unit unit)
        {
            List<TileCoord> selectedTiles = new List<TileCoord>();
            selectedTiles.Add(unit.Coord);
            return selectedTiles;
        }
    }
    public class AreaTileSelection : TileSelectionModelBase
    {
        public enum AreaType
        {
            Cross,
            Circle,
            Square
        }

        public int Range { get; set; } = 3;

        public AreaType ShapeType { get; set; } = AreaType.Cross;

        public override List<TileCoord> activeTiles(Unit unit)
        {
            var type = GlobalManagers.board.TileType;

            var selectedTiles = new List<TileCoord>();
            var origin = unit.Coord;

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

                        var current = new TileCoord(origin.Square.x + x, origin.Square.y + y);
                        if (current.isValid())
                        {
                            selectedTiles.Add(current);
                        }
                    }
                }
            }
            else if (type == TileType.Hex)
            {
                for (int q = -Range; q <= Range; ++q)
                {
                    for (int r = Mathf.Max(-Range, -q - Range); r <= Mathf.Min(Range, -q + Range); ++r)
                    {
                        var s = -q - r;

                        if (ShapeType == AreaType.Cross)
                        {
                            if (q != 0 && r != 0 && s != 0)
                            {
                                continue;
                            }
                        }

                        // Circle or Square
                        var current = new TileCoord(origin.Hex.q + q, origin.Hex.r + r, origin.Hex.s + s);
                        if (current.isValid())
                        {
                            selectedTiles.Add(current);
                        }
                    }
                }
            }

            return selectedTiles;
        }
    }
}