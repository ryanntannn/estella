using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

//mirage boss fight
public class Mirage : MonoBehaviour {
    public float maxIdleTime = 5;

	Enemy dataProvider;

    public GameObject mirageShadow;
    FiniteStateMachine fsm = new FiniteStateMachine();
    //states
    FiniteStateMachine.State Idle, RegularAttack, JumpAttack, KnifeThrow, TeleToPlayer;
	PlayerControl player;

    enum Skills { JumpBack, KnifeThrow, TeleToPlayer }
    bool[] skillCd = new bool[3];
    float timeStartIdle;
    float idleTime;

    //astar
    List<Node> path = new List<Node>();
    Node prevNode = null;
    Thread pathFinder;
    Vector3 playerPos = Vector3.zero, miragePos = Vector3.zero;
    // Start is called before the first frame update
    void Start() {
		dataProvider = GetComponent<Enemy>();
		dataProvider.ReferenceMap(Helper.FindComponentInScene<MapGrid>("Map"));  //set the reference to the map

		player = dataProvider.player;
        miragePos = transform.position;
        playerPos = player.transform.position;
        pathFinder = new Thread(FindPath);
        pathFinder.Start();
        pathFinder.IsBackground = true;

        InitStates();
    }

    void InitStates() {
        Idle = (gameObject) => {
            if (path.Count > 0) {
                //only idle for a set amount of time
                if (Time.time - timeStartIdle >= idleTime) {
                    //decide which state to go to next
                    Vector3 directionOfPlayer = player.transform.position - transform.position;
                    if (directionOfPlayer.magnitude > 15 && !skillCd[(int)Skills.JumpBack]) {
                        fsm.currentState = JumpAttack;
                    } else if (directionOfPlayer.magnitude > 10 && !skillCd[(int)Skills.KnifeThrow]) {
                        fsm.currentState = KnifeThrow;
                    } else {
                        fsm.currentState = RegularAttack;
                    }
                }
            }

            //watch out for incomming projectiles and try to dodge
        };

        RegularAttack = (gameObject) => {
            //walk up to player and slap him
            Vector3 directionOfPlayer = player.transform.position - transform.position;

            if (directionOfPlayer.sqrMagnitude < 2) {
                transform.LookAt(player.transform);
				dataProvider.anim.SetBool("isWalking", false);
                //set trigger
                if (!dataProvider.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
					dataProvider.anim.SetTrigger("whenAttack");
                }
                GoToIdle(dataProvider.anim.GetCurrentAnimatorStateInfo(0).length);
            } else {
                Vector3 toLookAt = path[1].worldPos;
                toLookAt.y = transform.position.y;
                transform.LookAt(toLookAt);
				dataProvider.anim.SetBool("isWalking", true);
                transform.position += transform.forward * dataProvider.currentSpeed * Time.deltaTime;
            }
        };

        JumpAttack = (gameObject) => {
            //set trigger
            if (!dataProvider.anim.GetCurrentAnimatorStateInfo(0).IsName("JumpBack")) {
				dataProvider.anim.SetTrigger("whenJump");
            }

            //set it on cd
            skillCd[(int)Skills.JumpBack] = true;
            StartCoroutine(SetCd(Skills.JumpBack, 20));  //20 second cd

            GoToIdle(dataProvider.anim.GetCurrentAnimatorStateInfo(0).length);
        };

        KnifeThrow = (gameObject) => {
            //look at player
            transform.LookAt(player.transform);

            //set trigger
            if (!dataProvider.anim.GetCurrentAnimatorStateInfo(0).IsName("KnifeThrow")) {
				dataProvider.anim.SetTrigger("whenThrowKnife");
            }

            //set it on cd
            skillCd[(int)Skills.KnifeThrow] = true;
            StartCoroutine(SetCd(Skills.KnifeThrow, 10));  //10 second cd

            GoToIdle(dataProvider.anim.GetCurrentAnimatorStateInfo(0).length);
        };


        TeleToPlayer = (gameObject) => {
            //teleport to player

        };

        //inital state to idle
        GoToIdle(0);
    }

    private void OnDrawGizmos() {
        if (path != null) {
            for (int count = 0; count < path.Count - 1; count++) {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(path[count].worldPos, path[count + 1].worldPos);
            }
        }
    }

    void GoToIdle(float clipLength) {
        //set start idle time
        timeStartIdle = Time.time;
        //set total idle time
        idleTime = UnityEngine.Random.Range(1.0f, maxIdleTime) + clipLength;
        fsm.currentState = Idle;
    }

    IEnumerator SetCd(Skills skill, float cd) {
        yield return new WaitForSeconds(cd);
        skillCd[(int)skill] = false;
    }

    void FindPath() {
        try {
            path = Algorithms.AStar(dataProvider.map, miragePos, playerPos);
        } catch (Exception) { };
    }

    //Update is called once per frame
    void Update() {
        miragePos = transform.position;
        playerPos = player.transform.position;
        if (!pathFinder.IsAlive) {
            pathFinder = new Thread(FindPath);
            pathFinder.Start();
        }

        fsm.currentState(gameObject);
    }

    private void OnApplicationQuit() {
        if(pathFinder.IsAlive) pathFinder.Abort();
    }

    private void OnDestroy() {
        //kill thread
        if (pathFinder.IsAlive) pathFinder.Abort();
    }

    public void JumpBack() {
		//add force to the foot of mirage
		//ey look man, the numbers work, so lets not change it
		//rb.AddForce(transform.up * 4, ForceMode.Impulse);
		dataProvider.rb.AddForce(-transform.forward * 10, ForceMode.Impulse);
        GameObject instance = Instantiate(mirageShadow, transform.position, Quaternion.identity);
        instance.transform.LookAt(player.transform.position, transform.up);
    }

    public void KnifeAttack() {
        RaycastHit hitInfo;
        //spherecast forward
        if(Physics.SphereCast(transform.position, 1, transform.forward, out hitInfo, 1 << Layers.Player)){
            hitInfo.collider.GetComponent<PlayerControl>().TakeDamage(5);
        }
    }


}
