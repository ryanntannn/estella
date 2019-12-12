using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FissureScript : MonoBehaviour {
    public float riseTime = 2;
    public float combinationDelay = 2;
    public float timeToLive = 10;

    const float TOTAL_HEIGHT = 2;
    Vector3 direction;
    float internalCounter = 0;
    // Start is called before the first frame update
    void Start() { 
        Vector3 properPosition = transform.position + transform.up * TOTAL_HEIGHT;
        direction = (properPosition - transform.position) / riseTime;
        StartCoroutine(gameObject.KillSelf(timeToLive));
    }

    // Update is called once per frame
    void Update() {
        if (internalCounter <= riseTime) {
            transform.position += direction * Time.deltaTime;
            internalCounter += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("FireBall")) {
            StartCoroutine(CreateSplinter());
            Destroy(gameObject.GetComponent<BoxCollider>());
            Destroy(transform.GetChild(0).gameObject);
            Destroy(other.gameObject);
        }
    }

    IEnumerator CreateSplinter() {
        yield return new WaitForSeconds(combinationDelay);
        GameObject magma = Resources.Load<GameObject>("Elements/Magma/EarthSplinter");
        magma = Instantiate(magma, transform.position - transform.up * 5, transform.rotation);
        Destroy(gameObject);
    }
}
