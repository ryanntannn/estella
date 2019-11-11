using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFissure : FissureScript {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.layer == Layers.Player) {
            //heal player
        }
    }
}
