using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Types/Power")]
public class PowerType : Type {
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
        RaycastHit hitInfo;
        Ray ray = new Ray();
        if (internalCounter <= 0) {
            if (Input.GetKey(element.getKey())) {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            }else if(Input.GetKeyUp(element.getKey())) {
                if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity)) {
                    GameObject instance = Instantiate(thing, hitInfo.point, element.transform.rotation);
                    //place down the power
                    internalCounter = 1 / fireRate; //reset timer
                }
            }
        } else {
            internalCounter -= Time.deltaTime;
            //lower timer
        }
    }
}
