using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace sail
{
    public enum TileLayerID
    {
        SELECTABLE,
        HIGHLIGHTED,
        SELECTED,
        COUNT
    }

    public class TileSelectionElement
    {
        Unit Owner { get; set; } = null;

        TileSelectionBase SelectionModel { get; set; } = null;
        TileSelectionBase TargetModel { get; set; } = null;

        public List<TileCoord>[] TileLayers = new List<TileCoord>[(int)TileLayerID.COUNT];

        public TileSelectionElement()
        {
            for (int i=0; i < TileLayers.Length; ++i)
            {
                TileLayers[i] = new List<TileCoord>();
            }
        }

        public void enable()
        {
            Assert.IsNotNull(Owner, "Tile selection: Owner is null.");
            Assert.IsNotNull(SelectionModel, "Tile selection: Selection Model is null.");
            Assert.IsNotNull(TargetModel, "Tile selection: Target Model is null.");

            var tiles = SelectionModel.activeTiles(Owner);
            set(TileLayerID.SELECTABLE, tiles);
        }

        public void disable()
        {
            clear();
        }

        public List<TileCoord> get(TileLayerID layer)
        {
            return new List<TileCoord>(TileLayers[(int)layer]);
        }

        public void set(TileLayerID layer, TileCoord tile)
        {
            clear(layer);
            add(layer, tile);
        }

        public void set(TileLayerID layer, List<TileCoord> tiles)
        {
            clear(layer);
            add(layer, tiles);
        }

        public void add(TileLayerID layer, TileCoord tile)
        {
            TileLayers[(int)layer].Add(tile);
            BattleFSM.Instance.board.setTileColor(tile, Color.blue);
        }

        public void add(TileLayerID layer, List<TileCoord> tiles)
        {
            foreach (var tile in tiles)
            {
                add(layer, tile);
            }
        }

        public void clear(TileLayerID layer)
        {
            var list = TileLayers[(int)layer];
            var board = GlobalManagers.board;
            foreach (var tile in list)
            {
                board.getTile(tile).GetComponent<MeshRenderer>().material.color = Color.white;
            }

            list.Clear();
        }

        public void clear()
        {
            var board = GlobalManagers.board;

            foreach (var layer in TileLayers)
            {
                foreach (var tile in layer)
                {
                    board.getTile(tile).GetComponent<MeshRenderer>().material.color = Color.white;
                }

                layer.Clear();
            }   
        }
    }
}
