using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//don't wanna inherit from the player projectile
public class MirageProjectile : MonoBehaviour {

    Rigidbody rb;
    // Start is called before the first frame update
    void Start() {
        //give it collider
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.size = new Vector3(0.01f, 0.09f, 0.02f);   //extremely preicise trail and error 
        collider.isTrigger = true;

        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.AddForce(transform.up * 300);
        rb.drag = 0.5f;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
