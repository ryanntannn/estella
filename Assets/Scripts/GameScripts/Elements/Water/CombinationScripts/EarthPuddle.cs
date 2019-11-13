using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPuddle : WaterPuddle {
    // Start is called before the first frame update
    void Start() {
        timeToLive = 10;
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
    }

    public override void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == Layers.Enemy) {
            //honestly don't know what this should do lmao
        }
    }
}
