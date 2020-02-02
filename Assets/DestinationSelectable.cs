using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestinationSelectable : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        Debug.Log("You clicked the secret tile");
        
        var pather = SelectionManager.currentSelected.GetComponentInChildren<PathFollower>();
        var pathfinder = new PathFinder();
        var newPath = pathfinder.getTilePath(pather.path[pather.path.Count - 1], GetComponent<Tile>());
        
        pather.SetList(newPath);
    }
}
