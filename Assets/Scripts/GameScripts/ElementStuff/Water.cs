using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Elements/Water")]
public class Water : Element {
    public override string BigAttackTrigger { get { return "WhenBigAttack"; } }
    public override string SmallAttackTrigger { get { return "WhenSmallAttack"; } }

    public override string ElementName { get { return "Water"; } }

	public override int SmallAttackCost => 6;

	public override int BigAttackCost => 10;

	public override void DoBasic(ElementControl agent, Hand hand) {
		//bubble shot
		GameObject instance = Instantiate(Resources.Load<GameObject>("Elements/Water/BubbleShot"), hand.handPos.position, hand.transform.rotation);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity)) {
            Debug.Log(hitInfo.collider.gameObject.name);
			Vector3 toLookAt = hitInfo.point;
			//toLookAt.y = instance.transform.position.y;
			instance.transform.LookAt(toLookAt);
		}
		agent.currentMana -= SmallAttackCost;
	}

	public override void DoBig(ElementControl agent, Hand hand) {
		//tsunami
		GameObject tsunami = Resources.Load<GameObject>("Elements/Water/Tsunami");
		tsunami = Instantiate(tsunami, agent.transform.position - agent.transform.forward - agent.transform.up, agent.transform.rotation);
		agent.currentMana -= BigAttackCost;
	}
}
