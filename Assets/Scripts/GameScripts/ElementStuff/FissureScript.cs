using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FissureScript : MonoBehaviour {
    public float totalTime = 2;

    GameObject mesh;
    float internalCounter = 0;
    // Start is called before the first frame update
    void Start() {
        mesh = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update() {
        if(internalCounter <= totalTime) {
            internalCounter += Time.deltaTime;
            mesh.transform.position += transform.up * Time.deltaTime;
        }
    }
}
