using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//mirage boss fight
public class Mirage : Enemy {
    public float moveSpeed = 3;
    public float lengthOfDash = 6;
    public Animator anim;

    GameObject player;

    // Start is called before the first frame update
    public override void Start() {
        base.Start();
        resistanceLevel = 10;
        player = GameObject.FindGameObjectWithTag("Player");
        if (!anim) anim = transform.GetComponentInChildren<Animator>(); //if nothing set
    }

    //Update is called once per frame
    public override void Update() {
        anim.SetBool("isWalking", Input.GetKey(KeyCode.P));
        if (Input.GetKeyDown(KeyCode.L)) {
            anim.SetTrigger("throwKnife");
        }
    }
}
