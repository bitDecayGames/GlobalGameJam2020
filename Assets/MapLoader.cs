using System;
using System.Collections;
using System.Collections.Generic;
using SuperTiled2Unity;
using UnityEngine;

public class MapLoader : MonoBehaviour
{    
    private SuperMap _map;
    private SuperTileLayer _baseDataLayer;
    public GameObject tilePrefab;

    
    // Start is called before the first frame update
    void Start()
    {
        _map = FindObjectOfType<SuperMap>();
        if (_map == null)
        {
            Debug.Log("Unable to find tilemap... What did you do...");
            return;
        }

        _baseDataLayer = _map.transform.Find("Grid").Find("data").GetComponent<SuperTileLayer>();

        var testList = new List<Tile>();
        
        // Rig up all the cardinal directions. THIS LOOP ASSUMES some things:
        // children are in Columns from left to right
        for (var i = 0; i < _baseDataLayer.transform.childCount; i++)
        {
            var currentTile = _baseDataLayer.transform.GetChild(i);
            var props = currentTile.GetComponent<SuperCustomProperties>();
            
            CustomProperty solidProp;
            var found = props.TryGetCustomProperty("type", out solidProp);
            if (!found)
            {
                throw new Exception($"Unable to find 'solid' property on custom properties of object");
            }
            
            var tileComp = currentTile.gameObject.AddComponent<Tile>();
            TileType tType;
            TileType.TryParse(solidProp.GetValueAsString(), true, out tType);
            tileComp.tileType = tType;
            if (i % _map.m_Height != 0)
            {
                tileComp.north = _baseDataLayer.transform.GetChild(i - 1).GetComponent<Tile>();
                _baseDataLayer.transform.GetChild(i - 1).GetComponent<Tile>().south = tileComp;
            }

            if ((i - _map.m_Height) >= 0)
            {
                tileComp.west = _baseDataLayer.transform.GetChild(i - _map.m_Height).GetComponent<Tile>();
                _baseDataLayer.transform.GetChild(i - _map.m_Height).GetComponent<Tile>().east = tileComp;
            }

            testList.Add(tileComp);
        }
        
        var testCar = FindObjectOfType<PathFollower>();
        testCar.SetList(testList);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
