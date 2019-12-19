using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Algorithms {
    //public static List<Node> Dijkstras(MapGrid grid, Vector3 start, Vector3 end) {
    //    Node startNode = grid.WorldPointToNode(start);
    //    Node endNode = grid.WorldPointToNode(end);

    //    Queue<Node> openList = new Queue<Node>();
    //    List<Node> closedList = new List<Node>();
    //    openList.Enqueue(startNode);
    //    Node currentNode = openList.Dequeue();
    //}

    public static List<Node> AStar(MapGrid grid, Vector3 start, Vector3 end, int threshold = 1000) {
        Node startNode = grid.WorldPointToNode(start);
        Node endNode = grid.WorldPointToNode(end);

        PiorityQueue<Node> openList = new PiorityQueue<Node>();
        List<Node> closedList = new List<Node>();

        openList.Enqueue(startNode);
        int count = 0;
        while (openList.Count > 0) {
            //error checking
            if (++count > threshold) {
                throw new AStarException("Threshold reached");
            }

            if (closedList.Contains(endNode)) break;

            Node currentNode = openList.Dequeue();
            foreach (Node n in grid.GetNeighbours(currentNode)) {
                if (n.walkable && !closedList.Contains(n)) {
                    float newG = GetEuclidian(grid, n, endNode);
                    float newH = GetManhatten(grid, startNode, n);
                    //if openlist alr contains n
                    if (openList.Contains(n)) {
                        //check f value
                        if (n.F > newG + newH) {
                            //this path is better, change it
                            n.G = newG;
                            n.H = newH;
                            n.parentNode = currentNode;
                        }
                    } else {
                        //add n into openlist
                        n.G = newG;
                        n.H = newH;
                        n.parentNode = currentNode;
                        try {
                            openList.Enqueue(n);
                        } catch (Exception) {
                            Debug.Log(count);
                        }
                    }
                }
            }

            //add currentnode to closed list
            closedList.Add(currentNode);
        }

        //make path
        List<Node> path = new List<Node>();
        Node pathNode = endNode;
        while (pathNode != startNode) {
            path.Add(pathNode);
            pathNode = pathNode.parentNode;
        }
        path.Add(startNode);
        path.Reverse();
        return path;
    }

    private static float GetManhatten(MapGrid grid, Node start, Node end) {
        Vector2 gridSize = (grid.mapSize / grid.nodeSize);
        float x = Mathf.Abs(start.gridPos.x - end.gridPos.x);
        x /= gridSize.x;    //normalise it
        float y = Mathf.Abs(start.gridPos.y - end.gridPos.y);
        y /= gridSize.y;

        return (x + y);
    }

    private static float GetEuclidian(MapGrid grid, Node start, Node end) {
        Vector2 gridSize = (grid.mapSize / grid.nodeSize);
        float eucDist = (start.gridPos - end.gridPos).magnitude;
        return eucDist / gridSize.magnitude;
    }
}

class AStarException : Exception {
    public AStarException(string _message) : base(_message) { }
}
