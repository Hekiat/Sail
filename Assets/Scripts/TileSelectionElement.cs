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
        private Unit Owner { get; set; } = null;

        private ActionSelectionModel Model { get; set; } = null;

        private List<TileCoord>[] TileLayers = new List<TileCoord>[(int)TileLayerID.COUNT];

        public TileSelectionElement(ActionSelectionModel model, Unit owner)
        {
            Owner = owner;
            Model = model;

            for (int i=0; i < TileLayers.Length; ++i)
            {
                TileLayers[i] = new List<TileCoord>();
            }
        }

        public void enable()
        {
            Assert.IsNotNull(Owner, "Tile selection: Owner is null.");
            Assert.IsNotNull(Model, "Tile selection: Selection Model is null.");

            var tiles = Model.SelectionModel.activeTiles(Owner);
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
