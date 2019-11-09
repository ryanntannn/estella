using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeInvisible : MonoBehaviour {
    [Range(0.0f, 1.0f)]
    public float transparency;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        foreach(SkinnedMeshRenderer mr in transform.GetChild(0).GetComponentsInChildren<SkinnedMeshRenderer>()) {
            Color temp = mr.materials[0].color;
            temp.a = transparency;
            mr.materials[0].color = temp;
        }
    }
}
