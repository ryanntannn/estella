using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using UnityEngine.AI;

public class KnightScript : MonoBehaviour {
    public bool useAStar = false;
    public bool useFSM = true;
    public bool wanderNear = false;
	public float stoppingDist = 2, aggroRange = 10;
	private float stoppingSqrt, aggroSqrt;

	//data provider
	Enemy dataProvider;
	//fsm stuff
	FiniteStateMachine fsm = new FiniteStateMachine();
	FiniteStateMachine.State IdleState, Wander, ChasePlayer, SpinAttack, RegularAttack, nullState;
    //ai
    NavMeshAgent navAgent;
    Vector3 targetLocation = Vector3.zero;
    float currentIdleTime;
    float internalCounter = 0;
    float walkDist = 20;
    bool spinOnCd = false;

    Vector3 startPos;
    Thread m_pathFinder;
    MapGrid m_map;
    List<Node> path = new List<Node>();
    Vector3 m_pos;

    public void Start() {
		dataProvider = GetComponent<Enemy>();
        stoppingSqrt = Mathf.Pow(stoppingDist, 2);
        aggroSqrt = Mathf.Pow(aggroRange, 2);
        startPos = transform.position;
        m_pos = transform.position;
        if (useAStar) {
            m_map = Helper.FindComponentInScene<MapGrid>("Map");
            m_pathFinder = new Thread(PathFinder);
            m_pathFinder.Start();
            m_pathFinder.IsBackground = true;
        } else {
            navAgent = GetComponent<NavMeshAgent>();
        }

        InitStates();
    }

    void PathFinder() {
        while (true) {
            path = Algorithms.AStar(m_map, m_pos, targetLocation);
        }
    }

	void InitStates() {
		//well, idle
		IdleState = (gameObject) => {
            internalCounter += Time.deltaTime;
            CheckForPlayer();

            if (internalCounter > currentIdleTime) {  //wait long enough
                internalCounter = 0;
                currentIdleTime = Random.Range(2.0f, 5.0f);
                fsm.currentState = Wander;
            }
		};

        Wander = (gameObject) => {
            //walk to radom pos
            if (!useAStar) {
                navAgent.SetDestination(targetLocation);
            }else {

            }
            if((transform.position - targetLocation).sqrMagnitude <= 3) {
                GoIdle();
            }

            CheckForPlayer();
        };

		ChasePlayer = (gameObject) => {
            if (!useAStar) {
                navAgent.SetDestination(dataProvider.player.transform.position);
            }else {
                targetLocation = dataProvider.player.transform.position;
                transform.LookAt(path[1].worldPos);
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            }

			//check for stopping distance
			if ((transform.position - dataProvider.player.transform.position).sqrMagnitude < stoppingSqrt) {
                if (!useAStar) {
                    navAgent.isStopped = true;
                }else {

                }
				fsm.currentState = spinOnCd ? RegularAttack : SpinAttack;
			}
		};

		SpinAttack = (gameObject) => {
            StartCoroutine(GoSpinCd());
			dataProvider.anim.SetTrigger("WhenSpin");
			fsm.currentState = nullState;
		};

        RegularAttack = (gameObject) => {
            dataProvider.anim.SetTrigger("WhenRegularAttack");
            fsm.currentState = nullState;
        };

		//actually do nothing
		nullState = (gameObject) => { };

        //set default state to idle
        GoIdle();
	}

    IEnumerator GoSpinCd() {
        spinOnCd = true;
        yield return new WaitForSeconds(20);
        spinOnCd = false;
    }

	private void OnDrawGizmos() {
        if (wanderNear) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(startPos, walkDist);
        }

        if (useAStar) {
            Gizmos.color = Color.red;
            if (path != null) {
                for (int count = 0; count < path.Count - 1; count++) {
                    Gizmos.DrawLine(path[count].worldPos, path[count + 1].worldPos);
                }
            }
        }
	}

    void CheckForPlayer() {
        Vector3 enemyToPlayer = dataProvider.player.transform.position - transform.position;
        if (enemyToPlayer.sqrMagnitude < aggroSqrt) {   //check if near enough
            //check for los        
            if (!Physics.Raycast(transform.position, enemyToPlayer, Mathf.Infinity, 1 << ~Layers.Player)) {
                fsm.currentState = ChasePlayer;
            }
        }
    }

	public void Update() {
		if (useFSM) {
            fsm.currentState(gameObject);

            if (!useAStar) {
                navAgent.speed = dataProvider.currentSpeed;
                dataProvider.anim.SetBool("IsWalking", navAgent.velocity.sqrMagnitude > 0.3f);
            } else {
                m_pos = transform.position;
                if(path != null) {
                    if (path.Count > 0) {
                        targetLocation = dataProvider.player.transform.position;
                        transform.LookAt(path[1].worldPos);
                        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                    }
                }
            }
        }
    }

    public void GoIdle() {
        internalCounter = 0;
        currentIdleTime = Random.Range(2.0f, 5.0f);
        Vector3 randomLoc = (wanderNear ? startPos : transform.position) + Random.insideUnitSphere * walkDist;

        if (!useAStar) {
            NavMeshHit hitInfo;
            NavMesh.SamplePosition(randomLoc, out hitInfo, walkDist, 1);
            targetLocation = hitInfo.position;

            navAgent.isStopped = false;
        }else {
            ////find on grid
            //Node myPos = m_map.WorldPointToNode(transform.position);
            //Vector2Int randomGridPos = myPos.gridPos + new Vector2Int((Random.Range(-10, 10)), (Random.Range(-10, 10)));
            //Node finalPos = m_map.Grid[randomGridPos.x, randomGridPos.y];
            //targetLocation = finalPos.worldPos;
            print(dataProvider.player.transform.position);
            targetLocation = m_map.WorldPointToNode(dataProvider.player.transform.position).worldPos;
        }
		fsm.currentState = IdleState;
    }
}
