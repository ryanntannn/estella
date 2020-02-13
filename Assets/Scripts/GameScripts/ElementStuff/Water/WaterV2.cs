using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaterV2 : BaseElementV2 {
    public override Hand HandUsing { get; set; }

    [SerializeField]
    private AnimationClip m_regularAttackAnimation;
    public override AnimationClip RegularAttackAnimation { get { return m_regularAttackAnimation; } }

    public override float RegularManaCost => 5;

    [SerializeField]
    private AnimationClip m_ultimateAttackAnimation;
    public override AnimationClip UltimateAttackAnimation { get { return m_ultimateAttackAnimation; } }

    public override float UltimateManaCost => 20;

    public override int ID => Water;

    public override void CastRegularAttack() {
        //fireball
        GameObject instance = Instantiate(Resources.Load<GameObject>("Elements/Water/BubbleShot"), HandUsing.handPos.position, transform.rotation);
        if (Targeter.Instance.Target) {
            instance.transform.LookAt(Targeter.Instance.CollisionPoint);
        }
        ElementControlV2.Instance.currentMana -= RegularManaCost;
    }

    public override void CastUltimateAttack() {
        //tsunami
        GameObject tsunami = Resources.Load<GameObject>("Elements/Water/Tsunami");
        tsunami = Instantiate(tsunami, ElementControlV2.Instance.transform.position - ElementControlV2.Instance.transform.forward - ElementControlV2.Instance.transform.up, ElementControlV2.Instance.transform.rotation);
        ElementControlV2.Instance.currentMana -= UltimateManaCost;
    }
}