using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Types/Bolt")]
public class BoltType : Type {
    public float maxResource = 5, currentResource, regenRate = 1, depeletionRate = 2;
    public float fireRate = 1;  //per second

    private float internalCounter;
    public override void alwaysUpdate(Element element) {
        currentResource = Mathf.Clamp(currentResource + regenRate * Time.deltaTime, 0, maxResource);
    }

    public override void start(Element element) {
        internalCounter = 0;
        currentResource = maxResource;
    }

    public override void update(Element element) {
        if(internalCounter <= 0) {
            if (Input.GetKey(element.getKey())) {
                GameObject instance = Instantiate(thing, element.transform.position, element.transform.rotation);
                //make a bolt
                internalCounter = 1 / fireRate; //reset timer
            }
        }else {
            internalCounter -= Time.deltaTime;
            //lower timer
        }
    }
}
