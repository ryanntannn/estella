using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MudGolem : MonoBehaviour, ISteamable {
    public bool isSteamed = false;

    MapGrid map;
    //states of a mud golem
    FiniteStateMachine.State FindTarget, Wander, Walk, Attack;
    FiniteStateMachine fsm = new FiniteStateMachine();
    public GameObject target;
    public float range = 3;
    List<Node> path = new List<Node>();
    int onNode = 0;

    //animator
    Animator anim;
    // Start is called before the first frame update
    void Start() {
        //anim = GetComponent<Animator>();

        map = Helper.FindComponentInScene<MapGrid>("Map");
        range = Mathf.Pow(range, 2);
        InitStates();
    }

    void InitStates() {
        FindTarget = (gameObject) => {
            //Dijkstras to find an enemy
            //get positions
            IEnumerable<Vector3> positions = from enemy in map.enemies select enemy.transform.position;
            //set the path of the golem
            path = Algorithms.Dijkstras(map, transform.position, positions.ToArray());
            //set target

            fsm.currentState = path.Count > 0 ? Walk : Wander;
        };

        Walk = (gameObject) => {
            //follow path
            if (target) {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime);
                if ((target.transform.position - transform.position).sqrMagnitude <= range) {
                    fsm.currentState = Attack;
                }
            }else {
                //target died before golem can reach
                fsm.currentState = FindTarget;
            }
        };

        Wander = (gameObject) => {
            //walk around
        };

        Attack = (gameObject) => {

        };

        fsm.currentState = FindTarget;
    }

    // Update is called once per frame
    void Update() {

    }

    public void SetSteamy(bool state) {
        isSteamed = state;
    }
}
