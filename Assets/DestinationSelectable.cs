using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationSelectable : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        Debug.Log("You clicked the secret tile");
        
        SelectionManager.currentSelected.GetComponentInChildren<PathFollower>().SetList(new List<Tile>() {GetComponent<Tile>()});
    }
}
