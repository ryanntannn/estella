using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPuddle : WaterPuddle {
    // Start is called before the first frame update
    void Start() {
        timeToLive = 10;
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
    }

    public override void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().ReactWind(Element.Types.Stream, transform.position);
            other.GetComponent<Enemy>().ReactWater(Element.Types.Power);
        }
    }
}
