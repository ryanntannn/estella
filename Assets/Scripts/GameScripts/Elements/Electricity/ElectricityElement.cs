using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityElement : LeftHandElement {
    public override void OnHit(GameObject other) {
        //same concept as fire
        foreach (Transform child in other.transform) {
            if (child.name.Equals("ElectricityOnHit")) {
                child.GetComponent<OnHit>().timer += types[currentType].effectTime * Time.deltaTime;
                //start/continue the particle effect
                break;
            }
        }
    }
}