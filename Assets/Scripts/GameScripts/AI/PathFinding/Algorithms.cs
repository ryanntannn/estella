using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Algorithms {
    public static List<Node> Dijkstras(MapGrid grid, Vector3 start, Vector3 end, int threshold = 1000) {
        Node startNode = grid.WorldPointToNode(start);
        Node endNode = grid.WorldPointToNode(end);
        //make sure walkable
        startNode = FindClosestWalkable(grid, startNode);
        endNode = FindClosestWalkable(grid, endNode);

        Queue<Node> openList = new Queue<Node>();
        List<Node> closedList = new List<Node>();
        openList.Enqueue(startNode);
        int count = 0;
        while (openList.Count > 0)  {
            if(++count >= threshold) {
                throw new AlgorithmExecption("Threshold reached");
            }

            if (closedList.Contains(endNode)) break;

            Node currentNode = openList.Dequeue();
            //neighbours
            foreach(Node n in grid.GetNeighbours(currentNode)) {
                if (n.walkable && !closedList.Contains(n)) {
                    n.parentNode = currentNode;
                    openList.Enqueue(n);
                }
            }

            //add to closed list
            closedList.Add(currentNode);
        }

        return TracePath(startNode, endNode);
    }

    public static List<Node> Dijkstras(MapGrid grid, Vector3 start, Vector3[] end, int threshold = 1000) {
        Node startNode = grid.WorldPointToNode(start);
        startNode = FindClosestWalkable(grid, startNode);
        Node[] endNodes = new Node[end.Length];
        for(int count = 0; count <= end.Length - 1; count++) {
            endNodes[count] = FindClosestWalkable(grid, grid.WorldPointToNode(end[count]));
        }

        Queue<Node> openList = new Queue<Node>();
        List<Node> closedList = new List<Node>();
        openList.Enqueue(startNode);
        int i = 0;
        while (openList.Count > 0) {
            if (++i >= threshold) {
                throw new AlgorithmExecption("Threshold reached");
            }

            //check if any of the items in closed list match the end
            foreach(Node n in endNodes) {
                if (closedList.Contains(n)) return TracePath(startNode, n);
            }

            Node currentNode = openList.Dequeue();
            //neighbours
            foreach (Node n in grid.GetNeighbours(currentNode)) {
                if (n.walkable && !closedList.Contains(n)) {
                    n.parentNode = currentNode;
                    openList.Enqueue(n);
                }
            }

            //add to closed list
            closedList.Add(currentNode);
        }

        //item not found
        return null;
    }

    public static List<Node> AStar(MapGrid grid, Vector3 start, Vector3 end, int threshold = 1000) {
        Node startNode = grid.WorldPointToNode(start);
        Node endNode = grid.WorldPointToNode(end);
        startNode = FindClosestWalkable(grid, startNode);
        endNode = FindClosestWalkable(grid, endNode);

        PiorityQueue<Node> openList = new PiorityQueue<Node>();
        List<Node> closedList = new List<Node>();

        openList.Enqueue(startNode);
        int count = 0;
        while (openList.Count > 0) {
            //error checking
            if (++count > threshold) {
                throw new AlgorithmExecption("Threshold reached");
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

        return TracePath(startNode, endNode);
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

    private static Node FindClosestWalkable(MapGrid grid, Node _input) {
        if (_input.walkable) return _input; //this is walkable

        //find neighbours
        foreach(Node n in grid.GetNeighbours(_input)) {
            return FindClosestWalkable(grid, n);
        }

        return null;
    }

    private static List<Node> TracePath(Node _start, Node _end) {
        List<Node> path = new List<Node>();
        Node currentNode = _end;
        while(currentNode != _start) {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
        path.Add(_start);
        path.Reverse();
        return path;
    }
}

class AlgorithmExecption : Exception {
    public AlgorithmExecption(string _message) : base(_message) { }
}
