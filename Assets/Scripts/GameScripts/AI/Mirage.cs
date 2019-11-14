using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//mirage boss fight
public class Mirage : Enemy {
    public float moveSpeed = 3;
    public float lengthOfDash = 6;

    GameObject player;

    // Start is called before the first frame update
    public override void Start() {
        base.Start();
        resistanceLevel = 10;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    //public override void Update() {
        
    //}


    
}
