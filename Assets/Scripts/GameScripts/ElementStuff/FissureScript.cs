using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FissureScript : MonoBehaviour {
    public float totalTime = 2;
    public float timeToLive = 5;

    GameObject mesh;
    float internalCounter = 0;
    // Start is called before the first frame update
    void Start() {
        mesh = transform.GetChild(0).gameObject;
        timeToLive += totalTime;
    }

    // Update is called once per frame
    void Update() {
        float deltaTime = Time.deltaTime;
        timeToLive -= deltaTime;
        if(internalCounter <= totalTime) {
            internalCounter += deltaTime;
            mesh.transform.position += transform.up * deltaTime;
        }

        if(timeToLive <= 0) {
            //can play some crumbling animation
            Destroy(gameObject);
        }
    }
}
