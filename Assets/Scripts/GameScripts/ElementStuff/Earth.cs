using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Elements/Earth")]
public class Earth : Element {
    public override string BigAttackTrigger { get { return "WhenEarthStrike"; } }
    public override string SmallAttackTrigger { get { return "WhenSmallAttack"; } }

    public override string ElementName { get { return "Earth"; } }

	public override int SmallAttackCost => 5;

	public override int BigAttackCost => 10;

	public override void DoBasic(ElementControl agent, Hand hand) {
        //fissure
        GameObject fissure = Instantiate(Resources.Load<GameObject>("Elements/Ground/FissureAttack"), agent.targetCircle.transform.position - agent.transform.up * 2, agent.targetCircle.transform.rotation);
		agent.currentMana -= SmallAttackCost;
	}

	public override void DoBig(ElementControl agent, Hand hand) {
        //ground breaker
        GameObject groundBreaker = Instantiate(Resources.Load<GameObject>("Elements/Ground/Ground Strike/EarthStrike"), agent.targetCircle.transform.position - agent.targetCircle.transform.up * 5, Quaternion.identity);
        groundBreaker.GetComponent<EarthStrikeScript>().finalYPos = agent.targetCircle.transform.position.y;
    }
}
