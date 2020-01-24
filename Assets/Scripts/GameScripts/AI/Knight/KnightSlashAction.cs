using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSlashAction : GoapAction {
    float timeStart = -1;
    public override bool Act(GameObject agent) {
        if (timeStart == -1) {
            agent.transform.GetChild(0).GetComponent<Animator>().SetTrigger("WhenSpin");
            timeStart = Time.time;
        }

        if(Time.time - timeStart > 2) {
            timeStart = -1;
            return true;
        }
        return false;
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
        return Time.time - timeStart > 2;
    }

    public override bool NeedRange() {
        return true;
    }

    // Start is called before the first frame update
    void Start() {
        Preconditions.Add(new KeyValuePair<string, object>("isAlive", true));
        Preconditions.Add(new KeyValuePair<string, object>("hasYelled", true));
        Effects.Add(new KeyValuePair<string, object>("damagePlayer", true));
        Effects.Add(new KeyValuePair<string, object>("hasYelled", false));
    }

    // Update is called once per frame
    void Update() {

    }
}
