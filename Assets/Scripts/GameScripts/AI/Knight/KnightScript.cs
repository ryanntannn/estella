using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using UnityEngine.AI;

public class KnightScript : MonoBehaviour {
    public bool useFSM = true;
	public float stoppingDist = 2;
	private float stoppingSqrt;

	//use navmesh
	NavMeshAgent navAgent;
	//data provider
	Enemy dataProvider;
	//fsm stuff
	FiniteStateMachine fsm = new FiniteStateMachine();
	FiniteStateMachine.State IdleState, ChasePlayer, AttackPlayer, nullState;

	public void Start() {
		dataProvider = GetComponent<Enemy>();
		navAgent = GetComponent<NavMeshAgent>();
		stoppingSqrt = Mathf.Sqrt(stoppingDist);
		InitStates();
	}

	void InitStates() {
		//well, idle
		IdleState = (gameObject) => {
			if (dataProvider.player) fsm.currentState = ChasePlayer;
		};

		ChasePlayer = (gameObject) => {
			navAgent.speed = dataProvider.currentSpeed;
			if (navAgent.SetDestination(dataProvider.player.transform.position)) {
				dataProvider.anim.SetBool("IsWalking", true);
				Vector3 toLookAt = navAgent.nextPosition;
				toLookAt.y = transform.position.y;
				transform.LookAt(toLookAt);
			}

			//check for stopping distance
			if ((transform.position - dataProvider.player.transform.position).sqrMagnitude <= stoppingSqrt) {
				navAgent.isStopped = true;
				dataProvider.anim.SetBool("IsWalking", false);
				fsm.currentState = AttackPlayer;
			}
		};

		AttackPlayer = (gameObject) => {
			dataProvider.anim.SetTrigger("WhenSpin");
			fsm.currentState = nullState;
		};

		//actually do nothing
		nullState = (gameObject) => { };

		//set default state to idle
		fsm.currentState = IdleState;
	}

	private void OnDrawGizmos() {

	}

	public void Update() {
		if (useFSM) {
			fsm.currentState(gameObject);
		}
	}

	public void GoIdle() {
		navAgent.isStopped = false;
		fsm.currentState = IdleState;
	}
}
