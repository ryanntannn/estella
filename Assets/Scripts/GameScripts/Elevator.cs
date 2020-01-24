using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {
    [Tooltip("How fast it ascends/decends")]
    public float speed = 12;
    [Tooltip("How much to acend/decend")]
    public float height = 10;
    [Tooltip("Button to activate elevator")]
    public KeyCode bind = KeyCode.F;

    float startHeight, endHeight;
    bool isUp = true;
    bool isMoving = false;
    // Start is called before the first frame update
    void Start() {
        startHeight = transform.position.y;
        endHeight = startHeight + height;
    }

    // Update is called once per frame
    void Update() {
        if (isMoving) {
            Vector3 newPos = transform.position;
            newPos.y = Mathf.Clamp(newPos.y + Time.deltaTime * (isUp ? -1 : 1) * speed, startHeight, endHeight);
            transform.position = newPos;

            //check if we reach alr
            if (isUp) {
                isMoving = newPos.y != startHeight;
            }else {
                isMoving = newPos.y != endHeight;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Player) {
            other.transform.parent = transform;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == Layers.Player) {
            other.transform.parent = null;
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.layer == Layers.Player) {
            if (Input.GetKeyDown(bind) && !isMoving) {
                isMoving = true;
                isUp = !isUp;
            }
        }
    }
}
