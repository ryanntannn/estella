using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterElement : RightHandElement {
    public override void OnHit(GameObject other) {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        //finds rigidbody
        if(rb){ //should not be needed, but just incase
            rb.velocity += (other.transform.position - transform.position).normalized * 3;
        }
        //knockback the target
    }
}
