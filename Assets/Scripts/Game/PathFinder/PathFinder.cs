using System.Collections.Generic;
using System;
using System.Linq;

public class PathFinder {
    public List<Tile> getTilePath(Tile start, Tile goal) {
        var startNode = new Node(start);
        var goalNode = new Node(goal);
        var path = this.getPath(startNode, goalNode);
        if (path == null)
        {
           UnityEngine.Debug.Log("NO PATH WAS FOUND TO: " + goal);
            return new List<Tile>() {start};
        }
        return path.Select(n => n.tile).ToList();
    }
    
    public List<Node> getPath(Node start, Node goal) {  
        var openList = new List<Node>(); // set containing start
        openList.Add(start);
        var closedList = new List<Node>(); // empty set
        start.f = start.g + heuristic(start, goal);

        while (openList.Count > 0) {
            var current = nodeWithLowestFCost(openList);
            if (current.sameLocation(goal)) {
                return constructPath(current);
            }
            openList.Remove(current);
            closedList.Add(current);
            foreach( var neighbor in neighbors(current)) {
                if (contains(closedList, neighbor) == null) {
                    neighbor.f = neighbor.g + heuristic(neighbor, goal);
                    if (contains(openList, neighbor) == null) {
                        openList.Add(neighbor);
                    } else {
                        var openNeighbor = contains(openList, neighbor);
                        if (neighbor.g < openNeighbor.g) {
                            openNeighbor.g = neighbor.g;
                            openNeighbor.parent = neighbor.parent;
                        }
                    }
                }  
            }
        }

        return null; // no path exists
    }

    private Node nodeWithLowestFCost(List<Node> list) {
        return list.OrderBy(n => n.g).First();
    }

    private List<Node> constructPath(Node node) {
        var path = new List<Node>(); // set containing node
        if (node.tile.tileType != TileType.DOOR)
        {
            path.Add(node);
        }
        while(node.parent != null) {
            node = node.parent;
            path.Add(node);
        }
        path.Reverse();
        return path;
    }
    private float heuristic(Node n, Node goal) {
        // Simple Manhattan Distance
        return Math.Abs(n.getX() - goal.getX()) + Math.Abs(n.getY() - goal.getY());
    }

    private Node contains(List<Node> list, Node searchFor) {
        foreach (var current in list) {
            if (current.sameLocation(searchFor)) {
                return current;
            }
        }
        return null;
    }

    private List<Node> neighbors(Node node) {
        var possibleNeighbors = new List<Node>();
        if ((node.tile.connections & 1) != 0)
        {
            possibleNeighbors.Add(new Node(node.tile.north));
        }
        if ((node.tile.connections & 2) != 0)
        {
            possibleNeighbors.Add(new Node(node.tile.east));
        }
        if ((node.tile.connections & 4) != 0)
        {
            possibleNeighbors.Add(new Node(node.tile.south));
        }
        if ((node.tile.connections & 8) != 0)
        {
            possibleNeighbors.Add(new Node(node.tile.west));
        }
        var validNeighbors = possibleNeighbors.Where(n => n.tile != null && n.tile.tileType != TileType.SOLID).ToList();
       
        foreach (var neighbor in validNeighbors) {
            neighbor.g = node.g + 1;
            neighbor.parent = node;
         }
         
         return validNeighbors;
    }

    public class Node {
        public float f = 0; // the total cost of the path via this node
        public float g = 0; // the incremental cost of moving from the start node to this node
        public float h = 0;  // the distance between this node and the goal
        public Node parent;
    
        public Tile tile;

        public Node(Tile tile)
        {
            this.tile = tile;
        }

        public float getX() {
            return this.tile.transform.position.x;
        }

        public float getY()
        {
            return this.tile.transform.position.y;
        }

        public bool sameLocation(Node other)
        {
            return this.getX() == other.getX() && this.getY() == other.getY();
        }
    }
}
