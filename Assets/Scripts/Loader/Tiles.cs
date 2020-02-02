using System;
using System.Collections.Generic;
using SuperTiled2Unity;
using UnityEngine;

namespace Loader
{
    public static class Tiles
    {
        public static void ParseTiles(SuperMap map, MonoBehaviour root)
        {
            var _baseDataLayer = map.transform.Find("Grid").Find("data").GetComponent<SuperTileLayer>();

            // TODO: Remove when real level parsing is in place
            var pathSet = false;

            Tile neighbor;
        
            // Rig up all the cardinal directions. THIS LOOP ASSUMES some things:
            // children are in Columns from left to right
            for (var i = 0; i < _baseDataLayer.transform.childCount; i++)
            {
                var currentTile = _baseDataLayer.transform.GetChild(i);
                currentTile.GetComponent<SpriteRenderer>().sortingOrder = -1;
                var props = currentTile.GetComponent<SuperCustomProperties>();
            
                CustomProperty solidProp;
                var found = props.TryGetCustomProperty("type", out solidProp);
                if (!found)
                {
                    throw new Exception($"Unable to find 'solid' property on custom properties of object");
                }
            
                var tileComp = currentTile.gameObject.AddComponent<Tile>();
                CustomProperty conProp;
                if (!props.TryGetCustomProperty("conns", out conProp))
                {
                    throw new Exception("NO CONNS FOUND");
                }
                tileComp.connections = conProp.GetValueAsInt();
            
                TileType tType;
                TileType.TryParse(solidProp.GetValueAsString(), true, out tType);
                tileComp.tileType = tType;

                if (tType == TileType.DOOR || tType == TileType.ROAD)
                {
                    currentTile.gameObject.AddComponent<DestinationSelectable>();
                    currentTile.gameObject.AddComponent<BoxCollider2D>();
                }
            
                if (i % map.m_Height != 0)
                {
                    tileComp.north = _baseDataLayer.transform.GetChild(i - 1).GetComponent<Tile>();

                    neighbor = _baseDataLayer.transform.GetChild(i - 1).GetComponent<Tile>();
                    neighbor.south = tileComp;

                    if (neighbor.tileType == TileType.DOOR)
                    {
                        tileComp.connections |= 1; // add a connection to the building to the north of us
                    }
                }

                if ((i - map.m_Height) >= 0)
                {
                    tileComp.west = _baseDataLayer.transform.GetChild(i - map.m_Height).GetComponent<Tile>();
                    _baseDataLayer.transform.GetChild(i - map.m_Height).GetComponent<Tile>().east = tileComp;
                }

                if (!pathSet)
                {
                    pathSet = true;
                    var testCar = MonoBehaviour.FindObjectOfType<PathFollower>();
                    testCar.SetList(new List<Tile>() {tileComp});
                }
            }
        }
    }
}