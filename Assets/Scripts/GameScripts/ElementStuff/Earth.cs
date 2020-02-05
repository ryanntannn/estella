using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Elements/Earth")]
public class Earth : Element {
    public override string BigAttackTrigger { get { return "WhenGroundbreaker"; } }
    public override string SmallAttackTrigger { get { return "WhenSmallAttack"; } }

    public override string ElementName { get { return "Earth"; } }

    public override void DoBasic(ElementControl agent, Hand hand) {
        //fissure
        GameObject fissure = Instantiate(Resources.Load<GameObject>("Elements/Ground/FissureAttack"), agent.targetCircle.transform.position - agent.transform.up * 2, agent.targetCircle.transform.rotation);
    }

    public override void DoBig(ElementControl agent, Hand hand) {
        //ground breaker
        GameObject groundBreaker = Instantiate(Resources.Load<GameObject>("Elements/Ground/GroundBreaker"), agent.targetCircle.transform.position, Quaternion.identity);
        //groundBreaker.transform.parent = createdByPlayer;
    }
}
