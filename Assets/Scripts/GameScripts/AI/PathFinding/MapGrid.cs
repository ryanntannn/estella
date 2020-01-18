using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//main map grid used for path finding
[System.Serializable]
public class MapGrid : MonoBehaviour {
    public Vector2 mapSize = new Vector2(50, 50);
    public float nodeSize = 2;
    public LayerMask obstacles = Layers.Terrain;
    public bool debug = true;
    Node[,] grid;
    public List<Enemy> enemies = new List<Enemy>();

    public void Start() {
        InitGrid();
    }

    public void OnDrawGizmos() {
        if (grid != null && debug) {
            foreach (Node n in grid) {
                n.DrawGizmos(nodeSize);
            }
        }
    }

    public void InitGrid() {
        Vector3 bottomLeft = transform.position - 
            transform.right * (mapSize.x / 2) - 
            transform.forward * (mapSize.y / 2);
        int noOfX = (int)(mapSize.x / nodeSize);
        int noOfY = (int)(mapSize.y / nodeSize);

        grid = new Node[noOfX, noOfY];
        for (int x = 0; x < noOfX; x++) {
            for (int y = 0; y < noOfY; y++) {
                Vector3 worldPos = bottomLeft +
                    (transform.right * (x * nodeSize + nodeSize / 2)) +
                    (transform.forward * (y * nodeSize + nodeSize / 2));
                bool walkable = !Physics.CheckBox(worldPos, Vector3.one * (nodeSize / 2), Quaternion.identity, obstacles);
                Vector2Int gridPos = new Vector2Int(x, y);
                grid[x, y] = new Node(worldPos, gridPos, walkable);
            }
        }
    }

    public void LoadGrid() {

    }

    public void SaveGrid() {

    }

    //get node neighbours given a node
    public Node[] GetNeighbours(Node _input) {
        List<Node> returnNodes = new List<Node>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                //no diagonal movement
                if (x == y) continue;
                if (!(x == 0 || y == 0)) continue;
                try {
                    returnNodes.Add(grid[_input.gridPos.x + x, _input.gridPos.y + y]);
                } catch (IndexOutOfRangeException) { };
            }
        }

        return returnNodes.ToArray();
    }

    public Node WorldPointToNode(Vector3 _input) {
        Vector3 bottomLeft = transform.position - transform.right * (mapSize.x / 2) - transform.forward * (mapSize.y / 2);
        _input -= bottomLeft;
        return grid[(int)(_input.x / (nodeSize * (3 / 2))), (int)(_input.z / (nodeSize * (3 / 2)))];
    }
}

//base node for map grid
[System.Serializable]
public class Node : IComparable {
    public Vector3 worldPos;
    public Vector2Int gridPos;
    public bool walkable;
    public Node parentNode;

    //AStar
    public float G = 0, H = 0, Cost = 1;
    public float F { get { return G + H + Cost; } }

    public Node(Vector3 _worldPos, Vector2Int _gridPos, bool _walkable) {
        worldPos = _worldPos;
        gridPos = _gridPos;
        walkable = _walkable;
    }

    public void DrawGizmos(float radius) {
        Gizmos.color = walkable ? Color.green : Color.red;
        Gizmos.DrawWireCube(worldPos, Vector3.one * radius * 0.9f);
    }


    public int CompareTo(object obj) {
        Node node = obj as Node;
        if (node.F > F) {
            return -1;
        } else if (node.F < F) {
            return 1;
        }
        return node.H > H ? 1 : -1;
    }
}
