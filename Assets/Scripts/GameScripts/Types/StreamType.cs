using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Types/Stream")]
public class StreamType : Type {
    //[HideInInspector]
    public float maxResource = 10, regenRate = 1,depeletionRate = 1, currentResource = 10;
    public float range = 3;

    private ParticleSystem ps;
    public override void AlwaysUpdate(Element element) {
        if (ps.isStopped) {
            currentResource = Mathf.Clamp(currentResource + regenRate * Time.deltaTime, 0, maxResource);
            //regen resource
        }
    }

    public override void TypeStart(Element element) {
        currentResource = maxResource;
        //reset that shit

        //set the ps
        //ps = element.gameObject.FindChildWithTag(element.GetType().Name).GetComponent<ParticleSystem>();
        ps = element.gameObject.FindComponentOfChildWithTag<ParticleSystem>(element.GetType().Name);
    }

    public override void TypeUpdate(Element element) {
        if (Input.GetKeyDown(element.GetKey())) {
            if (ps.isStopped) {
                ps.Play();
            }
        }//start attack

        if(Input.GetKeyUp(element.GetKey())) {
            if (ps.isPlaying) {
                ps.Stop();
            }
        }//graceful shutdown

        if (ps.isPlaying && currentResource > 0) { //we use ps.isplaying to check if we are attacking
            RaycastHit hitInfo;
            int mask = 1 << 9;
            Ray ray = new Ray(element.transform.position, element.transform.forward);
            if (Physics.Raycast(ray, out hitInfo, range, mask)) {
                element.OnHit(hitInfo.collider.gameObject);
            }//hit the enemy
            currentResource = Mathf.Clamp(currentResource - depeletionRate * Time.deltaTime, 0, maxResource);
            //reduce resource
            if(currentResource <= 0) {
                ps.Stop();
            }
        }else {
            currentResource = Mathf.Clamp(currentResource + regenRate * Time.deltaTime, 0, maxResource);
            //regen resource
            
        }
    }
}

