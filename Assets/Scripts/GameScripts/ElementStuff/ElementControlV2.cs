using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElementControlV2 : Singleton<ElementControlV2> {
    [HideInInspector]
    public float currentMana;
    public float maxMana = 100;
    public float manaRegenRate = 1.0f;
    public bool isCasting = false;
    [Range(0.1f, 1)]
    public float delay = 0.3f;
    public KeyCode swapLeft = KeyCode.Q, swapRight = KeyCode.E; //swapping elements

    private Hand m_leftHand, m_rightHand;
    public Hand LeftHand { get { return m_leftHand; } }
    public Hand RightHand { get { return m_rightHand; } }
    private Animator m_anim;
    public Animator Anim { get { return m_anim; } }
    private AnimatorOverrideController m_animOverride;
    public AnimatorOverrideController AnimOverride { get { return m_animOverride; } }
    public AnimationClip DummyAnimationClip;

    private Coroutine m_lhandcr, m_rhandcr;

    private CombinationElements[] m_combinations;
    private PlayerControl m_playerControl;
    private float m_empowerTime = 0;
    public float EmpowerTime { get { return m_empowerTime; } }

    // Start is called before the first frame update
    void Start() {
        m_playerControl = GetComponent<PlayerControl>();

        currentMana = maxMana;
        m_anim = GetComponentInChildren<Animator>();
        IEnumerable<Hand> hands = GetComponents<Hand>();
        m_leftHand = hands.First(hand => hand.handTag.Equals("Left"));
        m_rightHand = hands.First(hand => hand.handTag.Equals("Right"));

        //by default
        ChangeElement(m_rightHand, GetComponent<WaterV2>());   //rightHand
        ChangeElement(m_leftHand, GetComponent<FireV2>());

        m_animOverride = new AnimatorOverrideController(m_anim.runtimeAnimatorController);
        m_anim.runtimeAnimatorController = m_animOverride;

        //load combinations
        m_combinations = Resources.LoadAll<CombinationElements>("Combinations");
    }

    // Update is called once per frame
    void Update() {
        RegenMana();
        CheckRightHand();
        CheckLeftHand();
        CheckForCombination();
        EmpowerCheck();

        //update animator
        m_anim.SetBool("IsCasting", isCasting);

    }

    void EmpowerCheck() {
        if (m_empowerTime > 0) {
            m_empowerTime = Mathf.Clamp(m_empowerTime - Time.deltaTime, 0, 999);

            if (m_playerControl.dodging) {

            }
        }
    }

    void CheckRightHand() {
        if (Input.GetKeyDown(m_rightHand.bind) && !isCasting) {
            m_rhandcr = StartCoroutine(WaitDelay(m_rightHand));
        }
    }

    void CheckLeftHand() {
        if (Input.GetKeyDown(m_leftHand.bind) && !isCasting) {
            m_lhandcr = StartCoroutine(WaitDelay(m_leftHand));
        }
    }

    void CheckForCombination() {
        if(m_rightHand.waitingOnOther && LeftHand.waitingOnOther) {
            StopCoroutine(m_rhandcr);
            StopCoroutine(m_lhandcr);
            m_rightHand.waitingOnOther = false;
            m_leftHand.waitingOnOther = false;
            isCasting = true;
            //combination
            CombinationElements combination = m_combinations.First(comb => comb.ID == (m_rightHand.currentElement.ID | m_leftHand.currentElement.ID));
            AnimOverride["NullAnimation"] = combination.animation;
        }
    }

    void RegenMana() {
        currentMana = Mathf.Clamp(currentMana + Time.deltaTime * manaRegenRate, 0, maxMana);
    }

    //cast after delay
    IEnumerator WaitDelay(Hand _hand) {
        _hand.waitingOnOther = true;

        yield return new WaitForSeconds(delay);

        _hand.waitingOnOther = false;
        isCasting = true;

        if (_hand.handTag.Equals("Right")) {
            if (Input.GetKey(KeyCode.LeftAlt)) {
                CastRightHandUlti();
            }else {
                CastRightHand();
            }
        }else {
            if (Input.GetKey(KeyCode.LeftAlt)) {
                CastLeftHandUlti();
            } else {
                CastLeftHand();
            }
        }
    }

    #region Casting
    public void CastRightHand() {
        Anim.SetBool("IsFlipped", false);
        Targeter.Instance.LookAtTarget();
        m_rightHand.currentElement.StartCastRegularAttack();
    }

    public void CastRightHandUlti() {
        Anim.SetBool("IsFlipped", false);
        Targeter.Instance.LookAtTarget();
        m_rightHand.currentElement.StartCastUltimateAttack();
    }

    public void CastLeftHand() {
        Anim.SetBool("IsFlipped", true);
        Targeter.Instance.LookAtTarget();
        m_leftHand.currentElement.StartCastRegularAttack();
    }

    public void CastLeftHandUlti() {
        Anim.SetBool("IsFlipped", true);
        Targeter.Instance.LookAtTarget();
        m_leftHand.currentElement.StartCastUltimateAttack();
    }
    #endregion

    #region Change elements
    private void ChangeElement(Hand _handToChange, BaseElementV2 _element) {
        if (!_element.HandUsing) {
            if(_handToChange.currentElement)
                _handToChange.currentElement.HandUsing = null;
            _handToChange.currentElement = _element;
            _element.HandUsing = _handToChange;
        }
    }

    public void ChangeRightHand(int _element) {
        switch (_element) {
            case 1:
                ChangeElement(m_rightHand, GetComponent<FireV2>());
                break;
            case 2:
                ChangeElement(m_rightHand, GetComponent<EarthV2>());
                break;
            case 3:
                ChangeElement(m_rightHand, GetComponent<WaterV2>());
                break;
            case 4:
                ChangeElement(m_rightHand, GetComponent<WindV2>());
                break;
            case 5:
                ChangeElement(m_rightHand, GetComponent<ElectricityV2>());
                break;
        }
    }

    public void ChangeLeftHandHand(int _element) {
        switch (_element) {
            case 1:
                ChangeElement(m_leftHand, GetComponent<FireV2>());
                break;
            case 2:
                ChangeElement(m_leftHand, GetComponent<EarthV2>());
                break;
            case 3:
                ChangeElement(m_leftHand, GetComponent<WaterV2>());
                break;
            case 4:
                ChangeElement(m_leftHand, GetComponent<WindV2>());
                break;
            case 5:
                ChangeElement(m_leftHand, GetComponent<ElectricityV2>());
                break;
        }
    }
    #endregion

    #region Combinations
    public void SummonGolem() {
        currentMana -= Elements.GolemCost;
        GameObject golem = Instantiate(Resources.Load<GameObject>("Elements/Mud/MudGolem"), TargetingReticle.Instance.transform.position, TargetingReticle.Instance.transform.rotation);
    }

    public void DoPlasma() {
        currentMana -= Elements.PlasmaCost;
        //average position of both hands
        Vector3 avg = (m_rightHand.handPos.position + m_leftHand.handPos.position) / 2;
        GameObject plasma = Instantiate(Resources.Load<GameObject>("Elements/Plasma/Plasma"), transform.position + transform.forward * 1.5f, transform.rotation);
    }

    public void DoSteam() {
        currentMana -= Elements.SteamCost;
        //GameObject steampit = Instantiate(Resources.Load<GameObject>("Elements/Steam/SteamPit"), TargetingReticle.Instance.transform.position, Quaternion.identity);
        m_empowerTime += 10;
    }

    public void DoIce() {
        currentMana -= Elements.IceCost;
        GameObject blizzard = Instantiate(Resources.Load<GameObject>("Elements/Ice/Blizzard"), transform.position, transform.rotation);
    }

    public void DoMagma() {
        currentMana -= Elements.MagmaCost;
        GameObject earthSplinter = Instantiate(Resources.Load<GameObject>("Elements/Magma/Magma"), TargetingReticle.Instance.transform.position, TargetingReticle.Instance.transform.rotation);
    }

    public void DoBlaze() {
        currentMana -= Elements.BlazeCost;
        GameObject fireTornado = Instantiate(Resources.Load<GameObject>("Elements/Blaze/FireTornado"), TargetingReticle.Instance.transform.position, TargetingReticle.Instance.transform.rotation);
        fireTornado = Instantiate(Resources.Load<GameObject>("Elements/Blaze/FireTornado"), TargetingReticle.Instance.transform.position + TargetingReticle.Instance.transform.right * 0.5f, TargetingReticle.Instance.transform.rotation * Quaternion.Euler(0, 45, 0));
        fireTornado = Instantiate(Resources.Load<GameObject>("Elements/Blaze/FireTornado"), TargetingReticle.Instance.transform.position - TargetingReticle.Instance.transform.right * 0.5f, TargetingReticle.Instance.transform.rotation * Quaternion.Euler(0, -45, 0));
    }

    public void DoDust() {
        currentMana -= Elements.DustCost;
        GameObject dustStorm = Instantiate(Resources.Load<GameObject>("Elements/Dust/DustStorm"), TargetingReticle.Instance.transform.position, TargetingReticle.Instance.transform.rotation);
    }

    public void DoMagnet() {
        currentMana -= Elements.MagnetiseCost;
        GameObject blackhole = Instantiate(Resources.Load<GameObject>("Elements/Magnetise/Blackhole"), TargetingReticle.Instance.transform.position, TargetingReticle.Instance.transform.rotation);
    }

    public void DoStorm() {
        currentMana -= Elements.StormCost;
        GameObject storm = Instantiate(Resources.Load<GameObject>("Elements/Storm/Storm"), TargetingReticle.Instance.transform.position, TargetingReticle.Instance.transform.rotation);
    }

    public void StartShock() {
        currentMana -= Elements.ShockCost;
        //find enemies in sphere
        Collider[] hits = Physics.OverlapSphere(TargetingReticle.Instance.transform.position, 3, 1 << Layers.Enemy);
        StartCoroutine(DoShock(100, hits));
    }

    public IEnumerator DoShock(float totalDamage, Collider[] enemies) {
        m_anim.speed = 0;

        //calculate damage to deal to each enemy
        float indvDmg = totalDamage / enemies.Length;

        //if no enemies just teleport
        if (enemies.Length <= 0) {
            transform.position = TargetingReticle.Instance.transform.position + TargetingReticle.Instance.transform.up;
        }

        //start moving
        for (int count = 0; count <= enemies.Length - 1; count++) {
            transform.position = enemies[count].transform.position - enemies[count].transform.forward;
            enemies[count].GetComponent<Enemy>().TakeDamage(indvDmg);
            yield return new WaitForSeconds(0.1f);
        }

        m_anim.speed = 1;
    }
    #endregion
}

public abstract class BaseElementV2 : MonoBehaviour {
    public const int
        Water = 1,
        Fire = 2,
        Earth = 4,
        Wind = 8,
        Electricity = 16;

    //hand currently using the element; can be null
    public abstract Hand HandUsing { get; set; }
    public abstract AnimationClip RegularAttackAnimation { get; }
    public abstract AnimationClip UltimateAttackAnimation { get; }
    public abstract float RegularManaCost { get; }
    public abstract float UltimateManaCost { get; }
    public abstract int ID { get; }

    public void StartCastRegularAttack() {
        if (ElementControlV2.Instance.currentMana >= RegularManaCost) {
            ElementControlV2.Instance.AnimOverride["NullAnimation"] = RegularAttackAnimation;
        }
    }
    public void StartCastUltimateAttack() {
        if (ElementControlV2.Instance.currentMana >= UltimateManaCost) {
            ElementControlV2.Instance.AnimOverride["NullAnimation"] = UltimateAttackAnimation;
        }
    }
    public void DoneCasting() {
        ElementControlV2.Instance.isCasting = false;
    }

    public abstract void CastRegularAttack();
    public abstract void CastUltimateAttack();
}
