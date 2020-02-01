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

        // Rig up all the cardinal directions. THIS LOOP ASSUMES some things:
        // children are in Columns from left to right
        for (var i = 0; i < _baseDataLayer.transform.childCount; i++)
        {
            var currentTile = _baseDataLayer.transform.GetChild(i);
            currentTile.gameObject.AddComponent<Tile>();
            if (i % _map.m_Height != 0)
            {
                currentTile.GetComponent<Tile>().north = _baseDataLayer.transform.GetChild(i - 1).GetComponent<Tile>();
                _baseDataLayer.transform.GetChild(i - 1).GetComponent<Tile>().south = currentTile.GetComponent<Tile>();
            }

            if ((i - _map.m_Height) >= 0)
            {
                currentTile.GetComponent<Tile>().west = _baseDataLayer.transform.GetChild(i - _map.m_Height).GetComponent<Tile>();
                _baseDataLayer.transform.GetChild(i - _map.m_Height).GetComponent<Tile>().east = currentTile.GetComponent<Tile>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
