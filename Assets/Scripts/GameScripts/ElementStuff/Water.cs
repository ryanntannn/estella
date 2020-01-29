using System;
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
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 100)) {
			Vector3 toLookAt = hitInfo.point;
			toLookAt.y = instance.transform.position.y;
			instance.transform.LookAt(toLookAt);
		} else {
			Vector3 toLookAt = ray.direction * hitInfo.distance;
			toLookAt.y = instance.transform.position.y;
			instance.transform.LookAt(toLookAt);
		}
	}

	public override void DoBig(ElementControl agent, Hand hand) {
		//tsunami
		GameObject tsunami = Resources.Load<GameObject>("Elements/Water/Tsunami");
		tsunami = Instantiate(tsunami, agent.transform.position - agent.transform.forward - agent.transform.up, agent.transform.rotation);
		//tsunami.transform.parent = createdByPlayer;
	}
}
