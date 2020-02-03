using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public float speed;
    private float speedMult;
    public float maxHealth = 100;
    public float currentHealth = 100;
    public float maxStamina = 100;
    public float currentStamina = 100;
    public Animator animator;
    public Element[] elements;

    public Hand rHand, lHand;
    public KeyCode swapLeft = KeyCode.Q, swapRight = KeyCode.E; //swapping elements

    private GameObject pivot;
    Rigidbody rb;
    float rotation = 0;
    bool dodging = false;

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
        currentStamina = maxStamina;

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
        RotationUpdate();
        RadialMenuUpdate();
        InputUpdate();
        MovementUpdate();
    }

    void RadialMenuUpdate() {
        if (Input.GetKeyDown(KeyCode.Q) && !isInRadialMenu) {
            radialMenu1.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isInRadialMenu = true;
            Time.timeScale = 0.5f;
        } else if (Input.GetKeyUp(KeyCode.Q)) {
            radialMenu1.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            int index = radialMenu1.GetComponent<RMF_RadialMenu>().index;
            ChangeElementBasedOnIndex(false, index);
            isInRadialMenu = false;
            Time.timeScale = 1.0f;
        } else if (Input.GetKeyDown(KeyCode.E) && !isInRadialMenu) {
            radialMenu2.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isInRadialMenu = true;
            Time.timeScale = 0.5f;
        } else if (Input.GetKeyUp(KeyCode.E)) {
            radialMenu2.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            int index = radialMenu2.GetComponent<RMF_RadialMenu>().index;
            ChangeElementBasedOnIndex(true, index);
            isInRadialMenu = false;
            Time.timeScale = 1.0f;
        }
    }

    void InputUpdate() {
        speedMult = 0;
        if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && !isInRadialMenu && !ec.isCasting) {
            speedMult = ((Input.GetKey(KeyCode.LeftShift) && currentStamina > 0) ? 1.5f : 1);
        }
        animator.SetFloat("Speed", speedMult);
        if(speedMult > 1) {
            currentStamina -= Time.deltaTime * 2;
        } else {
            currentStamina = Mathf.Clamp(currentStamina + Time.deltaTime * (speedMult > 0.5f ? 1 : dodging ? 0 : 2), 0, maxStamina);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !dodging && currentStamina > 20) {
            currentStamina -= 20;
            dodging = true;
            animator.SetTrigger("WhenDodge");
        }
    }

    public void StartJump() {
        rb.AddForce(transform.forward * 2 * speed, ForceMode.Impulse);
    }

    public void DoneJumping() {
        Vector3 newVelocity = rb.velocity;
        newVelocity.x = 0;
        newVelocity.z = 0;
        rb.velocity = newVelocity;
        dodging = false;
    }

    void MovementUpdate() {
        if (speedMult > 0 && !dodging) {
            Vector3 temp = transform.forward * speedMult * speed;
            temp.y = rb.velocity.y;
            rb.velocity = temp;
        }
    }

    void RotationUpdate()
    {
		//animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
		//animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
		if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && !isInRadialMenu && !ec.isCasting && !dodging)
        {
            rotation = Mathf.Atan2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Mathf.Rad2Deg;
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotAngle + pivot.transform.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, rotation + pivot.transform.eulerAngles.y, transform.eulerAngles.z), Time.deltaTime * 15);
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
    /// Returns if attack killed player
    /// </summary>
    public bool TakeDamage(float damage) {
        currentHealth -= damage;
        if(currentHealth <= 0) {
            //die
            print("PlayerDied");
            animator.SetTrigger("WhenDie");
        }
        return currentHealth <= 0;
    }

    public bool TakeDamage(float damage, Vector3 sourcePos) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Vector3 targetDir = transform.position - sourcePos;
            float angle = Vector3.SignedAngle(targetDir, -transform.forward, Vector3.up);
            animator.SetFloat("DieRection", angle);
            animator.SetTrigger("WhenDie");
        }
        return currentHealth <= 0;
    }
}

