using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaShotgun : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        gameObject.KillSelf(5.0f);
    }

    // Update is called once per frame
    void Update() {

    }
}
