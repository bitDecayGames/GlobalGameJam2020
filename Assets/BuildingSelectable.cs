using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SuperTiled2Unity;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingSelectable : DestinationSelectable
{
    public GameObject door;
    public float doorDistance = 9999999999.0f;

    
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
    
    private void OnMouseUpAsButton()
    {
        SelectDestination(this.door);
    }
}