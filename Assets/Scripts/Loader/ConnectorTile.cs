using System.Collections.Generic;
using UnityEngine;

public class ConnectorTile : MonoBehaviour {
    private Tile tile;

    private void Start() {
        tile = GetComponentInChildren<Tile>();
    }

    private void OnMouseUpAsButton() {
        var selectable = RecursiveFindDestinationSelectable(new List<Tile> {tile}, tile.neighbors());
        if (selectable != null) selectable.Select();
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