using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTornadoScript : TornadoScript {
    protected override void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().DebuffEnemy(Time.deltaTime * 2, Enemy.Effects.Burn);
        }
    }
}
