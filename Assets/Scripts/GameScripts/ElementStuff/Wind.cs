using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Elements/Wind")]
public class Wind : Element {
    public override string BigAttackTrigger { get { return "WhenBigAttack"; } }
    public override string SmallAttackTrigger { get { return "WhenWindSlash"; } }

    public override string ElementName { get { return "Wind"; } }

	public override int SmallAttackCost => 7;

	public override int BigAttackCost => 10;

	public override void DoBasic(ElementControl agent, Hand hand) {
        //wind slash
        GameObject ws = Instantiate(Resources.Load<GameObject>("Elements/Wind/WindSlash/WindSlash"), hand.handPos.transform);
        //ws.GetComponent<WindSlashScript>().ec = agent;

		agent.currentMana -= SmallAttackCost;
    }

    public override void DoBig(ElementControl agent, Hand hand) {
        GameObject tornado = Instantiate(Resources.Load<GameObject>("Elements/Wind/Tornado/Tornado"), agent.targetCircle.transform.position, agent.targetCircle.transform.rotation);
		agent.currentMana -= BigAttackCost;
	}
}
