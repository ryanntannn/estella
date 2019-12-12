using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudGolem : MonoBehaviour, ISteamable {
    public float timeToRise = 2;
    public float yValue;
    public bool isSteamed = false;

    MapGrid map;
    //states of a mud golem
    FiniteStateMachine.State FindTarget, Walk, Attack;
    FiniteStateMachine fsm = new FiniteStateMachine();
    GameObject target;
    public float range = 3;
    // Start is called before the first frame update
    void Start() {
        map = Helper.FindComponentInScene<MapGrid>("Map");
        range = Mathf.Pow(range, 2);
        InitStates();
    }

    void InitStates() {
        FindTarget = (gameObject) => {
            //Dijkstras to find an enemy
            Queue<Node> openList = new Queue<Node>();
            List<Node> closedList = new List<Node>();
            Node startNode = map.WorldPointToNode(transform.position);
            openList.Enqueue(startNode);
            int justInCase = 0;
            while(openList.Count > 0 && ++justInCase <= 1000) {
                Node currentNode = openList.Dequeue();
                //check if node has an enemy on it
                Collider[] hitInfo = Physics.OverlapBox(currentNode.worldPos, Vector3.one * (map.nodeSize / 2), Quaternion.identity, 1 << Layers.Enemy);
                if (hitInfo.Length > 0) {
                    target = hitInfo[0].gameObject;
                    break;
                }

                foreach (Node n in map.GetNeighbours(currentNode)) {
                    if (!closedList.Contains(n)) {
                        n.parentNode = currentNode;
                        openList.Enqueue(n);
                    }
                }
            }

            if (target) {
                fsm.currentState = Walk;
            }
        };

        Walk = (gameObject) => {
            //go towards target
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime);

            if((transform.position - target.transform.position).sqrMagnitude > range) {
                fsm.currentState = Attack;
            }
        };

        Attack = (gameObject) => {
            
        };

        fsm.currentState = FindTarget;
    }

    // Update is called once per frame
    void Update() {
        if (transform.position.y != yValue) {
            Vector3 newPos = transform.position + transform.up * Time.deltaTime;
            newPos.y = Mathf.Clamp(newPos.y, Mathf.NegativeInfinity, yValue);
            transform.position = newPos;
        }//rising up
        else {
            //fsm.currentState(gameObject);
            //if (!target) {
            //    print("XD");
            //}

            Node currentNode = map.WorldPointToNode(transform.position);
            Collider[] hitInfo = Physics.OverlapBox(currentNode.worldPos, Vector3.one * (map.nodeSize / 2), Quaternion.identity, 1 << Layers.Enemy);
        }
    }

    public void SetSteamy(bool state) {
        isSteamed = state;
    }
}
