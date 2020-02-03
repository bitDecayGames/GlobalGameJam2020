using System;
using System.Collections;
using System.Collections.Generic;
using SuperTiled2Unity;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    private SuperMap _map;
    private SuperTileLayer _baseDataLayer;
    public List<GameObject> truckPrefabs;
    public GameObject storeInventory;
    public GameObject HQInventory;

    private Vector2 truckSpawn = new Vector2();
    
    // Start is called before the first frame update
    void Start()
    {
        _map = FindObjectOfType<SuperMap>();
        if (_map == null)
        {
            Debug.LogError("Unable to find tilemap... What did you do...");
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

            var tileComp = currentTile.gameObject.AddComponent<Tile>();
            tileComp.connections = getPropertyAsInt("conns", currentTile);
            tileComp.tileType = getPropertyAsTileType("type", currentTile);

            if (tileComp.tileType == TileType.DOOR || tileComp.tileType == TileType.ROAD)
            {
                currentTile.gameObject.AddComponent<DestinationSelectable>();
                currentTile.gameObject.AddComponent<BoxCollider2D>();
            } else if (tileComp.tileType == TileType.CONNECTION) {
                currentTile.gameObject.AddComponent<ConnectorTile>();
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
        
        var jobMgr = FindObjectOfType<JobManager>();
        if (jobMgr == null)
        {
            throw new Exception("No job manager found in the world");
        }

        for (var i = 0; i < _baseDataLayer.transform.childCount; i++)
        {
            var currentTile = _baseDataLayer.transform.GetChild(i);
            currentTile.GetComponent<SpriteRenderer>().sortingOrder = 0;
            
            if (getPropertyAsString("type", currentTile) == "DOOR")
            {
                if (getPropertyAsBool("jobbing", currentTile))
                {
                    // Debug.Log("Adding a location");
                    jobMgr.AddPossibleLocation(currentTile.gameObject);
                }
                else {
                    var nameProp = getPropertyAsString("name", currentTile);
                    if (nameProp == "shop") {
                        Instantiate(storeInventory, currentTile.transform);
                    } else if (nameProp == "hq") {
                        Instantiate(HQInventory, currentTile.transform);
                    }
                }
            }
        }
        
        var buildingLayer = _map.transform.Find("Grid").Find("buildings").GetComponent<SuperObjectLayer>();
        
        for (var i = 0; i < buildingLayer.transform.childCount; i++)
        {
            var currentTile = buildingLayer.transform.GetChild(i);
            if (getPropertyAsBool("tall", currentTile))
            {
                currentTile.GetComponentInChildren<SpriteRenderer>().sortingOrder = 3;
            }
            else
            {
                currentTile.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
            }

            var specialBuildingProp = getPropertyAsString("specialBuilding", currentTile);
            if (specialBuildingProp == "tools") {
                currentTile.gameObject.AddComponent<ToolsNStuffMarker>(); // so that we can find the tools building easily
            } else if (specialBuildingProp == "dans") {
                currentTile.gameObject.AddComponent<HQMarker>(); // so that we can find the tools building easily
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
                
                var truckInventory = CreateSingleTruck(x, y, 0);
                // give first truck one of each inventory item
                truckInventory.SetSlot(0, InventoryType.WRENCH);
                truckInventory.SetSlot(1, InventoryType.LIGHT_BULB);
                truckInventory.SetSlot(2, InventoryType.PAINT);
            }
        }
    }

    public Inventory CreateTruckDefaultSpawn(int truckIndex)
    {
        return CreateSingleTruck((int) truckSpawn.x, (int) truckSpawn.y, truckIndex);
    }

    public Inventory CreateSingleTruck(int cellX, int cellY, int truckIndex) {
        GameObject truckPrefab;
        if (truckPrefabs == null || truckPrefabs.Count == 0) throw new Exception("Truck prefab list cannot be empty");
        if (truckIndex < truckPrefabs.Count) truckPrefab = truckPrefabs[truckIndex];
        else truckPrefab = truckPrefabs[0];
        var truck = Instantiate(truckPrefab);

        var truckStartCell = _baseDataLayer.transform.GetChild((int)(cellY + cellX * _map.m_Height));
        var startPos = truckStartCell.transform.position;
        startPos.x += 0.5f;
        startPos.y += 0.5f;
        startPos.z = -1;
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

    private static CustomProperty getProperty(string name, Transform currentTile) {
        var props = currentTile.GetComponent<SuperCustomProperties>();
        CustomProperty prop;
        if (!props.TryGetCustomProperty(name, out prop)) {
            prop = new CustomProperty();
        }

        return prop;
    }

    private static String getPropertyAsString(string name, Transform currentTile) {
        var prop = getProperty(name, currentTile);
        if (prop.IsEmpty) return null;
        return prop.GetValueAsString();
    }

    private static bool getPropertyAsBool(string name, Transform currentTile) {
        var prop = getProperty(name, currentTile);
        if (prop.IsEmpty) return false;
        return prop.GetValueAsBool();
    }

    private static int getPropertyAsInt(string name, Transform currentTile) {
        var prop = getProperty(name, currentTile);
        if (prop.IsEmpty) return 0;
        return prop.GetValueAsInt();
    }
    private static TileType getPropertyAsTileType(string name, Transform currentTile) {
        var prop = getProperty(name, currentTile);
        if (prop.IsEmpty) return TileType.SOLID;
        
        TileType tType;
        TileType.TryParse(prop.GetValueAsString(), true, out tType);
        return tType;
    }
}