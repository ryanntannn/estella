using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosPlasma : MonoBehaviour {
    private Enemy m_dataProvider;
    public Enemy Skylark { set { m_dataProvider = value; } }

    // Start is called before the first frame update
    void Start() {
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Player) {
            m_dataProvider.DealDamage(30);
        }
    }
}
