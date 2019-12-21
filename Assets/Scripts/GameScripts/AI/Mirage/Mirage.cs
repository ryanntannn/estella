using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//mirage boss fight
public class Mirage : Enemy {
    public float lengthOfDash = 6;
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
    // Start is called before the first frame update
    public override void Start() {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        if (!anim) anim = transform.GetComponentInChildren<Animator>(); //if nothing set
        InitStates();
    }

    void InitStates() {
        Idle = (gameObject) => {
            //only idle for a set amount of time
            if(Time.time - timeStartIdle >= idleTime) {
                //decide which state to go to next
                //temp
                fsm.currentState = RegularAttack;
            }
            //idk just chill lol
        };

        RegularAttack = (gameObject) => {
            //walk up to player and slap him
            Vector3 directionOfPlayer = player.transform.position - transform.position;

            if (directionOfPlayer.magnitude < 3) {
                anim.SetBool("isWalking", false);
                //set trigger
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                    anim.SetTrigger("whenAttack");
                }
                GoToIdle(anim.GetCurrentAnimatorStateInfo(0).length);
            }else {
                transform.LookAt(player.transform);
                //walk towards player
                anim.SetBool("isWalking", true);
                transform.position += transform.forward * speed * Time.deltaTime;
            }
        };

        JumpAttack = (gameObject) => {
            //set trigger
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("JumpBack")) {
                anim.SetTrigger("whenJump");
            }

            //set it on cd
            skillCd[(int)Skills.JumpBack] = true;
            StartCoroutine(SetCd(Skills.JumpBack, 5));  //5 second cd
        };

        KnifeThrow = (gameObject) => {

            //set trigger
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("KnifeThrow")) {
                anim.SetTrigger("whenThrowKnife");
            }

            //set it on cd
            skillCd[(int)Skills.KnifeThrow] = true;
            StartCoroutine(SetCd(Skills.KnifeThrow, 3));  //5 second cd
        };


        TeleToPlayer = (gameObject) => {
            //teleport to player

        };

        //inital state to idle
        GoToIdle(0);
    }

    void GoToIdle(float clipLength) {
        //set start idle time
        timeStartIdle = Time.time;
        //set total idle time
        idleTime = UnityEngine.Random.Range(3.0f, maxIdleTime) + clipLength;
        fsm.currentState = Idle;
    }

    IEnumerator SetCd(Skills skill, float cd) {
        yield return new WaitForSeconds(cd);
        skillCd[(int)skill] = false;
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
}
