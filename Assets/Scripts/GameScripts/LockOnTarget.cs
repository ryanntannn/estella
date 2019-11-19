using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LockOnTarget : MonoBehaviour {
    public float radius = 3;

    public GameObject target = null;
    public Shader shd;

    GameObject prevTarget = null;
    public Shader defaultShd;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //non block
        //StartCoroutine("SetTarget");
        SetTarget();

        if (target) {
            target.transform.ChangeShader(shd);
            Debug.DrawLine(target.transform.position + transform.up, transform.position);
        }

        if (prevTarget != target) {
            if(prevTarget) prevTarget.transform.ChangeShader(defaultShd);
        }
    }

    //if got issue just remove the ienum shit
    void SetTarget() {
        prevTarget = target;
        RaycastHit[] hits = Physics.CapsuleCastAll(transform.position - transform.up, transform.position + transform.up, radius, Vector3.up, radius, 1 << Layers.Enemy);
        if (hits.Length <= 0) {
            target = null;
            return;
        }
        //find closest enemy
        float lowestDist = Mathf.Infinity;  //if no target
        if (target) {
            lowestDist = (target.transform.position - transform.position).magnitude;    //distance btwn currentTarget and player
        }
        foreach (RaycastHit hit in hits) {   //go thru all enemies in an area
            //check if got thing between
            float distInbetween = (transform.position - hit.transform.position + transform.up).magnitude;  //distbwtn player and hit.position  
            if (!Physics.Raycast(transform.position, hit.transform.position + transform.up, distInbetween, 1 << Layers.Player) && distInbetween < lowestDist) {    //if raycast not hit and dist is smaller
                target = hit.collider.gameObject;
            }
        }
    }
}
