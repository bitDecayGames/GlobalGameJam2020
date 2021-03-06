﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DestinationSelectable : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI Intercepted click");
            return;
        }

        Select();
    }

    public void Select() {
        Debug.Log($"Selected tile {name}");
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
        var newPath = pathfinder.getTilePath(pather.path[nodeToStart], GetComponent<Tile>());
        
        pather.SetList(newPath);
        // Debug.Log("Setting destination from DestinationSelectable");
        pather.SetDestinationObject(gameObject);
    }
}
