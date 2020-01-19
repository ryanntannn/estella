﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Elements/Water")]
public class Water : Element {
    public override string BigAttackTrigger { get { return "WhenTsunami"; } }
    public override string SmallAttackTrigger { get { return "WhenSmallAttack"; } }

    public override string ElementName { get { return "Water"; } }

	public override void DoBasic(ElementControl agent, Hand hand) {
		//bubble shot
		GameObject instance = Instantiate(Resources.Load<GameObject>("Elements/Water/BubbleShot"), hand.handPos.position, hand.transform.rotation);
		//Vector3 newRot = Camera.main.transform.eulerAngles;
		//newRot.x = 0;
		//instance.transform.rotation = Quaternion.Euler(newRot);

		//agent.isCasting = false;
		//instance.transform.parent = createdByPlayer;
	}

	public override void DoBig(ElementControl agent, Hand hand) {
		//tsunami
		GameObject tsunami = Resources.Load<GameObject>("Elements/Water/Tsunami");
		tsunami = Instantiate(tsunami, agent.transform.position - agent.transform.forward - agent.transform.up, agent.transform.rotation);
		//tsunami.transform.parent = createdByPlayer;
	}
}
