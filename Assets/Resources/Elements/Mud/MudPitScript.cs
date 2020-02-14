using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudPitScript : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        gameObject.KillSelf(10);
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().DebuffEnemy(Time.deltaTime, Enemy.Effects.Slow);
        }
    }
}
