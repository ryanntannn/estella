using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EarthV2 : BaseElementV2 {
    public override Hand HandUsing { get; set; }

    [SerializeField]
    private AnimationClip m_regularAttackAnimation;
    public override AnimationClip RegularAttackAnimation { get { return m_regularAttackAnimation; } }

    public override float RegularManaCost => 20;

    [SerializeField]
    private AnimationClip m_ultimateAttackAnimation;
    public override AnimationClip UltimateAttackAnimation { get { return m_ultimateAttackAnimation; } }

    public override float UltimateManaCost => 20;

    public override int ID => Earth;

    public override void CastRegularAttack() {
        //fissure
        GameObject fissure = Instantiate(Resources.Load<GameObject>("Elements/Ground/FissureAttack"), TargetingReticle.Instance.transform.position - TargetingReticle.Instance.transform.up * 2, TargetingReticle.Instance.transform.rotation);
    }

    public override void CastUltimateAttack() {
        //earth splitter
        GameObject earthSplitter = Instantiate(Resources.Load<GameObject>("Elements/Ground/Ground Strike/EarthStrike"), TargetingReticle.Instance.transform.position, Quaternion.identity);

    }
}