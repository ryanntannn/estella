using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    BY RYAN TAN
    MANAGES THIRD PERSON CAMERA MOVEMENT
*/

public class ThirdPersonCamera : MonoBehaviour
{
    //GameObject References
    private GameObject playerGO;
    private GameObject cameraGO;

    private Vector3 previousMousePos;
    
    [SerializeField] private Vector3 cameraOffset; //Offset value between the pivot and the camera
    [SerializeField] private float followDampening;


    // Start is called before the first frame update
    void Start()
    {
        //Initilise the GameObject references
        cameraGO = transform.GetChild(0).gameObject;
        playerGO = GameObject.Find("Player");
        previousMousePos = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        PivotRotationUpdate();
        PositionUpdate();
    }

    void PivotRotationUpdate()
    {
        Vector3 mouseDelta = Input.mousePosition - previousMousePos; //Change in mouse position from the previous frame
        Vector2 rotationDelta = new Vector2(-mouseDelta.y, mouseDelta.x) * Time.deltaTime;
        transform.eulerAngles = new Vector3(rotationDelta.x + transform.eulerAngles.x, rotationDelta.y + transform.eulerAngles.y, 0);

        previousMousePos = Input.mousePosition;
    }

    void PositionUpdate()
    {
        transform.position = Vector3.Lerp(playerGO.transform.position, transform.position, Time.deltaTime * followDampening);
        cameraGO.transform.localPosition = cameraOffset;
    }
}
