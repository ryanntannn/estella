using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public float speed;
    public int currentRight, currentLeft;

    Rigidbody rb;
    Element[] elements;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        elements = GetComponents<Element>();

        currentLeft = 0;
        currentRight = 2;
    }

    // Update is called once per frame
    void Update() {
        rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal"), rb.velocity.y, Input.GetAxisRaw("Vertical"));
        foreach(Element e in elements) {
            if (!e.enabled) {
                e.alwaysUpdate();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            elements[currentLeft++].shutDown();
            currentLeft %= 2;   //TODO: make sure this one not a predefined value
            elements[currentLeft].enabled = true;
        }else if (Input.GetKeyDown(KeyCode.E)){

        }
    }
}

