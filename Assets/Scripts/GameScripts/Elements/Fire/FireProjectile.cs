using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour {
    public float speed = 12;

    //future use
    Animator anim;
    private void Start() {
        
    }

    private void Update() {
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        int otherLayer = other.gameObject.layer;
        if(otherLayer == Layers.Enemy) {
            other.GetComponent<Enemy>().fireTimeToLive += 5;
            Destroy(gameObject);
        } else if(otherLayer == Layers.Terrain) {
            //can do some flame extingush here
            Destroy(gameObject);
        }
    }
}
