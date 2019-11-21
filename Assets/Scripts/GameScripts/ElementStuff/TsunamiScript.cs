using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TsunamiScript : MonoBehaviour {
    public float speed = 12;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other) {
        float force = 10;
        if(other.gameObject.layer == Layers.Enemy) {
            //play some knockback animation?

            //push enemy back
            float resist = 1 / other.GetComponent<Enemy>().resistanceLevel;
            other.GetComponent<Rigidbody>().AddForce(-transform.forward * force * resist);
        }else if(other.gameObject.layer == Layers.Terrain) {
            //when collide with terrain
            Destroy(gameObject);
        }
    }
}
