using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SuperTiled2Unity;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingSelectable : MonoBehaviour
{
    public GameObject door;
    public float doorDistance = 9999999999.0f;

    public void SelectDestination(GameObject destination)
    {
        Debug.Log("clicked on a destination");
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (SelectionManager.currentSelected == null)
        {
            // no vehicle selected
            // TODO: show this building's inventory if it has one
            // Ideally, with a timer, as we have no way to deselect buildings as they aren't selectable

            return;
        }

        var pather = SelectionManager.currentSelected.GetComponentInChildren<PathFollower>();
        var pathfinder = new PathFinder();

        var nodeToStart = pather.index;
        if (nodeToStart >= pather.path.Count)
        {
            nodeToStart = Math.Max(0, pather.path.Count - 1);
        }
        var newPath = pathfinder.getTilePath(pather.path[nodeToStart], destination.GetComponent<Tile>());

        pather.SetList(newPath);
        pather.SetDestinationObject(destination);

    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("clicked on a building:" + this.door);
            var buildingCollider = this.gameObject.GetComponentInChildren<BoxCollider2D>();
            SelectDestination(this.door);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var potentialDoor = collision.gameObject;
        
        var doorTile = potentialDoor.GetComponent<Tile>();
        if (!doorTile || doorTile.tileType != TileType.DOOR)
        {
            return;
        }


        Debug.Log("collided with a door!");
        var buildingCollider = gameObject.GetComponent<BoxCollider2D>();
        var currentDistance = collision.Distance(buildingCollider).distance;

        if (currentDistance < doorDistance)
        {
            Debug.Log("collided with a *close* door!");
            this.door = collision.gameObject;
            this.doorDistance = currentDistance;

            // Remove the colider from the door
            var doorCollider = this.door.GetComponent<BoxCollider2D>();
            if (doorCollider)
            {
                doorCollider.enabled = false;
            }

        }


    }
    
    // private void OnMouseUpAsButton()
    // {
    //     Debug.Log("clicked on a building:" + this.door);
    //     var buildingCollider = this.gameObject.GetComponentInChildren<BoxCollider2D>();
    //     SelectDestination(this.door);
    // }
}