using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sail
{
    [Flags]
    public enum TileSelectionFilter
    {
        NONE = 0 << 0,
        ENEMIES = 1 << 0,
        TERRAIN = 1 << 1,

        ALL = ENEMIES | TERRAIN
    }

    public abstract class TileSelectionModelBase
    {
        public TileSelectionFilter Filter = TileSelectionFilter.ALL;

        public abstract List<TileCoord> activeTiles(TileCoord origin, TileCoord? direction = null);

        protected virtual void filter(List<TileCoord> tiles)
        {
            if (Filter == TileSelectionFilter.ALL)
            {
                return;
            }

            bool[] keep = new bool[tiles.Count];
            for (int i = 0; i < keep.Length; i++) { keep[i] = false; }

            // Enemies
            if (Filter.HasFlag(TileSelectionFilter.ENEMIES))
            {
                foreach(var enemy in BattleFSM.Instance.enemies)
                {
                    var index = tiles.FindIndex((e) => e == enemy.Coord);
                    if (index != -1)
                    {
                        keep[index] = true;
                    }
                }
            }

            // OTHER TODO


            // Clean
            for (int i=keep.Length-1; i>=0; i--)
            {
                if (keep[i] == false)
                {
                    tiles.RemoveAt(i);
                }
            }
        }
    }

    public class SelfTileSelection : TileSelectionModelBase
    {
        public override List<TileCoord> activeTiles(TileCoord origin, TileCoord? direction = null)
        {
            List<TileCoord> selectedTiles = new List<TileCoord>();
            selectedTiles.Add(origin);
            return selectedTiles;
        }
    }

    public class ConeTileSelection : TileSelectionModelBase
    {
        public int Range { get; set; } = 3;

        public override List<TileCoord> activeTiles(TileCoord origin, TileCoord? direction = null)
        {
            List<TileCoord> selectedTiles = new List<TileCoord>();

            if (direction == null)
            {
                return selectedTiles;
            }

            // todo HEX
            var orto = direction.Value.Square.x == 0 ? TileCoord.AxisX : TileCoord.AxisY;

            for (int i=0; i<Range; ++i)
            {
                var dirOffset = direction.Value * i;

                for (int j=0-i; j<=i; ++j)
                {
                    var offset = dirOffset + orto * j;
                    var tileCoord = origin + offset;
                    if (tileCoord.isValid())
                    {
                        selectedTiles.Add(tileCoord);
                    }
                }
            }

            return selectedTiles;
        }
    }

    //public class EnemyTileSelection : TileSelectionModelBase
    //{
    //    public int Range { get; set; } = int.MaxValue;
    //
    //    public override List<TileCoord> activeTiles(Unit unit)
    //    {
    //        List<TileCoord> selectedTiles = new List<TileCoord>();
    //
    //        foreach (var enemy in BattleFSM.Instance.enemies)
    //        {
    //            var distance = Mathf.Abs(unit.Coord.Square.x - enemy.Coord.Square.x) + Mathf.Abs(unit.Coord.Square.y - enemy.Coord.Square.y);
    //
    //            if (distance <= Range)
    //            {
    //                selectedTiles.Add(enemy.Coord);
    //            }
    //        }
    //
    //        return selectedTiles;
    //    }
    //}

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

        public override List<TileCoord> activeTiles(TileCoord origin, TileCoord? direction = null)
        {
            var type = GlobalManagers.board.TileType;

            var selectedTiles = new List<TileCoord>();

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

            // filter
            filter(selectedTiles);

            return selectedTiles;
        }
    }
}