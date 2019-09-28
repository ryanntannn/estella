using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    public float sens = 1;

    float totalMovementX = 0, totalMovementY = 0;
    // Start is called before the first frame update
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        totalMovementX = transform.rotation.x;
        totalMovementY = transform.rotation.y;
    }

    // Update is called once per frame
    void Update() {
        totalMovementX += Input.GetAxisRaw("Mouse X") * sens;
        totalMovementY -= Input.GetAxisRaw("Mouse Y") * sens;
        transform.rotation = Quaternion.Euler(totalMovementY, totalMovementX, 0);
    }
}
