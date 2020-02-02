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
                //currentTile.gameObject.AddComponent<DestinationSelectable>();
                var doorCollider = currentTile.gameObject.AddComponent<BoxCollider2D>();
                doorCollider.isTrigger = true;
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

        var allDoors = new List<GameObject>();
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
                allDoors.Add(currentTile.gameObject);
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
            MakeBuildingSelectable(currentTile, allDoors);
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

    void MakeBuildingSelectable(Transform buildingTile, List<GameObject> allDoors)
    {
        var buildingSelectable = buildingTile.gameObject.AddComponent<BuildingSelectable>();
        var buildingCollider = buildingTile.gameObject.AddComponent<BoxCollider2D>();
        var rigid = buildingTile.gameObject.AddComponent<Rigidbody2D>();
        rigid.isKinematic = true;
        //rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        buildingCollider.isTrigger = true;

        // set up the colider to match the size and location of the sprite
        var s = buildingTile.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size;
        buildingCollider.size = s;
        buildingCollider.offset = s / 2;
        /*
        Debug.Log("looking for a door");
        foreach(var door in allDoors)
        {
            var doorCollider = door.gameObject.GetComponentInChildren<BoxCollider2D>();
            if (doorCollider.IsTouching(buildingCollider))
            {
                Debug.Log("found a door!");
                buildingSelectable.door = door.gameObject;
            }
        }
        */
    }

    void CreateTrucks()
    {
        var objs = _map.transform.Find("Grid").Find("poi").GetComponent<SuperObjectLayer>();
        for (int i = 0; i < objs.transform.childCount; i++)
        {
            var childObj = objs.transform.GetChild(i);
            if (childObj.name == "truckSpawn")
            {
                var truck = Instantiate(truckPrefab);
                truck.transform.position = childObj.transform.position;

                var x = Math.Floor(truck.transform.position.x);
                var y = Math.Floor(Math.Abs(truck.transform.position.y));

                var truckStartCell = _baseDataLayer.transform.GetChild((int)(y + x * _map.m_Height));
                truck.GetComponent<PathFollower>().SetList(new List<Tile>() {truckStartCell.gameObject.GetComponent<Tile>()});

                truck.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }

        
    }
}
