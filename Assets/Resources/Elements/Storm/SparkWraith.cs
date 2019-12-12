using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkWraith : MonoBehaviour {
    public GameObject target;
    public float speed = 5;
    public Vector3 center;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (!target) {
            //idle
        }else {
            //chase target
            //look at target
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position, transform.up), Time.deltaTime);
        }
    }
}
