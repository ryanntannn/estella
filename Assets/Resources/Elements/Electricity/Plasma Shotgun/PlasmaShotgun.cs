using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaShotgun : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        gameObject.KillSelf(5.0f);
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().TakeDamage(20);
            other.GetComponent<Enemy>().DebuffEnemy(2, Enemy.Effects.Stun);
        }
    }
}
