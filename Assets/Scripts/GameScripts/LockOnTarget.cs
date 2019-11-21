using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LockOnTarget : MonoBehaviour {
    public float radius = 3;
    public GameObject indicator;

    GameObject target;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //non block
        //StartCoroutine("SetTarget");
        SetTarget();

        indicator.SetActive(target);

        if (target) {
            indicator.transform.position = target.transform.position + target.transform.up * 2;
        }

    }

    //if got issue just remove the ienum shit
    void SetTarget() {
        //what we want is a targeting system that targets enemies closest to the cross hair
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        target = Physics.SphereCast(ray, radius, out hitInfo, 5, 1 << Layers.Enemy) ? hitInfo.collider.gameObject : null;
    }
}
