using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityTornado : TornadoScript {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
    }

    public override void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().ReactWind(Element.Types.Power, other.transform.position);
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().ReactElectricity(Element.Types.Stream);
        }
    }
}
