using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightYellAction : GoapAction {
    public override bool Act(GameObject agent) {
        agent.transform.GetChild(0).GetComponent<Animator>().SetTrigger("WhenYell");
        return true;
    }

    public override bool CheckPreconditions(GameObject agent) {
        return true;
    }

    public override void DoReset() {
        
    }

    public override bool InRange() {
        return false;
    }

    public override bool IsDone() {
        return !transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Yell");
    }

    public override bool NeedRange() {
        return false;
    }

    // Start is called before the first frame update
    void Start() {
        Preconditions.Add(new KeyValuePair<string, object>("isAlive", true));
        Effects.Add(new KeyValuePair<string, object>("hasYelled", true));
    }

    // Update is called once per frame
    void Update() {

    }
}
