using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_obk : MonoBehaviour
{
    public GameObject slashObj;
    public Vector3 Rotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        slashObj.transform.Rotate(Rotation, Space.World);
    }
}
