using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Elements/Fire")]
public class Fire : Element {
	public override string ElementName { get { return "Fire"; } }

	public override void DoBasic(ElementControl agent, Hand hand) {
		//fireball
		GameObject instance = Instantiate(Resources.Load<GameObject>("Elements/Fire/Fireball"), agent.transform.position, agent.transform.rotation);
		Vector3 newRot = Camera.main.transform.eulerAngles;
		newRot.x = 0;
		instance.transform.rotation = Quaternion.Euler(newRot);

		agent.isCasting = false;
	}

	public override void DoBig(ElementControl agent, Hand hand) {
		//fire pit
		GameObject firepit = Instantiate(Resources.Load<GameObject>("Elements/Fire/Firepit"), agent.targetCircle.transform.position, Quaternion.identity);
	}
}