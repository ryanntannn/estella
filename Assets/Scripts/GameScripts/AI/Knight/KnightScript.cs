using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;

public class KnightScript : Enemy {
    public bool useFSM = true;

    GameObject player;
    //fsm stuff
    FiniteStateMachine fsm = new FiniteStateMachine();
    FiniteStateMachine.State IdleState, ChasePlayer;
    List<Node> path = new List<Node>();
    Thread pathFinder;
    Vector3 playerPos, enemyPos;
    [SerializeField] float stoppingDistance = 0.5f;
    float stoppingSqrt;

	public override void Start() {
		base.Start();

        player = GameObject.FindWithTag("Player");
        ResetThread();
        ReferenceMap(FindObjectOfType<MapGrid>());
        stoppingSqrt = Mathf.Pow(stoppingSqrt, 2);
        InitStates();
	}

    void ResetThread() {
        pathFinder = new Thread(SetPath);
        pathFinder.IsBackground = true;
        pathFinder.Start();
    }

    void InitStates() {
        //well, idle
        IdleState = (gameObject) => {
            if (player) fsm.currentState = ChasePlayer;
        };

        ChasePlayer = (gameObject) => {
            if(path.Count > 0) {
                //move to second path
                Vector3 toLookAt = path[1].worldPos;
                toLookAt.y = transform.position.y;
                transform.LookAt(toLookAt);
                transform.position += transform.forward * Time.deltaTime * currentSpeed;
            }

            //check for stopping distance
            if ((transform.position - player.transform.position).sqrMagnitude <= stoppingSqrt) {
                fsm.currentState = IdleState;
            }
        };

        //set default state to idle
        fsm.currentState = IdleState;
    }

    void SetPath() {
        path = Algorithms.AStar(map, enemyPos, playerPos);
    }

    private void OnApplicationQuit() {
        if (pathFinder.IsAlive) pathFinder.Abort();
    }

    private void OnDrawGizmos() {
        if (path.Count > 0) {
            for (int count = 0; count < path.Count - 1; count++) {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(path[count].worldPos, path[count + 1].worldPos);
            }
        }
    }

    public override void Update() {
		base.Update();
        if (useFSM) {
            fsm.currentState(gameObject);
            //reset path
            if (!pathFinder.IsAlive) {
                ResetThread();
            }
            playerPos = player.transform.position;
            enemyPos = transform.position;
        }
	}

	public override void DebuffEnemy(float duration, Effects effect) {
		base.DebuffEnemy(duration, effect);
	}
}
