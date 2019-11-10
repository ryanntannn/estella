using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : Projectile {
    //future use
    Animator anim;

    public override string elementName {
        get { return "Fire"; }
    }
    public override float speed {
        get { return 12; }
    }

    private void Start() {

    }

    private void Update() {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other) {
        int otherLayer = other.gameObject.layer;
        if (otherLayer == Layers.Enemy) {
            other.GetComponent<Enemy>().ReactFire(Element.Types.Bolt);
            Destroy(gameObject);
        } else if (otherLayer == Layers.Terrain) {
            //can do some flame extingush here
            Destroy(gameObject);
        }
    }
}
