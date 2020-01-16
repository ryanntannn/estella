using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormScript : MonoBehaviour {
    public int noOfSparks = 5;

    public GameObject sparkWraith;
    Vector3 centerOfStorm;
    GameObject[] sparkWraiths;
    // Start is called before the first frame update
    void Start() {
        centerOfStorm = transform.position + transform.up * 2.5f;
        sparkWraiths = new GameObject[noOfSparks];

        float step = 360 / noOfSparks;
        for(int count = 0; count <= noOfSparks - 1; count++) {
            sparkWraiths[count] = Instantiate(sparkWraith, transform.position, Quaternion.Euler(-30, count * step, 0));
        }
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            //set target of all wraiths without a target
            foreach(GameObject instance in sparkWraiths) {
                SparkWraith instanceCall = instance.GetComponent<SparkWraith>();
                if (!instanceCall.target) {
                    instanceCall.target = other.gameObject;
                }
            }
        }
    }
}
