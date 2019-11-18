using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirageShadow : MonoBehaviour {
    public Shader shd;

    public float timeToLive = 5;

    // Start is called before the first frame update
    void Start() {
        transform.ChangeShader(shd);
    }

    // Update is called once per frame
    void Update() {
        timeToLive -= Time.deltaTime;
        if(timeToLive <= 0) {
            Die();
        }
    }

    public void Die() {
        Destroy(gameObject);
    }
}
