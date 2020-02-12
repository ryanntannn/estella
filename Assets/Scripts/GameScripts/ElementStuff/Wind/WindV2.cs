using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WindV2 : BaseElementV2 {
    public override Hand HandUsing { get; set; }

    [SerializeField]
    private AnimationClip m_regularAttackAnimation;
    public override AnimationClip RegularAttackAnimation { get { return m_regularAttackAnimation; } }

    public override float RegularManaCost => 5;

    [SerializeField]
    private AnimationClip m_ultimateAttackAnimation;
    public override AnimationClip UltimateAttackAnimation { get { return m_ultimateAttackAnimation; } }

    public override float UltimateManaCost => 20;

    public override int ID => Wind;

    public override void CastRegularAttack() {
        //wind slash
        GameObject ws = Instantiate(Resources.Load<GameObject>("Elements/Wind/WindSlash/WindSlash"), HandUsing.handPos.transform);
    }

    public override void CastUltimateAttack() {
        //tornado
        GameObject tornado = Instantiate(Resources.Load<GameObject>("Elements/Wind/Tornado/Tornado"), TargetingReticle.Instance.transform.position, TargetingReticle.Instance.transform.rotation);
    }
}
