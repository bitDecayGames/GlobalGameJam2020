using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public TileType tileType;
    public Tile west;
    public Tile north;
    public Tile east;
    public Tile south;

    public List<Tile> neighbors() {
        return new List<Tile>{west, north, east, south};
    }
}

public enum TileType {
    SOLID,
    DOOR,
    ROAD,
}
