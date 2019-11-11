using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFissure : FissureScript {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
    }

    private void OnTriggerEnter(Collider other) {
        //push them away
        if(other.gameObject.layer == Layers.Enemy) {
            //work around
            //in hindsight should have used a vector3 instead of a transform
            other.GetComponent<Enemy>().ReactWind(Element.Types.Stream, other.ClosestPoint(transform.position));
        }
    }
}
