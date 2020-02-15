using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skelington : MonoBehaviour {
    public bool debug = true;
    public float range = 10;

    //data provider
    Enemy m_dataProvider;
    Animator m_anim;
    NavMeshAgent m_navAgent;
    FiniteStateMachineWithStack fsm = new FiniteStateMachineWithStack();
    FiniteStateMachineWithStack.State Waiting, JumpingOut, Idle, Chase, Attack;
    float m_rangeSqrt;

    // Start is called before the first frame update
    void Start() {
        m_dataProvider = GetComponent<Enemy>();
        m_anim = m_dataProvider.anim;
        m_navAgent = GetComponent<NavMeshAgent>();
        m_rangeSqrt = Mathf.Pow(range, 2);
        m_navAgent.enabled = false;
        InitStates();
    }

    void InitStates() {
        Waiting = (gameObject) => {
            if ((m_dataProvider.player.transform.position - transform.position).sqrMagnitude <= m_rangeSqrt) {
                fsm.PopState();
                fsm.PushState(Idle);
                fsm.PushState(JumpingOut);
            }
        };

        JumpingOut = (gameObject) => {
            //raycast up
            RaycastHit hitInfo;
            if(Physics.Raycast(transform.position, transform.up, out hitInfo, Mathf.Infinity, 1 << Layers.Terrain)) {
                transform.position = hitInfo.point + transform.up;
            }
            m_navAgent.enabled = true;
            fsm.PopState();
        };

        Idle = (gameObject) => {
            if ((m_dataProvider.player.transform.position - transform.position).sqrMagnitude <= m_rangeSqrt) {
                fsm.PopState();
                fsm.PushState(Chase);
            }
        };

        Chase = (gameObject) => {
            m_navAgent.SetDestination(m_dataProvider.player.transform.position);
            if ((m_dataProvider.player.transform.position - transform.position).sqrMagnitude <= 3) {
                fsm.PushState(Attack);
            }
        };

        Attack = (gameObject) => {
            m_anim.SetTrigger("WhenAttack");
        };

        fsm.PushState(Waiting);
    }

    private void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawSphere(transform.position, range);
    }

    public void DoneAttacking() {
        fsm.PopState();
    }

    // Update is called once per frame
    void Update() {
        m_navAgent.speed = m_dataProvider.currentSpeed;
        m_anim.SetFloat("speedh", m_navAgent.velocity.x);
        m_anim.SetFloat("speedy", m_navAgent.velocity.y);

        fsm.Act(gameObject);
    }
}
