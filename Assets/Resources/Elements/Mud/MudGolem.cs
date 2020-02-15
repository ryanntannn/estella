using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class MudGolem : MonoBehaviour, ISteamable {
    public bool isSteamed = false;

    MapGrid map;
    //states of a mud golem
    FiniteStateMachine.State FindTarget, Wander, Walk, Attack, NullState, Idle;
    FiniteStateMachine fsm = new FiniteStateMachine();
    public GameObject target;
    public float range = 3;
    List<Node> path = new List<Node>();
    int onNode = 0;
    NavMeshAgent m_navAgent;
    public GameObject hitEffect;
    Vector3 targetPos;

    //animator
    Animator anim;

    public bool useNavmesh = true;
    // Start is called before the first frame update
    void Start() {
        anim = GetComponentInChildren<Animator>();

        map = Helper.FindComponentInScene<MapGrid>("Map");
        m_navAgent = GetComponent<NavMeshAgent>();
        range = Mathf.Pow(range, 2);
        InitStates();
    }

    void InitStates() {
        Idle = (gameObject) => {
            if (useNavmesh) {
                CheckForTarget();
            }
        };

        FindTarget = (gameObject) => {
            if (map && !useNavmesh) {
                //Dijkstras to find an enemy
                //get positions
                IEnumerable<Vector3> positions = from enemy in map.enemies select enemy.transform.position;
                //set the path of the golem
                path = Algorithms.Dijkstras(map, transform.position, positions.ToArray());
                //set target
                fsm.currentState = path.Count > 0 ? Walk : Wander;
            } else {
                CheckForTarget();
                if (!target) {
                    Vector3 randomLoc = UnityEngine.Random.insideUnitSphere * range;
                    NavMeshHit hitInfo;
                    NavMesh.SamplePosition(randomLoc, out hitInfo, Mathf.Infinity, 1);
                    targetPos = hitInfo.position;
                    fsm.currentState = Wander;
                }else {
                    fsm.currentState = Walk;
                }
            }
        };

        Walk = (gameObject) => {
            //follow path
            if (target) {
                if (!useNavmesh) {
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime);
                } else {
                    m_navAgent.SetDestination(target.transform.position);
                }

                if ((target.transform.position - transform.position).sqrMagnitude < 10.0f) {
                    fsm.currentState = Attack;
                }
            } else {
                //target died before golem can reach
                fsm.currentState = FindTarget;
            }
        };

        Wander = (gameObject) => {
            //walk around
            if (useNavmesh) {
                CheckForTarget();
                if (target) {
                    fsm.currentState = Walk;
                    return;
                }
                if ((transform.position - targetPos).sqrMagnitude <= 2.0f) {
                    fsm.currentState = Idle;
                    StartCoroutine(GoToFindAfterDelay(UnityEngine.Random.Range(1.5f, 3.0f)));
                } else {
                    m_navAgent.SetDestination(targetPos);
                }
            }
        };

        Attack = (gameObject) => {
            if ((target.transform.position - transform.position).sqrMagnitude < 7.0f) {
                if (useNavmesh) m_navAgent.isStopped = true;
                anim.SetTrigger("WhenHit");
                StartCoroutine(HitEnemy());
                fsm.currentState = NullState;
            }
        };

        NullState = (gameObject) => { };

        fsm.currentState = NullState;
    }

    void CheckForTarget() {
        Collider[] hits = Physics.OverlapSphere(transform.position, range, 1 << Layers.Enemy);
        if (hits.Length > 0) {
            //find closests
            Collider closests = hits[0];
            float closestsSqrDist = (closests.transform.position - transform.position).sqrMagnitude;
            for (int count = 1; count <= hits.Length - 1; count++) {
                float sqrDist = (hits[count].transform.position - transform.position).sqrMagnitude;
                if (sqrDist < closestsSqrDist) {
                    closests = hits[count];
                    closestsSqrDist = sqrDist;
                }
            }

            target = closests.gameObject;
        }
    }

    IEnumerator HitEnemy() {
        yield return new WaitForSeconds(1.1f);
        target.GetComponent<Enemy>().TakeDamage(isSteamed ? 75 : 50);
        StartCoroutine(KillEffect(Instantiate(hitEffect, transform.position + transform.forward * 1.5f, Quaternion.identity)));

    }

    IEnumerator KillEffect(GameObject instance) {
        float timer = 0;
        while(timer <= 5) {
            timer += Time.deltaTime;
            instance.transform.position -= instance.transform.up * Time.deltaTime * 0.3f;
            yield return null;
        }
        Destroy(instance);
    }

    IEnumerator GoToFindAfterDelay(float _delay) {
        yield return new WaitForSeconds(_delay);
        fsm.currentState = FindTarget;
    }

    public void Ready() {
        fsm.currentState = FindTarget;
    }

    // Update is called once per frame
    void Update() {
        //navmesh fallback
        if (useNavmesh) {
            anim.SetBool("IsWalking", m_navAgent.velocity.sqrMagnitude >= 0.3f);
        }

        fsm.currentState(gameObject);
    }

    public void SetSteamy(bool state) {
        isSteamed = state;
    }
}
