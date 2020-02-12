using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform cameraPivot;
    // Start is called before the first frame update
    void Start()
    {
        cameraPivot = GameObject.Find("camera-pivot").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.eulerAngles = new Vector3(90, cameraPivot.eulerAngles.y, 0);
    }
}
