using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {
    [HideInInspector]
    public Transform firingPoint;

    Rigidbody rb;
    Transform original;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        original = transform.parent;
    }

    // Update is called once per frame
    void Update() {
        if (transform.parent) {
            transform.position = transform.parent.position;
            transform.LookAt(firingPoint);
        }else {
            //raycast out and find anything
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 2, ~(1 << Layers.Enemy))) {
                transform.position = hitInfo.point;
                transform.parent = hitInfo.collider.transform;

                Destroy(GetComponent<Rigidbody>());
                Destroy(GetComponent<Collider>());
                Destroy(this);
            }
        }


    }
}
