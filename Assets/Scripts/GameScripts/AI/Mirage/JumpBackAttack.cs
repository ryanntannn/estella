using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBackAttack : MinMaxAction {
    public override float Reward { get { return 10; } } //ten dmg

    public float cooldown = 5f;
    public Animator anim;

    float internalCounter = 0;
    bool onCd = false;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (onCd) {
            internalCounter += Time.deltaTime;
            if(internalCounter >= cooldown) {
                onCd = false;
                internalCounter = 0;
            }
        }
    }

    public override void Act() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("JumpBack")) {  //ensure only trigger once
            anim.SetTrigger("whenJump");
        }
    }

    public override bool CheckIfDoable() {
        return onCd;
    }
}
