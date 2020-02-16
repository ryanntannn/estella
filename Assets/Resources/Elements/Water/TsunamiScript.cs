using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TsunamiScript : MonoBehaviour {
    public float speed = 12;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == Layers.Terrain) {
            //when collide with terrain
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<NavMeshAgent>().Warp(other.transform.position + transform.forward * Time.deltaTime * 5);
        }
    }
}
