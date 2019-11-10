using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : Projectile {
    public GameObject waterPs;

    public override string elementName {
        get {
            return "Water";
        }
    }

    public override float speed {
        get {
            return 12;
        }
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other) {
        int otherLayer = other.gameObject.layer;
        if (otherLayer == Layers.Enemy) {
            //splash water
            GameObject instance = Instantiate(waterPs, transform.position, Quaternion.identity);
            Destroy(instance, 1);
            other.GetComponent<Enemy>().ReactWater(Element.Types.Bolt);
            Destroy(gameObject);
        } else if (otherLayer == Layers.Terrain) {
            Destroy(gameObject);
        }
    }
}
