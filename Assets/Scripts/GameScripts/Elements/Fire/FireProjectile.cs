using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : Projectile {
    //future use
    Animator anim;

    public GameObject target;

    public override string elementName {
        get { return "Fire"; }
    }
    public override float speed {
        get { return 12; }
    }

    private void Start() {

    }

    private void Update() {
        if (target) {
            Vector3 vecTarget = target.transform.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(vecTarget);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 5);
        }
        transform.position += transform.forward * speed * Time.deltaTime;

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
