using System;
using System.Collections;
using System.Collections.Generic;
using SuperTiled2Unity;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    private SuperMap _map;
    private SuperTileLayer _baseDataLayer;
    public GameObject truckPrefab;
    public GameObject storeInventory;
    public GameObject HQInventory;

    private Vector2 truckSpawn = new Vector2();
    
    // Start is called before the first frame update
    void Start()
    {
        _map = FindObjectOfType<SuperMap>();
        if (_map == null)
        {
            Debug.Log("Unable to find tilemap... What did you do...");
            return;
        }

        LoadTiles();
        LoadBuildings();
        CreateTrucks();
        UpdateEnvironment();
    }

    void LoadTiles()
    {
        _baseDataLayer = _map.transform.Find("Grid").Find("data").GetComponent<SuperTileLayer>();

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
            
            if (i % _map.m_Height != 0)
            {
                tileComp.north = _baseDataLayer.transform.GetChild(i - 1).GetComponent<Tile>();

                neighbor = _baseDataLayer.transform.GetChild(i - 1).GetComponent<Tile>();
                neighbor.south = tileComp;

                if (neighbor.tileType == TileType.DOOR)
                {
                    tileComp.connections |= 1; // add a connection to the building to the north of us
                }
            }

            if ((i - _map.m_Height) >= 0)
            {
                tileComp.west = _baseDataLayer.transform.GetChild(i - _map.m_Height).GetComponent<Tile>();
                _baseDataLayer.transform.GetChild(i - _map.m_Height).GetComponent<Tile>().east = tileComp;
            }
        }
    }

    void LoadBuildings()
    {
        _baseDataLayer = _map.transform.Find("Grid").Find("data").GetComponent<SuperTileLayer>();
        
        var jobMgr = UnityEngine.Object.FindObjectOfType<JobManager>();
        if (jobMgr == null)
        {
            throw new Exception("No job manager found on the main camera");
        }

        var NonJobIndex = 0;

        for (var i = 0; i < _baseDataLayer.transform.childCount; i++)
        {
            var currentTile = _baseDataLayer.transform.GetChild(i);
            currentTile.GetComponent<SpriteRenderer>().sortingOrder = 0;
            var props = currentTile.GetComponent<SuperCustomProperties>();

            CustomProperty typeProp;
            if (!props.TryGetCustomProperty("type", out typeProp))
            {
                throw new Exception("No type property on tile: " + currentTile);
            }
            
            

            if (typeProp.GetValueAsString() == "DOOR")
            {
                CustomProperty jobbingProp;
                if (!props.TryGetCustomProperty("jobbing", out jobbingProp))
                {
                    throw new Exception("No jobbing property on door tile: " + currentTile);
                }

                if (jobbingProp.GetValueAsBool())
                {
                    Debug.Log("Adding a location");
                    jobMgr.AddPossibleLocation(currentTile.gameObject);
                }
                else
                {
                    NonJobIndex++;
                    if (NonJobIndex == 1)
                    {
                        // XXX: This is our Tool shop
                        Instantiate(storeInventory, currentTile.transform);
                    } else if (NonJobIndex == 2)
                    {
                        Instantiate(HQInventory, currentTile.transform);
                    }
                }
            }
        }
        
        var buildingLayer = _map.transform.Find("Grid").Find("buildings").GetComponent<SuperObjectLayer>();
        
        for (var i = 0; i < buildingLayer.transform.childCount; i++)
        {
            var currentTile = buildingLayer.transform.GetChild(i);
            var props = currentTile.GetComponent<SuperCustomProperties>();

            CustomProperty tallProp;
            if (!props.TryGetCustomProperty("tall", out tallProp))
            {
                throw new Exception("No tall property on tile: " + currentTile);
            }

            if (tallProp.GetValueAsBool())
            {
                currentTile.GetComponentInChildren<SpriteRenderer>().sortingOrder = 3;
            }
            else
            {
                currentTile.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
            }
        }
    }

    void CreateTrucks()
    {
        var objs = _map.transform.Find("Grid").Find("poi").GetComponent<SuperObjectLayer>();
        for (int i = 0; i < objs.transform.childCount; i++)
        {
            var childObj = objs.transform.GetChild(i);
            if (childObj.name == "truckSpawn")
            {
                var x = (int)Math.Floor(childObj.transform.position.x);
                var y = (int)Math.Floor(Math.Abs(childObj.transform.position.y));

                truckSpawn.Set(x, y);
                
                var truckInventory = CreateSingleTruck(x, y);
                truckInventory.SetSlot(0, InventoryType.WRENCH);
                truckInventory.SetSlot(1, InventoryType.LIGHT_BULB);
                truckInventory.SetSlot(2, InventoryType.PAINT);
            }
        }
    }

    public Inventory CreateTruckDefaultSpawn()
    {
        return CreateSingleTruck((int) truckSpawn.x, (int) truckSpawn.y);
    }

    public Inventory CreateSingleTruck(int cellX, int cellY)
    {
        var truck = Instantiate(truckPrefab);

        var truckStartCell = _baseDataLayer.transform.GetChild((int)(cellY + cellX * _map.m_Height));
        var startPos = truckStartCell.transform.position;
        startPos.x += 0.5f;
        startPos.y += 0.5f;
        truck.transform.position = startPos;
        truck.GetComponent<PathFollower>().SetList(new List<Tile>() {truckStartCell.gameObject.GetComponent<Tile>()});

        truck.GetComponent<SpriteRenderer>().sortingOrder = 2;
        return truck.GetComponentInChildren<Inventory>();
    }

    void UpdateEnvironment()
    {
        var objs = _map.transform.Find("Grid").Find("environment").GetComponent<SuperLayer>();
        for (int i = 0; i < objs.transform.childCount; i++)
        {
            var childObj = objs.transform.GetChild(i);
            if (Math.Abs(childObj.transform.position.y) > _map.m_Height / 2.0)
            {
                // lower half, this shiz renders high
                childObj.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
            else
            {
                // upper half, this shiz renders low, behind stuff
                childObj.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
        }
    }
}