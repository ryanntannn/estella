using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArcherScript : MonoBehaviour {
    public float scareDist = 10;
    float scareSqr;
    public float shootRange = 30;
    float shootSqr;
    public bool showDebug = false;

    GameObject arrowPfb;
    GameObject currentArrow = null;
    public Transform firingPoint;
    public Transform instantiatePoint;
    //pathfinding
    NavMeshAgent navAgent;
    //data provider
    Enemy dataProvider;
    //fsm
    FiniteStateMachine fsm = new FiniteStateMachine();
    FiniteStateMachine.State Idle, Shoot, Run, nullState;
    // Start is called before the first frame update
    void Start() {
        arrowPfb = Resources.Load<GameObject>("Enemies/Arrow");

        dataProvider = GetComponent<Enemy>();
        navAgent = GetComponent<NavMeshAgent>();
        shootSqr = Mathf.Pow(shootRange, 2);
        scareSqr = Mathf.Pow(scareSqr, 2);

        InitStates();
    }

    void InitStates() {
        Idle = (gameObject) => {
            CheckForPlayer();
        };

        Shoot = (gameObject) => {
			Vector3 direction = (firingPoint.position - dataProvider.transform.position).normalized;

            dataProvider.anim.SetTrigger("WhenShoot");
            fsm.currentState = nullState;
        };

        Run = (gameObject) => {
            //run away from player

        };

        nullState = (gameObject) => { };

        fsm.currentState = Idle;
    }

    void CheckForPlayer() {
        Vector3 enemyToPlayer = dataProvider.player.transform.position - transform.position;
        if (enemyToPlayer.sqrMagnitude < shootSqr) {   //check if in shooting range
            if (!Physics.Raycast(transform.position, enemyToPlayer, Mathf.Infinity, 1 << ~Layers.Player)) {
                //check if in scary range
                fsm.currentState = enemyToPlayer.sqrMagnitude < scareSqr ? Run : Shoot;
            }
        }
    }

    public void GetArrow() {
        //instaniate arrow
        currentArrow = Instantiate(arrowPfb, instantiatePoint);
        currentArrow.GetComponent<ArrowScript>().firingPoint = firingPoint;
        currentArrow.GetComponent<ArrowScript>().dataProvider = dataProvider;
    }

    public void ShootArrow() {
        //shoot arrow
        //angle? idk
        //hotel? trivago
        currentArrow.transform.parent = null;
        currentArrow.GetComponent<Rigidbody>().AddForce(currentArrow.transform.forward, ForceMode.Impulse);
    }

    private void OnDrawGizmos() {
        if (showDebug) {
            Gizmos.color = new Color(1, 0, 0.2f);
            Gizmos.DrawSphere(transform.position, shootRange / 2);
            Gizmos.color = new Color(0, 1, 0, 0.4f);
            Gizmos.DrawSphere(transform.position, scareDist / 2);
        }
    }

    public void DoneShooting() {
        fsm.currentState = Idle;
    }

    // Update is called once per frame
    void Update() {
        fsm.currentState(gameObject);
        dataProvider.anim.SetBool("IsWalking", navAgent.velocity.sqrMagnitude > 0.5f);
    }
}
