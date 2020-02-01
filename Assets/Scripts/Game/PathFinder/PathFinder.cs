using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class PathFinder {
    public IEnumerable<Tile> getTilePath(Tile start, Tile goal) {
        var startNode = new Node(start);
        var goalNode = new Node(goal);
        var path = this.getPath(startNode, goalNode);
        return path.Select(n => n.tile);
    }
    
    public List<Node> getPath(Node start, Node goal) {  
        var openList = new List<Node>(); // set containing start
        openList.Add(start);
        var closedList = new List<Node>(); // empty set
        start.f = start.g + heuristic(start, goal);

        while (openList.Count > 0) {
            var current = nodeWithLowestFCost(openList);
            if (current.sameLocation(goal)) {
                return constructPath(goal);
            }
            openList.Remove(current);
            closedList.Add(current);
            foreach( var neighbor in neighbors(current)) {
                if (contains(closedList, neighbor) != null) {
                    neighbor.f = neighbor.g + heuristic(neighbor, goal);
                    if (contains(openList, neighbor) != null) {
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
        path.Add(node);
        while(node.parent != null) {
            var parentNode = node.parent;
            path.Add(parentNode);
        }
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
        possibleNeighbors.Add(node.north);
        possibleNeighbors.Add(node.south);
        possibleNeighbors.Add(node.east);
        possibleNeighbors.Add(node.west);
        var validNeighbors = possibleNeighbors.Where(n => n != null || n.tile.tileType != TileType.SOLID).ToList();
       
        foreach (var neighbor in validNeighbors) {
            neighbor.g = node.g + 1;
            neighbor.parent = node;
         }
         
         return validNeighbors;
    }

    public class Node {
        public float f; // the total cost of the path via this node
        public float g; // the incremental cost of moving from the start node to this node
        public float h;  // the distance between this node and the goal
        public Node parent;
        public Node north = null;
        public Node south = null;
        public Node east = null;
        public Node west = null;
    
        public Tile tile;

        public Node(Tile tile)
        {
            if (tile.north != null) this.north = new Node(tile.north);
            if (tile.east != null) this.east = new Node(tile.east);
            if (tile.south != null) this.south = new Node(tile.south);
            if (tile.west != null) this.west = new Node(tile.west);
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
