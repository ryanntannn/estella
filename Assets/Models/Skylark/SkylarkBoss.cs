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
    float m_stoppingDist = Mathf.Pow(1.5f, 2);

    SkylarkSkill[] m_skillsAvaliable = new SkylarkSkill[5];
    GameObject m_beamPfb;

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

        m_beamPfb = Resources.Load<GameObject>("Skylark/SkylarkBeam");

        #region Initalising skills DONT LOOK IT'S BAD
        //As the wise Yandere dev once said
        //'The user will never see this'
        m_skillsAvaliable[0] = new TigerFist(this);
        m_skillsAvaliable[1] = new SparkWraith(this);
        m_skillsAvaliable[2] = new ChaosBeam(this);
        m_skillsAvaliable[3] = new ChargingBlast(this);
        m_skillsAvaliable[4] = new PlasmaThrow(this);
        #endregion
    }

    void InitStates() {
        Idle = (gameObject) => {
            //determine next state
            //pop idle
            fsm.PopState();
            m_anim.SetBool("IsWalking", false);

            foreach (SkylarkSkill s in m_skillsAvaliable) {
                if (s.IsAvaliable()) {
                    s.Act();
                    return;
                }
            }

            //no actions avaliable
            fsm.PushState(pathToPlayer.Count > 1 ? Chase : Idle);
        };

        Chase = (gameObject) => {
            foreach (SkylarkSkill s in m_skillsAvaliable) {
                if (s.IsAvaliable()) {
                    m_anim.SetBool("IsWalking", false);
                    s.Act();
                    return;
                }
            }

            m_anim.SetBool("IsWalking", true);
            if(pathToPlayer.Count > 1) {
                transform.LookAt(pathToPlayer[1].worldPos);
                Vector3 temp = transform.rotation.eulerAngles;
                temp.x = temp.z = 0;
                transform.rotation = Quaternion.Euler(temp);
                transform.position += transform.forward * Time.deltaTime * m_dataProvider.currentSpeed;
            }else {
                PushIdle();
            }

            if((m_playerPos - m_bossPos).sqrMagnitude <= m_stoppingDist) {
                PushIdle();
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

    #region State machine stuff
    public void PushIdle() {
        fsm.PopState();
        fsm.PushState(Idle);
    }

    void DoTigerFist() {

    }

    void DoPlasmaThrow() {

    }

    void DoChargingBlast() {
        m_dataProvider.rb.AddForce(transform.forward * 4 * m_dataProvider.currentSpeed, ForceMode.Impulse);
    }

    //generic do damage function
    void DoDamage() {
        m_dataProvider.DealDamage(20, 1);
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
        protected SkylarkBoss agent;
        public SkylarkSkill(SkylarkBoss _agent) {
            agent = _agent;
        }

        public abstract bool IsAvaliable();
        public void Act() {
            //go on cd
            agent.StartCoroutine(GoOnCd());
            agent.fsm.PushState(State);
        }

        protected IEnumerator GoOnCd() {
            OnCd = true;
            yield return new WaitForSeconds(CoolDown);
            OnCd = false;
        }
    }

    //tiger fist
    private class TigerFist : SkylarkSkill {
        public override float CoolDown => 10;
        private float m_minRage = Mathf.Pow(1.5f, 2);   //sqred

        public TigerFist(SkylarkBoss _agent) : base(_agent) {
        }

        public override FiniteStateMachineWithStack.State State => (gameObject) => {
            //punch infront 3 times and do damage
            agent.Anim.SetTrigger("WhenTigerFist");
            agent.transform.LookAt(agent.m_playerPos);
            agent.fsm.PopState();
            agent.fsm.PushState(agent.NullState);
        };

        public override bool IsAvaliable() {
            return
                !OnCd   //not on cd
                && ((agent.m_playerPos - agent.m_bossPos).sqrMagnitude <= m_minRage);    //and close to player
        }
    }

    //spark wraith
    private class SparkWraith : SkylarkSkill {
        public SparkWraith(SkylarkBoss _agent) : base(_agent) {
        }

        public override float CoolDown => 30;
        public override FiniteStateMachineWithStack.State State => (gameObject) => {
            Instantiate(Resources.Load<GameObject>("Skylark/Baller"), agent.transform);
            agent.PushIdle();//go idle
        };

        public override bool IsAvaliable() {
            return
                !OnCd;
        }
    }

    //chaos beam
    private class ChaosBeam : SkylarkSkill {
        public ChaosBeam(SkylarkBoss _agent) : base(_agent) {
        }

        public override float CoolDown => 40;
        public override FiniteStateMachineWithStack.State State => (gameObject) => {
            //make 3 lines at random locations
            for(int count = 0; count < 3; count++) {
                Vector2 randomLoc2d = UnityEngine.Random.insideUnitCircle * 10;
                Vector3 randomLocation = new Vector3(randomLoc2d.x, agent.transform.position.y, randomLoc2d.y);
                GameObject instance = Instantiate(agent.m_beamPfb, randomLocation, Quaternion.identity);
                instance.GetComponent<SkylarkBeam>().playerPos = agent.m_playerPos;
            }

            agent.PushIdle();
        };

        public override bool IsAvaliable() {
            return !OnCd;
        }
    }

    //chargin blast
    private class ChargingBlast : SkylarkSkill {
        public override float CoolDown => 15;
        private float m_minRage = Mathf.Pow(1.5f, 2);   //sqred

        public ChargingBlast(SkylarkBoss _agent) : base(_agent) {
        }

        public override FiniteStateMachineWithStack.State State => (gameObject) => {
            agent.Anim.SetTrigger("WhenChargingBlast");
            agent.transform.LookAt(agent.m_playerPos);
            agent.fsm.PopState();
            agent.fsm.PushState(agent.NullState);
        };

        public override bool IsAvaliable() {
            return
                !OnCd   //not on cd
                && ((agent.m_playerPos - agent.m_bossPos).sqrMagnitude >= m_minRage);    //and far from player
        }
    }

    //plasma throw
    private class PlasmaThrow : SkylarkSkill {
        public override float CoolDown => 15;
        private float m_minRage = Mathf.Pow(3, 2);   //sqred

        public PlasmaThrow(SkylarkBoss _agent) : base(_agent) {
        }

        public override FiniteStateMachineWithStack.State State => (gameObject) => {
            agent.Anim.SetTrigger("WhenPlasmaThrow");
            agent.fsm.PopState();
            agent.fsm.PushState(agent.NullState);
        };

        public override bool IsAvaliable() {
            //bool everythingElseOnCd =
            //   (from SkylarkSkill s in agent.m_skillsAvaliable
            //    where
            //    (s.GetType() != typeof(PlasmaThrow)
            //    && (s.IsAvaliable()))
            //    select s).Count() > 0;
            foreach(SkylarkSkill s in agent.m_skillsAvaliable) {
                if(s.GetType() != typeof(PlasmaThrow)) {
                    if (s.IsAvaliable()) return false;
                }
            }

            return
                !OnCd   //not on cd
                && ((agent.m_playerPos - agent.m_bossPos).sqrMagnitude >= m_minRage);    //and far from player
        }
    }
}


