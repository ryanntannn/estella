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
            if (map) {
                //Dijkstras to find an enemy
                //get positions
                IEnumerable<Vector3> positions = from enemy in map.enemies select enemy.transform.position;
                //set the path of the golem
                path = Algorithms.Dijkstras(map, transform.position, positions.ToArray());
                //set target
                fsm.currentState = path.Count > 0 ? Walk : Wander;
            } else {
                Collider[] hits = Physics.OverlapSphere(transform.position, 20, 1 << Layers.Enemy);
                if (hits.Length > 0) {
                    //find closests
                    Collider closests = hits[0];
                    float closestsSqrDist = (closests.transform.position - transform.position).sqrMagnitude;
                    for(int count = 1; count <= hits.Length - 1; count++) {
                        float sqrDist = (hits[count].transform.position - transform.position).sqrMagnitude;
                        if(sqrDist < closestsSqrDist) {
                            closests = hits[count];
                            closestsSqrDist = sqrDist;
                        }
                    }

                    //set target
                    target = closests.gameObject;
                } else {
                    fsm.currentState = Wander;
                }
            }
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
            //anim.SetTrigger("WhenAttack");
            if ((target.transform.position - transform.position).sqrMagnitude <= range) {
                target.GetComponent<Enemy>().TakeDamage(isSteamed ? 100 : 50);
            }

            Destroy(gameObject);
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
