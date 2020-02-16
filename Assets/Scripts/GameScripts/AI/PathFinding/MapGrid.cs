using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

//main map grid used for path finding
[System.Serializable]
public class MapGrid : MonoBehaviour {
    public GameObject start, end;

    public Vector2 mapSize = new Vector2(50, 50);
    public float nodeSize = 2;
    public LayerMask obstacles = Layers.Terrain;
    public bool debug = true;
    Node[,] grid;
    public Node[,] Grid { get { return grid; } }
    public List<Enemy> enemies = new List<Enemy>();

    [HideInInspector]
    public float centerToSide;
    Vector3 bottomLeft;
    Vector3 right;
    Vector3 up;
    Vector3 forward;

    public void Awake() {
        bottomLeft = transform.position -
            transform.right * (mapSize.x / 2) -
            transform.forward * (mapSize.y / 2);
        right = transform.right;
        up = transform.up;
        forward = transform.forward;

        InitGrid();
    }

    public MapGrid() { }//default

    public void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(mapSize.x, 1, mapSize.y));
        if (grid != null && debug) {
            foreach (Node n in grid) {
                n.DrawGizmos(nodeSize);
            }

            //DEBUGGING SHIT
            //if (start && end) {
            //    Node startN = WorldPointToNode(start.transform.position);
            //    Node endN = WorldPointToNode(end.transform.position);

            //    List<Node> path = Algorithms.AStar(this, start.transform.position, end.transform.position);
            //    for (int count = 0; count <= path.Count - 2; count++) {
            //        Gizmos.color = Color.red;
            //        Gizmos.DrawLine(path[count].worldPos, path[count + 1].worldPos);
            //    }

            //    Gizmos.DrawCube(startN.worldPos, Vector3.one * 0.5f);
            //    Gizmos.DrawCube(endN.worldPos, Vector3.one * 0.5f);
            //}
        }
    }

    public void InitGrid() {
        centerToSide = Mathf.Sqrt(Mathf.Pow(nodeSize, 2) - Mathf.Pow(nodeSize / 2, 2));

        int noOfX = (int)(mapSize.x / (centerToSide * 2));
        int noOfY = (int)(mapSize.y / (nodeSize * 1.5f));

        grid = new Node[noOfX, noOfY];
        for (int x = 0; x < noOfX; x++) {
            for (int y = 0; y < noOfY; y++) {
                Vector3 worldPos = bottomLeft +
                (right * (x * centerToSide * 2 + centerToSide * (y & 1))) +
                (forward * (y * nodeSize * 1.5f));
                bool hasSpace = true;
                if (!Physics.CheckBox(worldPos, Vector3.one * nodeSize, Quaternion.identity, 1 << Layers.Terrain)) { //no terrain
                    //raycast down
                    float stepHeight = 3;
                    RaycastHit hitInfo;
                    if(Physics.Raycast(worldPos, -transform.up, out hitInfo, stepHeight)) {
                        worldPos = hitInfo.point;
                    }else {
                        hasSpace = false;
                    }
                }
                bool walkable = !Physics.CheckBox(worldPos, Vector3.one * nodeSize, Quaternion.identity, obstacles) && hasSpace;
                Vector2Int gridPos = new Vector2Int(x, y);
                grid[x, y] = new Node(worldPos, gridPos, walkable);
            }
        }

        //go to each unwalkable node and check if they have any walkable neighbours
        //if they do, raycast down and check again
        bool timeToGo = true;
        while (timeToGo) {
            timeToGo = false;
            foreach (Node n in grid) {
                if (!n.walkable) {
                    if (GetNeighbours(n).Any(x => x.walkable)) {
                        bool hasSpace = true;
                        //raycast down and check
                        if (!Physics.CheckBox(n.worldPos, Vector3.one * nodeSize, Quaternion.identity, 1 << Layers.Terrain)) { //no terrain
                            //raycast down
                            float stepHeight = 3;
                            RaycastHit hitInfo;
                            if (Physics.Raycast(n.worldPos, -transform.up, out hitInfo, stepHeight)) {
                                n.worldPos = hitInfo.point;

                                bool walkable = !Physics.CheckBox(n.worldPos, Vector3.one * nodeSize, Quaternion.identity, obstacles) && hasSpace;
                                n.walkable = walkable && hasSpace;

                                if (n.walkable) timeToGo = true;
                            } else {
                                hasSpace = false;
                            }
                        }
                    }
                }
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

        //-1
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (y == 0 && x == 0) continue; //itself
                if (y != 0 && x == (_input.gridPos.y % 2 == 0 ? 1 : -1)) continue; //corner

                try {
                    returnNodes.Add(grid[_input.gridPos.x + x, _input.gridPos.y + y]);
                } catch (IndexOutOfRangeException) { };
            }
        }

        return returnNodes.ToArray();
    }

    public Node WorldPointToNode(Vector3 _input) {
        _input -= bottomLeft;

        int y = (int)((_input.z + nodeSize) / (nodeSize * 1.5f));
        int x = (int)((_input.x + (centerToSide * (y % 2 == 0 ? 1 : 0))) / (centerToSide * 2));

        return grid[x, y];
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
    public float F { get { return G + H; } }

    public Node(Vector3 _worldPos, Vector2Int _gridPos, bool _walkable) {
        worldPos = _worldPos;
        gridPos = _gridPos;
        walkable = _walkable;
    }

    public void DrawGizmos(float radius) {
        Gizmos.color = walkable ? Color.green : Color.red;
        //radius *= 0.95f;
        //Gizmos.DrawWireCube(worldPos, Vector3.one * radius * 0.9f);
        float centerToSide = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(radius / 2, 2));
        //pointy tipped
        Vector3[] corners = new Vector3[]
        {
            worldPos + Vector3.forward * radius,
             worldPos + Vector3.forward * (radius / 2) + Vector3.right * centerToSide,
             worldPos - Vector3.forward * (radius / 2) + Vector3.right * centerToSide,
             worldPos - Vector3.forward * radius,
             worldPos - Vector3.forward * (radius / 2) - Vector3.right * centerToSide,
             worldPos + Vector3.forward * (radius / 2) - Vector3.right * centerToSide
        };

        for (int count = 0; count < 6; count++) {
            Gizmos.DrawLine(corners[count], corners[(count + 1) % 6]);
        }
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
