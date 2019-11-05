using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public float speed;
    public int currentRight, currentLeft;
    public Animator animator;

    private GameObject pivot;
    Rigidbody rb;
    Element[] rightHandElement, leftHandElement;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        rightHandElement = GetComponents<RightHandElement>();
        leftHandElement = GetComponents<LeftHandElement>();
        pivot = GameObject.Find("camera-pivot");

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
        MovementUpdate();
        ElementUpdate();
    }

    void MovementUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            float inputAngle = Mathf.Atan2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, pivot.transform.eulerAngles.y + inputAngle, transform.eulerAngles.z);
            rb.velocity = transform.forward * speed;
            animator.SetBool("running", true);
        } else
        {
            animator.SetBool("running", false);
        }
    }

    void ElementUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            leftHandElement[currentLeft++].ShutDown();
            currentLeft %= leftHandElement.Length;
            leftHandElement[currentLeft].enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            rightHandElement[currentRight++].ShutDown();
            currentRight %= rightHandElement.Length;
            rightHandElement[currentRight].enabled = true;
        }
    }
}

