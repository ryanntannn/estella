using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    BY RYAN TAN
    MANAGES THIRD PERSON CAMERA MOVEMENT
*/

public class ThirdPersonCamera : MonoBehaviour {
    //GameObject References
    private GameObject playerGO;
    private GameObject cameraGO;

    private Vector2 previousMousePos;

    [SerializeField]
    private Vector3 cameraOffset; //Offset value between the pivot and the camera
    [SerializeField]
    private float sensitivity;
    [SerializeField]
    private float followDampening;
    public float clampAngle;

    // Start is called before the first frame update
    void Start() {
        //Initilise the GameObject references
        cameraGO = transform.GetChild(0).gameObject;
        playerGO = GameObject.Find("Player");
        previousMousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update() 
    {
        PivotRotationUpdate();
    }

    private void FixedUpdate()
    {
        PositionUpdate();
    }

    void PivotRotationUpdate() {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector2 rotationDelta = new Vector2(-mouseDelta.y, mouseDelta.x) * Time.deltaTime * 10 * sensitivity;
        if (rotationDelta.x + transform.eulerAngles.x >= clampAngle && rotationDelta.x + transform.eulerAngles.x <= 360 - clampAngle) {
            rotationDelta.x = 0;
        }
        Vector3 clampedAngle = new Vector3(rotationDelta.x + transform.eulerAngles.x, rotationDelta.y + transform.eulerAngles.y, 0);
        transform.localRotation = Quaternion.Euler(clampedAngle.x, clampedAngle.y, clampedAngle.z);
    }

    void PositionUpdate() {
        transform.position = Vector3.Lerp(playerGO.transform.position, transform.position, followDampening);
        cameraGO.transform.localPosition = cameraOffset;

        //raycast back
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, (cameraGO.transform.position - transform.position), out hitInfo, cameraOffset.magnitude, 1 << Layers.Terrain)) {
            cameraGO.transform.position = hitInfo.point;
        }
    }
}
