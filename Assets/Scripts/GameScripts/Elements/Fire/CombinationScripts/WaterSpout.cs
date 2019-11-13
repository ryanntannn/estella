using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpout : FireSpoutScript {
    // Start is called before the first frame update
    void Start() {
        timeToLive = 10;
    }

    // Update is called once per frame
    public override void Update() {

    }

    public override void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == Layers.Enemy) {

        }
    }
}
