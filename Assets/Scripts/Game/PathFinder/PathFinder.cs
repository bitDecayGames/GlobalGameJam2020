using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
public class PathFinder {
    public static void Test()
    {
        Console.WriteLine("Hello Test!");
        var expected = new List<Node>();
        //expected.Add(new Node(0, 0, new Tile()));
        var got = new List<Node>();
       
        assertEquals(expected, got);
    }

    public static void assertEquals(List<Node> a, List<Node> b) {
        if (!a.SequenceEqual(b)) {
            throw new Exception($"{a} does not equal {b}. That's bad");
        }
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
    private int heuristic(Node n, Node goal) {
        // Simple Manhattan Distance
        return Math.Abs(n.x - goal.x) + Math.Abs(n.y - goal.y);
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
        var validNeighbors = possibleNeighbors.Where(n => n != null || n.tile.type != TileType.SOLID).ToList();
       
        foreach (var neighbor in validNeighbors) {
            neighbor.g = node.g + 1;
            neighbor.parent = node;
         }
         
         return validNeighbors;
    }

    public class Node {
        public int x;
        public int y;

        public int f; // the total cost of the path via this node
        public int g; // the incremental cost of moving from the start node to this node
        public int h;  // the distance between this node and the goal
        public Node parent;
        public Node north = null;
        public Node south = null;
        public Node east = null;
        public Node west = null;
    
        public Tile tile;

        public Node(int x, int y, Tile tile) {
            this.x = x;
            this.y = y;

            this.tile = tile;
        }

        public bool sameLocation(Node other) {
            return this.x == other.x && this.y == other.y;
        }

        public override string ToString() {
            return $"({x}, {y})";
        }
    }

    public class Tile {
        public Tile north = null;
        public Tile south = null;
        public Tile east = null;
        public Tile west = null;
        public TileType type = TileType.EMPTY;
    }
    
    public enum TileType {
        SOLID,
        EMPTY
    } 
}
