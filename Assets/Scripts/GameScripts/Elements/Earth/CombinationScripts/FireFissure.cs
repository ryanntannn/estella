using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFissure : FissureScript {
    // Start is called before the first frame update
    void Start() {
        timeToLive = 10;
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
    }

    private void OnTriggerStay(Collider other) {
        //people in radius gets set on fire
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().ReactFire(Element.Types.Stream);
        }
    }
}
