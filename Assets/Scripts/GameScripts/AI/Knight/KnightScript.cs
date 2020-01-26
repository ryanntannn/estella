using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;

public class KnightScript : Enemy {
    public bool useFSM = true;

    //fsm stuff
    FiniteStateMachine fsm = new FiniteStateMachine();
    FiniteStateMachine.State IdleState, ChasePlayer, AttackPlayer, nullState;
    List<Node> path = new List<Node>();
    Thread pathFinder;
    Vector3 playerPos, enemyPos;
    [SerializeField] float stoppingDistance = 0.5f;
    float stoppingSqrt;

	public override void Start() {
		base.Start();

        playerPos = player.transform.position;
        enemyPos = transform.position;

        ResetThread();
        ReferenceMap(FindObjectOfType<MapGrid>());
        stoppingSqrt = Mathf.Pow(stoppingDistance, 2);
        InitStates();
	}

    void ResetThread() {
        pathFinder = new Thread(SetPath);
        pathFinder.Start();
        pathFinder.IsBackground = true;
    }

    void InitStates() {
        //well, idle
        IdleState = (gameObject) => {
            if (player) fsm.currentState = ChasePlayer;
        };

        ChasePlayer = (gameObject) => {
            if(path.Count > 1) {
                anim.SetBool("IsWalking", true);
                //move to second path
                Vector3 toLookAt = path[1].worldPos;
                toLookAt.y = transform.position.y;
                transform.LookAt(toLookAt);
                transform.position += transform.forward * Time.deltaTime * currentSpeed;
            }

            //check for stopping distance
            if ((transform.position - player.transform.position).sqrMagnitude <= stoppingSqrt) {
                anim.SetBool("IsWalking", false);
                fsm.currentState = AttackPlayer;
            }
        };

        AttackPlayer = (gameObject) => {
            anim.SetTrigger("WhenSpin");
            fsm.currentState = nullState;
        };

        //actually do nothing
        nullState = (gameObject) => { };

        //set default state to idle
        fsm.currentState = IdleState;
    }

    void SetPath() {
        path = Algorithms.AStar(map, enemyPos, playerPos);
    }

    private void OnApplicationQuit() {
        //kill thread
        if (pathFinder.IsAlive) pathFinder.Abort();
    }

    private void OnDestroy() {
        //kill thread
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

    public void GoIdle() {
        fsm.currentState = IdleState;
    }
}
