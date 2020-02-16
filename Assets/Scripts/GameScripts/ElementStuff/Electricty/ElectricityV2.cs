using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ElectricityV2 : BaseElementV2 {
    public override Hand HandUsing { get; set; }

    [SerializeField]
    private AnimationClip m_regularAttackAnimation;
    public override AnimationClip RegularAttackAnimation { get { return m_regularAttackAnimation; } }

    public override float RegularManaCost => 10;

    [SerializeField]
    private AnimationClip m_ultimateAttackAnimation;
    public override AnimationClip UltimateAttackAnimation { get { return m_ultimateAttackAnimation; } }

    public override float UltimateManaCost => 20;

    public override int ID => Electricity;

    public override void CastRegularAttack() {
        //plasmashotgun
        GameObject instance = Instantiate(Resources.Load<GameObject>("Elements/Electricity/Plasma Shotgun/PlasmaShotgun"), HandUsing.handPos.transform.position, HandUsing.transform.rotation);
    }

    public override void CastUltimateAttack() {
        //lightning strike
        GameObject instance = Instantiate(Resources.Load<GameObject>("Elements/Electricity/LightningStrikeOne"), TargetingReticle.Instance.transform.position, Quaternion.identity);
    }
}