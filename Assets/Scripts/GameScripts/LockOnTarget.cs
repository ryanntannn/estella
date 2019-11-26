using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LockOnTarget : MonoBehaviour {
    public float radius = 3;
    public GameObject indicator;

    public GameObject target;

    GameObject cameraGO; 

    // Start is called before the first frame update
    void Start() {
        cameraGO = GameObject.Find("Main Camera");
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
        //RaycastHit hitInfo;
        RaycastHit[] hitInfo = Physics.SphereCastAll(ray, radius, 7, 1 << Layers.Enemy);
        GameObject closest = null;
        float closestDistance = 0f; 
        foreach(RaycastHit hit in hitInfo)
        {
            float distance = Vector3.Cross(ray.direction, hit.transform.position - ray.origin).magnitude;
            if(closest)
            {
                //Checks if target is not between the camera and the player
                if(distance < closestDistance && Vector3.Distance(hit.transform.position, cameraGO.transform.position) > 3f)
                {
                    closest = hit.transform.gameObject;
                    closestDistance = distance;
                }
            } else
            {
                closest = hit.transform.gameObject;
                closestDistance = distance;
            }
        }
        target = closest;
        //target = Physics.SphereCast(ray, radius, out hitInfo, 7, 1 << Layers.Enemy) ? hitInfo.collider.gameObject : null;
    }
}
