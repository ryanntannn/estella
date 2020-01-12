using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Elements/Wind")]
public class Wind : Element {
    public override string ElementName { get { return "Wind"; } }

    public override void DoBasic(ElementControl agent, Hand hand) {
        //win slash
    }

    public override void DoBig(ElementControl agent, Hand hand) {
        GameObject tornado = Instantiate(Resources.Load<GameObject>("Elements/Wind/Tornado"), agent.targetCircle.transform.position, agent.targetCircle.transform.rotation);
        //tornado.transform.parent = createdByPlayer;
        agent.isCasting = false;
    }
}
