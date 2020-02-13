using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireV2 : BaseElementV2 {
    public override Hand HandUsing { get; set; }

    [SerializeField]
    private AnimationClip m_regularAttackAnimation;
    public override AnimationClip RegularAttackAnimation { get { return m_regularAttackAnimation; } }

    public override float RegularManaCost => 5;

    [SerializeField]
    private AnimationClip m_ultimateAttackAnimation;
    public override AnimationClip UltimateAttackAnimation { get { return m_ultimateAttackAnimation; } }

    public override float UltimateManaCost => 20;

    public override int ID => Fire;

    public override void CastRegularAttack() {
        //fireball
        GameObject instance = Instantiate(Resources.Load<GameObject>("Elements/Fire/Fireball"), HandUsing.handPos.position, transform.rotation);
        if (Targeter.Instance.Target) {
            instance.transform.LookAt(Targeter.Instance.CollisionPoint);
        }
        ElementControlV2.Instance.currentMana -= RegularManaCost;
    }

    public override void CastUltimateAttack() {
        //fire pit
        GameObject firepit = Instantiate(Resources.Load<GameObject>("Elements/Fire/Fire Pit/Blaze"), TargetingReticle.Instance.transform.position, Quaternion.identity);
    }
}