using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class SkylarkBoss : MonoBehaviour {
    FiniteStateMachineWithStack fsm = new FiniteStateMachineWithStack();
    FiniteStateMachineWithStack.State Idle, Chase, DoTigerFist, DoSparkWraiths, DoChaosBeam, DoChargingBlast, DoPlasmaThrow, NullState;
    //dataprovider
    Enemy m_dataProvider;
    Animator m_anim;
    public Animator Anim { get { return m_anim; } }

    //astar
    MapGrid m_map;
    Thread m_pathFinder;
    Vector3 m_bossPos, m_playerPos; //threading
    List<Node> pathToPlayer = new List<Node>();

    // Start is called before the first frame update
    void Start() {
        m_map = Helper.FindComponentInScene<MapGrid>("Map");
        m_dataProvider = GetComponent<Enemy>();
        m_anim = m_dataProvider.anim;
        m_bossPos = transform.position;
        m_playerPos = m_dataProvider.player.transform.position;
        m_pathFinder = new Thread(FindPath);
        m_pathFinder.Start();
    }

    void InitStates() {
        Idle = (gameObject) => {

        };

        Chase = (gameObject) => {

        };

        #region Skills
        DoTigerFist = (gameObject) => {

        };

        DoChaosBeam = (gameObject) => {

        };
        DoSparkWraiths = (gameObject) => {

        };

        DoChargingBlast = (gameObject) => {

        };

        DoPlasmaThrow = (gameObject) => {

        };
        #endregion
    }

    // Update is called once per frame
    void Update() {
        //threading
        m_bossPos = transform.position;
        m_playerPos = m_dataProvider.player.transform.position;
    }

    #region Threading
    void FindPath() {
        while (true) {  //monkaS loop
            pathToPlayer = Algorithms.AStar(m_map, m_bossPos, m_playerPos);
        }
    }

    private void OnApplicationQuit() {
        if (m_pathFinder.IsAlive) m_pathFinder.Abort();
    }

    private void OnDestroy() {
        if (m_pathFinder.IsAlive) m_pathFinder.Abort();
    }
    #endregion

    /*
     * 
     * 
     *SKYLARK SKILLS 
     * 
     * 
     */
    private abstract class SkylarkSkill {
        //Seconds
        public abstract float CoolDown { get; }
        public bool OnCd { get; private set; }
        public abstract void Act(SkylarkBoss _agent);

        protected IEnumerator GoOnCd() {
            OnCd = true;
            yield return new WaitForSeconds(CoolDown);
            OnCd = false;
        }
    }

    private class TigerFist : SkylarkSkill {
        public override float CoolDown => 3;

        public override void Act(SkylarkBoss _agent) {
            _agent.StartCoroutine(GoOnCd());
        }
    }
}


