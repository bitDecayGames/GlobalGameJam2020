using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectorTile : MonoBehaviour {
    private Tile tile;

    private void Start() {
        tile = GetComponentInChildren<Tile>();
    }

    private void OnMouseUpAsButton() {
        // only continue if mouse did not click the UI
        if (!EventSystem.current.IsPointerOverGameObject()) {
            var selectable = RecursiveFindDestinationSelectable(new List<Tile> {tile}, tile.neighbors());
            if (selectable != null) selectable.Select();
        } else {
            Debug.Log("UI Intercepted click");
        }
    }

    private DestinationSelectable RecursiveFindDestinationSelectable(List<Tile> visited, List<Tile> toVist) {
        List<Tile> newToVisit = new List<Tile>();
        foreach (Tile tv in toVist) {
            if (tv.tileType == TileType.DOOR) {
                var ds = tv.GetComponent<DestinationSelectable>();
                if (ds != null) return ds;
            } else if (tv.tileType == TileType.CONNECTION) {
                visited.Add(tv);
                tv.neighbors()
                    .ForEach(n => {
                        if (!visited.Contains(n) && !newToVisit.Contains(n)) newToVisit.Add(n);
                    });
            }
        }

        if (newToVisit.Count == 0) return null;
        return RecursiveFindDestinationSelectable(visited, newToVisit);
    }
}