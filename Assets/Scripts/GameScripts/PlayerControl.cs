using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public float speed;
    public Animator animator;

    private GameObject pivot;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        pivot = GameObject.Find("camera-pivot");
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
    }
}

