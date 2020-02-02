using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DestinationSelectable : MonoBehaviour
{
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


    private void OnMouseUpAsButton()
    {
        SelectDestination(gameObject);
    }
}
