using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElement : Element {
    public override void onHit(GameObject other) {
        //set target on fire
        foreach(Transform child in other.transform) {
            if (child.name.Equals("FireOnHit")) {
                child.GetComponent<OnHit>().timer += types[currentType].effectTime * Time.deltaTime;
                //start/continue the particle effect
                break;
            }
        }
    }

}
