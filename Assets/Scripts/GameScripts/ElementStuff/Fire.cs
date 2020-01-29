using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Elements/Fire")]
public class Fire : Element {
    public override string BigAttackTrigger { get { return "WhenFirepit"; } }
    public override string SmallAttackTrigger { get { return "WhenSmallAttack"; } }

    public override string ElementName { get { return "Fire"; } }

    public override void DoBasic(ElementControl agent, Hand hand) {
        //fireball
        GameObject instance = Instantiate(Resources.Load<GameObject>("Elements/Fire/Fireball"), hand.handPos.position, hand.transform.rotation);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if(Physics.Raycast(ray, out hitInfo, 100)) {
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
        //fire pit
        GameObject firepit = Instantiate(Resources.Load<GameObject>("Elements/Fire/Firepit"), agent.targetCircle.transform.position, Quaternion.identity);
    }
}