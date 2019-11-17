using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirageShadow : MonoBehaviour {
    public Shader shd;

    // Start is called before the first frame update
    void Start() {
        ChangeShader(transform);
    }

    // Update is called once per frame
    void Update() {

    }

    public void Die() {

    }

    void ChangeShader(Transform t) {
        //got to each child
        foreach (Transform child in t) {
            Renderer rend = child.GetComponent<Renderer>();
            if (rend) {
                foreach (Material mat in rend.materials) {
                    mat.shader = shd;
                }
                //change all shaders in child
            }
            //change shaders of child
            ChangeShader(child);
        }
    }
}
