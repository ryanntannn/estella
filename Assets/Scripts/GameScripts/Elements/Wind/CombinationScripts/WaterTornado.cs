using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTornado : TornadoScript {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    public override void Update() {
        base.Update();

        //leave a trail of water that slows down enemies

    }

    public override void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().ReactWind(Element.Types.Power, transform.position);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().ReactWater(Element.Types.Stream);
        }
    }
}
