using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//mirage boss fight
public class Mirage : Enemy {
    public float moveSpeed = 3;
    public float lengthOfDash = 6;
    public Animator anim;

    public GameObject mirageShadow;

    GameObject player;
    // Start is called before the first frame update
    public override void Start() {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        if (!anim) anim = transform.GetComponentInChildren<Animator>(); //if nothing set
    }

    //Update is called once per frame
    public override void Update() {
        base.Update();

    }

    public void JumpBack() {
        //add force to the foot of mirage
        //ey look man, the numbers work, so lets not change it
        //rb.AddForce(transform.up * 4, ForceMode.Impulse);
        rb.AddForce(-transform.forward * 10, ForceMode.Impulse);
        GameObject instance = Instantiate(mirageShadow, transform.position, Quaternion.identity);
        instance.transform.LookAt(player.transform.position, transform.up);
    }
}
