using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityPuddle : WaterPuddle {
    // Start is called before the first frame update
    void Start() {
        timeToLive = 10;
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
    }

    public override void OnTriggerEnter(Collider other) {
        //apply slow and zap zap
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().ReactElectricity(Element.Types.Stream);
            other.GetComponent<Enemy>().ReactWater(Element.Types.Power);
        }
        
    }
}
