using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormScript : MonoBehaviour {
    public int noOfSparks = 5;

    GameObject sparkWraiths;
    Vector3 centerOfStorm;
    // Start is called before the first frame update
    void Start() {
        centerOfStorm = transform.position + transform.up * 2.5f;
        sparkWraiths = Resources.Load<GameObject>("Elements/Storm/SparkWraith");

        float step = 360 / noOfSparks;
        for(int count = 0; count <= noOfSparks - 1; count++) {
            GameObject instance = Instantiate(sparkWraiths, transform.position, Quaternion.Euler(-30, count * step, 0));
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
