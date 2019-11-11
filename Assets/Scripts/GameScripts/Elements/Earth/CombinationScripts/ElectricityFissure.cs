using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityFissure : FissureScript {
    // Start is called before the first frame update
    void Start() {
        timeToLive = 10;
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
    }

    public override void OnCollisionEnter(Collision collision) {
        
    }

    public void OnTriggerEnter(Collider other) {
        //tase the nigga that comes close
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().ReactElectricity(Element.Types.Bolt);
        }
    }
}
