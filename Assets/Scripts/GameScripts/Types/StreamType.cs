using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Types/Stream")]
public class StreamType : Type {
    //[HideInInspector]
    public float maxResource = 10, regenRate = 1,depeletionRate = 1, currentResource = 10;
    public float range = 3;

    public override void alwaysUpdate(Element element) {
        if (element.ps.isStopped) {
            currentResource = Mathf.Clamp(currentResource + regenRate * Time.deltaTime, 0, maxResource);
            //regen resource
        }
    }

    public override void start(Element element) {
        currentResource = maxResource;
        //reset that shit
    }

    public override void update(Element element) {
        if(Input.GetMouseButtonDown(element.isRightHand ? 1 : 0)) {
            if (element.ps.isStopped) {
                element.ps.Play();
            }
        }//start attack

        if(Input.GetMouseButtonUp(element.isRightHand ? 1 : 0)) {
            if (element.ps.isPlaying) {
                element.ps.Stop();
            }
        }//graceful shutdown

        if (element.ps.isPlaying && currentResource > 0) { //we use ps.isplaying to check if we are attacking
            RaycastHit hitInfo;
            int mask = 1 << 9;
            Ray ray = new Ray(element.transform.position, element.transform.forward);
            if (Physics.Raycast(ray, out hitInfo, range, mask)) {
                element.onHit(hitInfo.collider.gameObject);
            }//hit the enemy
            currentResource = Mathf.Clamp(currentResource - depeletionRate * Time.deltaTime, 0, maxResource);
            //reduce resource
            if(currentResource <= 0) {
                element.ps.Stop();
            }
        }else {
            currentResource = Mathf.Clamp(currentResource + regenRate * Time.deltaTime, 0, maxResource);
            //regen resource
            
        }
    }
}

