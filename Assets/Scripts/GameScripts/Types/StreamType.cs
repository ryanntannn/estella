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
    public override void alwaysUpdate(Element element) {
        if (ps.isStopped) {
            currentResource = Mathf.Clamp(currentResource + regenRate * Time.deltaTime, 0, maxResource);
            //regen resource
        }
    }

    public override void start(Element element) {
        currentResource = maxResource;
        //reset that shit

        //set the ps
        ps = Helper.findChildWithTag(element.gameObject, element.GetType().Name).GetComponent<ParticleSystem>();
    }

    public override void update(Element element) {
        if (Input.GetKeyDown(element.getKey())) {
            if (ps.isStopped) {
                ps.Play();
            }
        }//start attack

        if(Input.GetKeyUp(element.getKey())) {
            if (ps.isPlaying) {
                ps.Stop();
            }
        }//graceful shutdown

        if (ps.isPlaying && currentResource > 0) { //we use ps.isplaying to check if we are attacking
            RaycastHit hitInfo;
            int mask = 1 << 9;
            Ray ray = new Ray(element.transform.position, element.transform.forward);
            if (Physics.Raycast(ray, out hitInfo, range, mask)) {
                element.onHit(hitInfo.collider.gameObject);
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

