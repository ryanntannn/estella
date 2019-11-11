using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityFissure : FissureScript {

    float internalCounter = 0;
    float zapRate = 1;  //per second
    List<Collider> niggasToZap = new List<Collider>();  //things in aoe
    // Start is called before the first frame update
    void Start() {
        timeToLive = 10;
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();

        internalCounter += Time.deltaTime;//bump counter
        if (internalCounter >= zapRate) {
            niggasToZap.ForEach(x => {
                x.GetComponent<Enemy>().ReactElectricity(Element.Types.Bolt);
            });
            internalCounter = 0;//reset counter
        }
    }

    public void OnTriggerEnter(Collider other) {
        //tase the nigga that comes close
        if(other.gameObject.layer == Layers.Enemy) {
            niggasToZap.Add(other);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (niggasToZap.Contains(other)) {
            niggasToZap.Remove(other);
        }
    }
}
