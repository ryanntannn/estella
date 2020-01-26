using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public float speed;
    private float cspeed;
    public float maxHealth = 100;
    public float currentHealth = 100;
    public Animator animator;
    public Element[] elements;

    public Hand rHand, lHand;
    public KeyCode swapLeft = KeyCode.Q, swapRight = KeyCode.E; //swapping elements

    private GameObject pivot;
    Rigidbody rb;
    float rotation = 0;

    //Radial Menu
    GameObject radialMenu1;
    GameObject radialMenu2;
    public bool isInRadialMenu;

	//element control
	ElementControl ec;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        pivot = GameObject.Find("camera-pivot");
        radialMenu1 = GameObject.Find("RadialMenu 1");
        radialMenu2 = GameObject.Find("RadialMenu 2");
        radialMenu1.SetActive(false);
        radialMenu2.SetActive(false);

        //init health
        currentHealth = maxHealth;

		//set ec
		ec = GetComponent<ElementControl>();

        //check for lHand rHand
        if (!lHand) {
            lHand = ec.lHand;
        }
        if (!rHand) {
            rHand = ec.rHand;
        }
    }

    // Update is called once per frame
    void Update() {
        //RotationUpdate();
        MovementUpdate();
    }

    void RotationUpdate() {
        //when mouse pressed
        if (Input.GetKey(lHand.bind) || Input.GetKey(rHand.bind)) {
            //look at same direction as camera
            //rot of cam
            Quaternion camRot = Camera.main.transform.rotation;
            //change player rotation
            transform.rotation = Quaternion.Euler(0, camRot.eulerAngles.y, 0);
        }
    }

    void MovementUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Q) && !isInRadialMenu)
        {
            radialMenu1.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isInRadialMenu = true;
            Time.timeScale = 0.5f;
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            radialMenu1.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            int index = radialMenu1.GetComponent<RMF_RadialMenu>().index;
            ChangeElementBasedOnIndex(false, index);
            isInRadialMenu = false;
            Time.timeScale = 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.E) && !isInRadialMenu)
        {
            radialMenu2.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isInRadialMenu = true;
            Time.timeScale = 0.5f;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            radialMenu2.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            int index = radialMenu2.GetComponent<RMF_RadialMenu>().index;
            ChangeElementBasedOnIndex(true, index);
            isInRadialMenu = false;
            Time.timeScale = 1.0f;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            cspeed = speed * 3f;
        } else
        {
            cspeed = speed;
        }

		//animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
		//animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
		if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && !isInRadialMenu && !ec.isCasting)
        {

            rotation = Mathf.Atan2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Mathf.Rad2Deg;
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotAngle + pivot.transform.eulerAngles.y, transform.eulerAngles.z);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, rotation + pivot.transform.eulerAngles.y, transform.eulerAngles.z), Time.deltaTime * 15);
            Vector3 temp = transform.forward * cspeed;
            temp.y = rb.velocity.y;
            rb.velocity = temp;
            animator.SetBool("running", true);
        } else
        {
            animator.SetBool("running", false);
        }
    }

    public void ChangeElementBasedOnIndex(bool hand, int i)
    {
        if (hand)
        {
            ChangeElement(i);
        } 
        else
        {
            ChangeElement2(i);
        }
    }

    //rightHand
    public void ChangeElement(int i)
    {
        //TODO Change Element Code Tiong ples do
        rHand.currentElement = elements[i];
    }

    //leftHand
    public void ChangeElement2(int i)
    {
        lHand.currentElement = elements[i];
    }

    /// <summary>
    /// All damage taken would come through here
    /// </summary>
    public void TakeDamage(float damage) {
        currentHealth -= damage;
        if(currentHealth <= 0) {
            //die
            print("PlayerDied");
            animator.SetTrigger("WhenDie");
        }
    }

    public void TakeDamage(float damage, Vector3 sourcePos) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Vector3 targetDir = transform.position - sourcePos;
            float angle = Vector3.SignedAngle(targetDir, -transform.forward, Vector3.up);
            animator.SetFloat("DieRection", angle);
            //die
            print("PlayerDied");
            animator.SetTrigger("WhenDie");
        }
    }
}

