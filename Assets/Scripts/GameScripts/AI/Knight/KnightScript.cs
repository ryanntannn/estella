using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using UnityEngine.AI;

public class KnightScript : MonoBehaviour {
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

    public void Start() {
		dataProvider = GetComponent<Enemy>();
		navAgent = GetComponent<NavMeshAgent>();
        stoppingSqrt = Mathf.Pow(stoppingDist, 2);
        aggroSqrt = Mathf.Pow(aggroRange, 2);
        startPos = transform.position;

        InitStates();
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
            navAgent.SetDestination(targetLocation);
            if((transform.position - targetLocation).sqrMagnitude <= 3) {
                GoIdle();
            }

            CheckForPlayer();
        };

		ChasePlayer = (gameObject) => {
            navAgent.SetDestination(dataProvider.player.transform.position);		

			//check for stopping distance
			if ((transform.position - dataProvider.player.transform.position).sqrMagnitude < stoppingSqrt) {
				navAgent.isStopped = true;
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
            navAgent.speed = dataProvider.currentSpeed;
            fsm.currentState(gameObject);
		}

        dataProvider.anim.SetBool("IsWalking", navAgent.velocity.sqrMagnitude > 0.3f);
    }

    public void GoIdle() {
        internalCounter = 0;
        currentIdleTime = Random.Range(2.0f, 5.0f);
        Vector3 randomLoc = (wanderNear ? startPos : transform.position) + Random.insideUnitSphere * walkDist;
        NavMeshHit hitInfo;
        NavMesh.SamplePosition(randomLoc, out hitInfo, walkDist, 1);
        targetLocation = hitInfo.position;

        navAgent.isStopped = false;
		fsm.currentState = IdleState;
    }
}
