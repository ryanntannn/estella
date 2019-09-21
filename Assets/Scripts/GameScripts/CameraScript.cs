using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    Quaternion originalRot;
    Vector2 mouseLook;
    Vector2 smoothingVector;
    // Start is called before the first frame update
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;

        originalRot = transform.localRotation;
        mouseLook = Vector2.zero;
        smoothingVector = Vector2.zero;
    }

    // Update is called once per frame
    void Update() {
        float sens = 2f;
        float smoothing = 5f;
        Vector2 xy = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        xy = Vector2.Scale(xy, new Vector2(sens * smoothing, sens * smoothing));
        smoothingVector.x = Mathf.Lerp(smoothingVector.x, xy.x, 1f / smoothing);
        smoothingVector.y = Mathf.Lerp(smoothingVector.y, xy.y, 1f / smoothing);
        mouseLook += smoothingVector;

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right) * Quaternion.AngleAxis(mouseLook.x, Vector3.up);

    }
}
