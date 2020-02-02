using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestinationSelectable : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {   
        var pather = SelectionManager.currentSelected.GetComponentInChildren<PathFollower>();
        var pathfinder = new PathFinder();

        var nodeToStart = pather.index;
        if (nodeToStart >= pather.path.Count)
        {
            nodeToStart = Math.Max(0, pather.path.Count - 1);
        }
        var newPath = pathfinder.getTilePath(pather.path[nodeToStart], GetComponent<Tile>());
        
        pather.SetList(newPath);
        pather.SetDestinationObject(gameObject);
    }
}
