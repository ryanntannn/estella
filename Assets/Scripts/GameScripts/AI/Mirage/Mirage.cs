using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//mirage boss fight
public class Mirage : Enemy {
    public Animator anim;
    public float maxIdleTime = 5;

    public GameObject mirageShadow;
    FiniteStateMachine fsm = new FiniteStateMachine();
    //states
    FiniteStateMachine.State Idle, RegularAttack, JumpAttack, KnifeThrow, TeleToPlayer;
    GameObject player;

    enum Skills { JumpBack, KnifeThrow, TeleToPlayer }
    bool[] skillCd = new bool[3];
    float timeStartIdle;
    float idleTime;

    //astar
    MapGrid map;
    List<Node> path = new List<Node>();
    int currentNode = 0;
    float pathTimer = 5;    //reset every 5 seconds
    Node prevNode = null;
    // Start is called before the first frame update
    public override void Start() {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        if (!anim) anim = transform.GetComponentInChildren<Animator>(); //if nothing set
        map = Helper.FindComponentInScene<MapGrid>("Map");
        InitStates();
    }

    void InitStates() {
        Idle = (gameObject) => {
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

            //watch out for incomming projectiles and try to dodge

        };

        RegularAttack = (gameObject) => {
            //walk up to player and slap him
            Vector3 directionOfPlayer = player.transform.position - transform.position;

            if (directionOfPlayer.magnitude < 3) {
                transform.LookAt(player.transform);
                anim.SetBool("isWalking", false);
                //set trigger
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                    anim.SetTrigger("whenAttack");
                }
                GoToIdle(anim.GetCurrentAnimatorStateInfo(0).length);
            } else {
                pathTimer += Time.deltaTime;
                if (pathTimer >= 5) {
                    QueryPath();
                }
                Vector3 toLookAt = path[currentNode].worldPos;
                toLookAt.y = transform.position.y;
                transform.LookAt(toLookAt);
                anim.SetBool("isWalking", true);
                transform.position += transform.forward * speed * Time.deltaTime;

                if ((transform.position - toLookAt).magnitude <= 0.2f) {
                    currentNode++;
                }
            }
        };

        JumpAttack = (gameObject) => {
            //set trigger
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("JumpBack")) {
                anim.SetTrigger("whenJump");
            }

            //set it on cd
            skillCd[(int)Skills.JumpBack] = true;
            StartCoroutine(SetCd(Skills.JumpBack, 20));  //20 second cd

            GoToIdle(anim.GetCurrentAnimatorStateInfo(0).length);
        };

        KnifeThrow = (gameObject) => {
            //look at player
            transform.LookAt(player.transform);

            //set trigger
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("KnifeThrow")) {
                anim.SetTrigger("whenThrowKnife");
            }

            //set it on cd
            skillCd[(int)Skills.KnifeThrow] = true;
            StartCoroutine(SetCd(Skills.KnifeThrow, 10));  //10 second cd

            GoToIdle(anim.GetCurrentAnimatorStateInfo(0).length);
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

    void QueryPath() {
        path = Algorithms.AStar(map, transform.position, player.transform.position);
        if (prevNode != path[0]) {
            currentNode = 0;
        }
        pathTimer = 0;  //reset timer
    }

    //Update is called once per frame
    public override void Update() {
        base.Update();

        fsm.currentState(gameObject);
    }

    public void JumpBack() {
        //add force to the foot of mirage
        //ey look man, the numbers work, so lets not change it
        //rb.AddForce(transform.up * 4, ForceMode.Impulse);
        rb.AddForce(-transform.forward * 10, ForceMode.Impulse);
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
