using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Linq;

namespace sail
{
    [Flags]
    public enum TileLayerID
    {
        NONE = 0,
        SELECTABLE = 1 << 0,
        HIGHLIGHTED = 1 << 1,
        SELECTED = 1 << 2
    }

    public static class TileLayerIDExtensions
    {
        public static bool isBitSet(this TileLayerID flag, TileLayerID bit)
        {
            return (flag & bit) != 0;
        }
    }

    public class TileSelectionElement
    {
        private Unit Owner { get; set; } = null;

        private ActionSelectionModel Model { get; set; } = null;

        private Dictionary<TileCoord, TileLayerID> Tiles = new Dictionary<TileCoord, TileLayerID>();

        public TileSelectionElement(ActionSelectionModel model, Unit owner)
        {
            Owner = owner;
            Model = model;
        }

        public void enable()
        {
            Assert.IsNotNull(Owner, "Tile selection: Owner is null.");
            Assert.IsNotNull(Model, "Tile selection: Selection Model is null.");

            var tiles = Model.SelectionModel.activeTiles(Owner);
            set(tiles, TileLayerID.SELECTABLE);
        }

        public void disable()
        {
            clear();
        }

        public List<TileCoord> get(TileLayerID layer)
        {
            var list = Tiles.Where(pair => (pair.Value & layer) == layer).Select(pair => pair.Key).ToList();

            if (layer == TileLayerID.SELECTED)
            {
                Debug.Log("SELECT TILES " + list);
            }

            return list;
        }

        public void set(TileCoord tile, TileLayerID layer)
        {
            clear(layer);
            add(tile, layer);
        }

        public void set(List<TileCoord> tiles, TileLayerID layer)
        {
            clear(layer);
            add(tiles, layer);
        }

        public void add(TileCoord tile, TileLayerID layer)
        {
            if (Tiles.ContainsKey(tile) == false)
            {
                Tiles.Add(tile, layer);
                return;
            }

            Tiles[tile] |= layer;
        }

        public void add(List<TileCoord> tiles, TileLayerID layer)
        {
            foreach (var tile in tiles)
            {
                add(tile, layer);
            }
        }

        public void clear(TileLayerID layer)
        {
            foreach (var key in Tiles.Keys.ToList())
            {
                 Tiles[key] = Tiles[key] & ~layer;
            }
        }

        public void clear()
        {
            var board = GlobalManagers.board;

            foreach (var pair in Tiles)
            {
                var tile = board.getTile(pair.Key);
                tile.setColor(Color.white);
            }

            Tiles.Clear();
        }

        public void update()
        {
            var board = GlobalManagers.board;

            foreach (var pair in Tiles)
            {
                Color color = Color.white;

                if (pair.Value.isBitSet(TileLayerID.SELECTED))
                {
                    color = Color.yellow;
                }
                else if(pair.Value.isBitSet(TileLayerID.HIGHLIGHTED))
                {
                    color = Color.cyan;
                }
                else if (pair.Value.isBitSet(TileLayerID.SELECTABLE))
                {
                    color = Color.blue;
                }

                board.getTile(pair.Key).setColor(color);
            }
        }
    }
}
