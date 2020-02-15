using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class WizardScript : MonoBehaviour {
    public GameObject golemPfb;

    //dataprovider
    Enemy m_dataProvider;
    NavMeshAgent m_navAgent;
    Animator m_anim;

    //fsm
    FiniteStateMachineWithStack fsm = new FiniteStateMachineWithStack();
    FiniteStateMachineWithStack.State Idle, Walk, SummonGolem, ShootFireBall, NullState;
    GameObject golem;
    // Start is called before the first frame update
    void Start() {
        m_dataProvider = GetComponent<Enemy>();
        m_navAgent = GetComponent<NavMeshAgent>();
        m_anim = m_dataProvider.anim;

        InitStates();
    }

    void InitStates() {
        Idle = (gameObject) => {
            fsm.PopState();
            m_navAgent.isStopped = true;
            //check if player is in range
            float meToPlayer = (m_dataProvider.player.transform.position - transform.position).sqrMagnitude;
            //print(meToPlayer);
            if (meToPlayer <= 50) {
                //determine action
                //too late to shoot
                if(meToPlayer >= 10) {
                    //summon golem then run then fireball
                    fsm.PushState(Idle);
                    fsm.PushState(ShootFireBall);
                    fsm.PushState(Walk);
                    fsm.PushState(SummonGolem);
                }else {
                    fsm.PushState(Idle);
                    fsm.PushState(SummonGolem);
                    fsm.PushState(Walk);
                    fsm.PushState(ShootFireBall);
                }
                return;
            }

            fsm.PushState(Idle);
        };

        Walk = (gameObject) => {
            //walk away from player
            Vector3 vecAway = (transform.position - m_dataProvider.player.transform.position).normalized * 10;
            NavMeshHit hitInfo;

            if (!NavMesh.SamplePosition(transform.position + vecAway, out hitInfo, 10, 1)) {
                //path not found
                //its over
                //stand and fight
                fsm.PopState();
            } else {
                m_navAgent.SetDestination(hitInfo.position);
                m_navAgent.isStopped = false;
            }

            if ((transform.position - m_dataProvider.player.transform.position).sqrMagnitude >= 20) {
                //far enough I guess
                fsm.PopState();
            }
        };

        SummonGolem = (gameObject) => {
            if (golem) {
                fsm.PopState();
                return;
            }

            m_navAgent.isStopped = true;
            transform.LookAt(m_dataProvider.player.transform.position);
            m_anim.SetTrigger("WhenSummonGolem");
            fsm.PopState();
            fsm.PushState(NullState);
        };

        ShootFireBall = (gameObject) => {
            m_navAgent.isStopped = true;
            transform.LookAt(m_dataProvider.player.transform.position);
            m_anim.SetTrigger("WhenFireball");
            fsm.PopState();
            fsm.PushState(NullState);
        };

        NullState = (gameObject) => { };

        fsm.PushState(Idle);
    }

    // Update is called once per frame
    void Update() {
        m_anim.SetFloat("VeloX", m_navAgent.velocity.x);
        m_anim.SetFloat("VeloY", m_navAgent.velocity.z);

        fsm.Act(gameObject);
    }

    void DoFireball() {

    }

    void DoGolem() {
        golem = Instantiate(golemPfb, transform.position + transform.forward, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0));
    }

    void PopState() {
        fsm.PopState();
    }
}
