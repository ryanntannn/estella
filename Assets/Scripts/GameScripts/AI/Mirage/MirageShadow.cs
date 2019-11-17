using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirageShadow : MonoBehaviour {
    public Shader shd;

    // Start is called before the first frame update
    void Start() {
        transform.ChangeShader(shd);
    }

    // Update is called once per frame
    void Update() {

    }

    public void Die() {

    }
}
