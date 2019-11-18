using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeInvisible : MonoBehaviour {
    [Range(0.0f, 1.0f)]
    public float transparency;

    // Start is called before the first frame update
    void Start() {
        UpdateTransparency();
    }

    // Update is called once per frame
    void Update() {
    }

    public void UpdateTransparency() {
        //updates transparency
       foreach(SkinnedMeshRenderer mr in transform.GetChild(0).GetComponentsInChildren<SkinnedMeshRenderer>()) {    //loop thru all smr in estella model
            Renderer[] rend = mr.GetComponents<Renderer>(); //get components
            foreach(Renderer red in rend) { //each renderer
                foreach(Material mat in red.materials) {    //each materials
                    mat.SetFloat("Vector1_EA580445", transparency); //set the transparency
                }
            }
        }
    }
}
