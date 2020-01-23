using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSlashAction : GoapAction {
    public override bool Act(GameObject agent) {
        agent.transform.GetChild(0).GetComponent<Animator>().SetTrigger("WhenSpin");
        return true;
    }

    public override bool CheckPreconditions(GameObject agent) {
        target = GameObject.FindWithTag("Player");
        return target;
    }

    public override void DoReset() {
        
    }

    public override bool InRange() {
        return (target.transform.position - transform.position).magnitude <= 0.5f;
    }

    public override bool IsDone() {
        return transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Spin");
    }

    public override bool NeedRange() {
        return true;
    }

    // Start is called before the first frame update
    void Start() {
        Preconditions.Add(new KeyValuePair<string, object>("isAlive", true));
        Effects.Add(new KeyValuePair<string, object>("playerHealth", false));
    }

    // Update is called once per frame
    void Update() {

    }
}
