using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Linq;

public class SkylarkBoss : MonoBehaviour {
    FiniteStateMachineWithStack fsm = new FiniteStateMachineWithStack();
    FiniteStateMachineWithStack.State Idle, Chase, NullState;
    //dataprovider
    Enemy m_dataProvider;
    Animator m_anim;
    public Animator Anim { get { return m_anim; } }

    //astar
    MapGrid m_map;
    Thread m_pathFinder;
    Vector3 m_bossPos, m_playerPos; //threading
    List<Node> pathToPlayer = new List<Node>();

    SkylarkSkill[] m_skillsAvaliable = new SkylarkSkill[5];

    // Start is called before the first frame update
    void Start() {
        m_map = Helper.FindComponentInScene<MapGrid>("Map");
        m_dataProvider = GetComponent<Enemy>();
        m_anim = m_dataProvider.anim;
        m_bossPos = transform.position;
        m_playerPos = m_dataProvider.player.transform.position;
        m_pathFinder = new Thread(FindPath);
        m_pathFinder.Start();

        InitStates();

        #region Initalising skills DONT LOOK IT'S BAD
        //As the wise Yandere dev once said
        //'The user will never see this'
        m_skillsAvaliable[0] = new TigerFist();
        m_skillsAvaliable[1] = new SparkWraith();
        m_skillsAvaliable[2] = new ChaosBeam();
        m_skillsAvaliable[3] = new ChargingBlast();
        m_skillsAvaliable[4] = new PlasmaThrow();
        #endregion
    }

    void InitStates() {
        Idle = (gameObject) => {
            //determine next state
            fsm.PopState();

            foreach (SkylarkSkill s in m_skillsAvaliable) {
                if (s.IsAvaliable(this)) {
                    s.Act(this);
                    return;
                }
            }

            //no actions avaliable
            fsm.PushState(pathToPlayer.Count > 1 ? Chase : Idle);
        };

        Chase = (gameObject) => {
            m_anim.SetBool("IsWalking", true);
            if(pathToPlayer.Count > 1) {
                transform.LookAt(pathToPlayer[1].worldPos);
                Vector3 temp = transform.rotation.eulerAngles;
                temp.x = temp.z = 0;
                transform.rotation = Quaternion.Euler(temp);
                transform.position += transform.forward * Time.deltaTime * m_dataProvider.currentSpeed;
            }else {
                fsm.PopState();
                fsm.PushState(Idle);
            }
        };

        NullState = (gameObject) => { };

        fsm.PushState(Idle);
    }

    // Update is called once per frame
    void Update() {
        //threading
        m_bossPos = transform.position;
        m_playerPos = m_dataProvider.player.transform.position;

        fsm.Act(gameObject);
    }

    //debug
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if (pathToPlayer != null) {
            for (int count = 0; count < pathToPlayer.Count - 1; count++) {
                Gizmos.DrawLine(pathToPlayer[count].worldPos, pathToPlayer[count + 1].worldPos);
            }
        }
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
        public abstract float CoolDown { get; } //in seconds
        public abstract FiniteStateMachineWithStack.State State { get; }
        public bool OnCd { get; private set; }

        public abstract bool IsAvaliable(SkylarkBoss _agent);
        public virtual void Act(SkylarkBoss _agent) {
            //go on cd
            _agent.StartCoroutine(GoOnCd());
            _agent.fsm.PushState(State);
        }

        protected IEnumerator GoOnCd() {
            OnCd = true;
            yield return new WaitForSeconds(CoolDown);
            OnCd = false;
        }
    }

    //tiger fist
    private class TigerFist : SkylarkSkill {
        public override float CoolDown => 3;
        private float m_minRage = Mathf.Pow(3, 2);   //sqred
        public override FiniteStateMachineWithStack.State State => (gameObject) => {

        };

        public override void Act(SkylarkBoss _agent) {
            base.Act(_agent);
            //punch infront 3 times and do damage
        }

        public override bool IsAvaliable(SkylarkBoss _agent) {
            return
                !OnCd   //not on cd
                && ((_agent.m_playerPos - _agent.m_bossPos).sqrMagnitude <= m_minRage);    //and close to player
        }
    }

    //spark wraith
    private class SparkWraith : SkylarkSkill {
        public override float CoolDown => 30;
        public override FiniteStateMachineWithStack.State State => (gameObject) => {

        };

        public override void Act(SkylarkBoss _agent) {
            base.Act(_agent);
        }

        public override bool IsAvaliable(SkylarkBoss _agent) {
            return
                !OnCd;
        }
    }

    //chaos beam
    private class ChaosBeam : SkylarkSkill {
        public override float CoolDown => 40;
        public override FiniteStateMachineWithStack.State State => (gameObject) => {

        };

        public override void Act(SkylarkBoss _agent) {
            base.Act(_agent);
        }

        public override bool IsAvaliable(SkylarkBoss _agent) {
            return !OnCd;
        }
    }

    //chargin blast
    private class ChargingBlast : SkylarkSkill {
        public override float CoolDown => 5;
        private float m_minRage = Mathf.Pow(3, 2);   //sqred
        public override FiniteStateMachineWithStack.State State => (gameObject) => {

        };

        public override void Act(SkylarkBoss _agent) {
            base.Act(_agent);
        }

        public override bool IsAvaliable(SkylarkBoss _agent) {
            return
                !OnCd   //not on cd
                && ((_agent.m_playerPos - _agent.m_bossPos).sqrMagnitude >= m_minRage);    //and far from player
        }
    }

    //plasma throw
    private class PlasmaThrow : SkylarkSkill {
        public override float CoolDown => 5;
        private float m_minRage = Mathf.Pow(3, 2);   //sqred
        public override FiniteStateMachineWithStack.State State => (gameObject) => {

        };

        public override void Act(SkylarkBoss _agent) {
            base.Act(_agent);
        }

        public override bool IsAvaliable(SkylarkBoss _agent) {
            bool everythingElseOnCd =
               (from SkylarkSkill s in _agent.m_skillsAvaliable
                where
                (s.GetType() != typeof(PlasmaThrow)
                && (s.IsAvaliable(_agent)))
                select s).Count() > 0;

            return
                !OnCd   //not on cd
                && ((_agent.m_playerPos - _agent.m_bossPos).sqrMagnitude >= m_minRage)    //and far from player
                && everythingElseOnCd;  //everything else is on cd
        }
    }
}


