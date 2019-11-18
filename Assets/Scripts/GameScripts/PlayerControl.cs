using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public float speed;
    public Animator animator;

    public KeyCode leftHandButton = KeyCode.Mouse0, rightHandButton = KeyCode.Mouse1;   //activating elements
    public KeyCode swapLeft = KeyCode.Q, swapRight = KeyCode.E; //swapping elements


    private GameObject pivot;
    Rigidbody rb;
    int leftHand = 0, rightHand = 2;    //keep track of which one being used
    const int NO_OF_LEFT = 2, NO_OF_RIGHT = 3;  //number of elements belonging on left and right hand
    Element[] elements;
    float rotation = 0;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        pivot = GameObject.Find("camera-pivot");

        //element stuff
        elements = GetComponents<Element>();
        foreach (Element e in elements) {
            e.button = e.isRightHand ? rightHandButton : leftHandButton;
            e.Disable();
        }

        //enable defaults
        elements[leftHand].Enable();
        elements[rightHand].Enable();
    }

    // Update is called once per frame
    void Update() {
        ElementUpdate();
        MovementUpdate();
    }

    void MovementUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            rotation = Mathf.Atan2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Mathf.Rad2Deg;
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotAngle + pivot.transform.eulerAngles.y, transform.eulerAngles.z);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, rotation + pivot.transform.eulerAngles.y, transform.eulerAngles.z), Time.deltaTime * 15);
            rb.velocity = transform.forward * speed;
            animator.SetBool("running", true);
        } else
        {
            animator.SetBool("running", false);
        }
    }

    void ElementUpdate()
    {
        if (Input.GetKeyDown(swapLeft)) {
            elements[leftHand++].Disable();   //disable the currentLeft then bump it
            leftHand %= NO_OF_LEFT; //prevent overflow
            elements[leftHand].Enable();    //enable new left
        }

        if (Input.GetKeyDown(swapRight)) {
            elements[rightHand].Disable();
            rightHand -= NO_OF_LEFT;
            rightHand++;
            rightHand %= NO_OF_RIGHT;
            rightHand += NO_OF_LEFT;
            elements[rightHand].Enable();
        }//same stuff here
    }
}

