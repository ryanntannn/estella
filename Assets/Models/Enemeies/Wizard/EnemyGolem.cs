using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class EnemyGolem : MonoBehaviour {

    //dataprov
    Enemy m_dataProvider;
    Animator m_anim;
    NavMeshAgent m_navAgent;
    FiniteStateMachine fsm = new FiniteStateMachine();
    FiniteStateMachine.State Idle, Chase, Attack, NullState;
    
    // Start is called before the first frame update
    void Start() {
        m_dataProvider = GetComponent<Enemy>();
        m_anim = m_dataProvider.anim;
        m_navAgent = GetComponent<NavMeshAgent>();

        InitStates();
    }

    void InitStates() {
        Idle = (gameObject) => {
            m_navAgent.isStopped = true;

            if ((m_dataProvider.player.transform.position - transform.position).sqrMagnitude <= 50.0f) {
                fsm.currentState = Chase;
            }
        };

        Chase = (gameObject) => {
            m_navAgent.isStopped = false;
            m_navAgent.SetDestination(m_dataProvider.player.transform.position);

            if ((m_dataProvider.player.transform.position - transform.position).sqrMagnitude <= 5.0f) {
                fsm.currentState = Attack;
            }
        };

        Attack = (gameObject) => {
            m_navAgent.isStopped = true;

            m_anim.SetTrigger("WhenAttack");
            fsm.currentState = NullState;
        };

        NullState = (gameObject) => { };

        fsm.currentState = NullState;
    }

    void GoIdle() {
        fsm.currentState = Idle;
    }

    // Update is called once per frame
    void Update() {
        fsm.currentState(gameObject);
        m_anim.SetBool("IsWalking", m_navAgent.velocity.sqrMagnitude >= 0.5f);
    }
}
