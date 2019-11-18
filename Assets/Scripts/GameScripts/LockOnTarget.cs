using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnTarget : MonoBehaviour {
    public float radius = 3;

    public GameObject target = null;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //non block
        StartCoroutine("SetTarget");
        if (target) {
            Debug.DrawLine(target.transform.position + transform.up, transform.position);
        }
    }

    //if got issue just remove the ienum shit
    IEnumerator SetTarget() {
        RaycastHit[] hits = Physics.CapsuleCastAll(transform.position - transform.up, transform.position + transform.up, radius, Vector3.up, radius, 1 << Layers.Enemy);
        if(hits.Length <= 0) {
            target = null;
            yield return null;
        }
        //find closest enemy
        float lowestDist = Mathf.Infinity;
        if (target) {
            lowestDist = (target.transform.position - transform.position).magnitude;
        }
        foreach (RaycastHit hit in hits) {   //go thru all enemies in an area
            //check if got thing between
            float distInbetween = (transform.position - hit.transform.position).magnitude;
            if (!Physics.Raycast(transform.position, hit.transform.position, (transform.position - hit.transform.position).magnitude) && distInbetween < lowestDist) {
                target = hit.collider.gameObject;
            }
            yield return null;
        }
    }
}
