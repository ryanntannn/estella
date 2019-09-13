using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public float speed;
    public int currentRight, currentLeft;

    Rigidbody rb;
    Element[] elements;
    List<Element> rightHandElement = new List<Element>();
    List<Element> leftHandElement = new List<Element>();
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        elements = GetComponents<Element>();
        foreach(Element e in elements) {
            if (e.isRightHand) {
                rightHandElement.Add(e);
            }else {
                leftHandElement.Add(e);
            }
        }
        currentLeft = 0;
        currentRight = 0;
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
            leftHandElement[currentLeft++].shutDown();
            currentLeft %= leftHandElement.Count;
            leftHandElement[currentLeft].enabled = true;
        }else if (Input.GetKeyDown(KeyCode.E)){
            rightHandElement[currentRight++].shutDown();
            currentRight %= rightHandElement.Count;
            rightHandElement[currentRight].enabled = true;
        }
    }
}

