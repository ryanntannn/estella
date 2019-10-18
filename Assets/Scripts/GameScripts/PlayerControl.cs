using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public float speed;
    public int currentRight, currentLeft;

    Rigidbody rb;
    Element[] rightHandElement, leftHandElement;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        rightHandElement = GetComponents<RightHandElement>();
        leftHandElement = GetComponents<LeftHandElement>();

        currentLeft = 0;
        currentRight = 0;
        for(int count = 1; count <= rightHandElement.Length - 1; count++) {
            rightHandElement[count].enabled = false;
        }
        for (int count = 1; count <= leftHandElement.Length - 1; count++) {
            leftHandElement[count].enabled = false;
        }
    }

    // Update is called once per frame
    void Update() {
        rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal"), rb.velocity.y, Input.GetAxisRaw("Vertical"));


        if (Input.GetKeyDown(KeyCode.Q)) {
            leftHandElement[currentLeft++].ShutDown();
            currentLeft %= leftHandElement.Length;
            leftHandElement[currentLeft].enabled = true;
        }else if (Input.GetKeyDown(KeyCode.E)){
            rightHandElement[currentRight++].ShutDown();
            currentRight %= rightHandElement.Length;
            rightHandElement[currentRight].enabled = true;
        }
    }
}

