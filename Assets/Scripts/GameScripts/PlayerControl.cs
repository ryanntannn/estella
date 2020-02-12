using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public float speed;
    private float speedMult;
    public float runMultiplier = 2;
    public float maxHealth = 100;
    public float currentHealth = 100;
    [Range(0, 10.0f)]
    public float healthRegenRate = 1.0f;
    public float maxStamina = 100;
    public float currentStamina = 100;
    [Range(0, 10.0f)]
    public float staminaRegenRate = 1.0f;
    public Animator animator;
    public bool InVulnerable = false;

    public Hand rHand, lHand;

    private GameObject pivot;
    Rigidbody rb;
    float rotation = 0;
    bool dodging = false;

    //Radial Menu
    GameObject radialMenu1;
    GameObject radialMenu2;
    public bool isInRadialMenu;

    //Interactable object
    public InteractableObject currentInteractableObject = null;

    IngameUI igui;

    // Start is called before the first frame update
    void Start() {
        igui = GameObject.Find("UI (1)").GetComponent<IngameUI>();
        rb = GetComponent<Rigidbody>();
        pivot = GameObject.Find("camera-pivot");
        radialMenu1 = GameObject.Find("RadialMenu 1");
        radialMenu2 = GameObject.Find("RadialMenu 2");
        radialMenu1.SetActive(false);
        radialMenu2.SetActive(false);

        //init health
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update() {
        RotationUpdate();
        RadialMenuUpdate();
        InputUpdate();
        MovementUpdate();
        InteractableObjectUpdate();
    }

    void InteractableObjectUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            Vector3 toLookAt = hitInfo.point;
            if (hitInfo.transform.gameObject.GetComponent<InteractableObject>())
            {
                InteractableObject interactedObject
                    = hitInfo.transform.gameObject.GetComponent<InteractableObject>();
                if (!currentInteractableObject)
                {
                    igui.ShowHoldF();
                    currentInteractableObject = interactedObject;
                }
            } else
            {
                if (currentInteractableObject)
                {
                    igui.HideHoldF();
                    currentInteractableObject.CancelActivate();
                    currentInteractableObject = null;
                }
            }
        }
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
        if (!dodging) {
            Vector3 temp = rb.velocity;
            temp.x = 0;
            temp.z = 0;
            rb.velocity = temp;
        }
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);

        if (
            (yAxis != 0 || xAxis != 0)
            && !ElementControlV2.Instance.isCasting
            && !isInRadialMenu
            && !dodging
        ) 
        {
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            animator.SetBool("IsRunning", isRunning);
            animator.SetBool("IsWalking", true);
            Vector2 temp = Vector2.ClampMagnitude((new Vector2(xAxis, yAxis) * Mathf.Exp(2)), 1);

            rb.velocity = transform.forward
                * speed
                * (isRunning ? runMultiplier : 1)
                * temp.sqrMagnitude
                * (yAxis < 0 ? 0.75f : 1);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            animator.SetTrigger("WhenDive");
            dodging = true;
        }

        if (currentInteractableObject && Input.GetKey(KeyCode.F))
        {
            currentInteractableObject.IsActivating();
        }

        if(currentInteractableObject && Input.GetKeyUp(KeyCode.F))
        {
            currentInteractableObject.CancelActivate();
        }
    }

    public void StartJump() {
        rb.AddForce(transform.forward * 2 * speed, ForceMode.Impulse);
    }

    public void DoneJumping() {
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
		if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && !isInRadialMenu && !ElementControlV2.Instance.isCasting && !dodging)
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
        igui.ChangeElement(!hand, i);
    }

    //rightHand
    public void ChangeElement(int i)
    {
        //TODO Change Element Code Tiong ples do
        //rHand.currentElement = elements[i];
        //set current element to null
        ElementControlV2.Instance.ChangeRightHand(i + 1);
    }

    //leftHand
    public void ChangeElement2(int i)
    {
        // lHand.currentElement = elements[i];
        ElementControlV2.Instance.ChangeLeftHandHand(i + 1);
    }

    /// <summary>
    /// All damage taken would come through here
    /// Returns if attack killed player
    /// </summary>
    public bool TakeDamage(float damage) {
        if (!InVulnerable && !dodging) {
            currentHealth -= damage;
            if (currentHealth <= 0) {
                //die
                print("PlayerDied");
                animator.SetTrigger("WhenDie");
            }
        }

        return currentHealth <= 0;
    }

    public bool TakeDamage(float damage, Vector3 sourcePos) {
        if (!InVulnerable && !dodging) {
            currentHealth -= damage;
            if (currentHealth <= 0) {
                Vector3 targetDir = transform.position - sourcePos;
                float angle = Vector3.SignedAngle(targetDir, -transform.forward, Vector3.up);
                animator.SetFloat("DieRection", angle);
                animator.SetTrigger("WhenDie");
                //disable this component
                enabled = false;
            } else {
                animator.SetFloat("GetHitDamage", damage);
                animator.SetTrigger("WhenHit");
            }
        }
        return currentHealth <= 0;
    }
}

