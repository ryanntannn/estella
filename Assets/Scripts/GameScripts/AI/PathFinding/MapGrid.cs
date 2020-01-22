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
        int noOfX = (int)(mapSize.x / (nodeSize * 2));
        int noOfY = (int)(mapSize.y / (nodeSize * 2));
		float centerToSide = Mathf.Sqrt(Mathf.Pow(nodeSize, 2) - Mathf.Pow(nodeSize / 2, 2));

		grid = new Node[noOfX, noOfY];
        for (int x = 0; x < noOfX; x++) {
            for (int y = 0; y < noOfY; y++) {
				Vector3 worldPos = Vector3.zero;
				if(y % 2 == 0) {
					worldPos = bottomLeft +
					(transform.right * (x * nodeSize * 2 + nodeSize / 2)) +
					(transform.forward * (y * nodeSize * 2 + nodeSize / 2));
				} else {
					worldPos = bottomLeft +
					(transform.right * (x * nodeSize * 2 + nodeSize / 2 + centerToSide)) +
					(transform.forward * (y * nodeSize * 2 + nodeSize / 2));
				}
                bool walkable = !Physics.CheckBox(worldPos, Vector3.one * nodeSize, Quaternion.identity, obstacles);
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
        Vector3 bottomLeft = transform.position - transform.right * (mapSize.x / 2) - transform.forward * (mapSize.y / 2);
        _input -= bottomLeft;
		float centerToSide = Mathf.Sqrt(Mathf.Pow(nodeSize, 2) - Mathf.Pow(nodeSize / 2, 2));

		int y = (int)((_input.z - nodeSize) / (nodeSize * 1.98f));
		int x = (int)(_input.x / (centerToSide * 2.28f));
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
    public float F { get { return G + H + Cost; } }

    public Node(Vector3 _worldPos, Vector2Int _gridPos, bool _walkable){
        worldPos = _worldPos;
        gridPos = _gridPos;
        walkable = _walkable;
    }

    public void DrawGizmos(float radius) {
        Gizmos.color = walkable ? Color.green : Color.red;
		radius *= 0.95f;
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

		for(int count = 0; count < 6; count++) {
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
