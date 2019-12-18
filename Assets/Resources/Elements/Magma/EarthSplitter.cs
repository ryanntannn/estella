using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSplitter : MonoBehaviour {
    public float riseTime = 1;
    public float timeToLive = 5;

    const float TOTAL_HEIGHT = 5;
    Vector3 direction;
    float internalCounter = 0;
    // Start is called before the first frame update
    void Start() {
        Vector3 properPosition = transform.position + transform.up * TOTAL_HEIGHT;
        direction = (properPosition - transform.position) / riseTime;
        //ray cast up and check for enemies
        RaycastHit[] hitInfo = Physics.RaycastAll(transform.position, transform.up, TOTAL_HEIGHT * 2, 1 << Layers.Enemy);
        foreach(RaycastHit hit in hitInfo) {
            hit.collider.GetComponent<Enemy>().DebuffEnemy(2, Enemy.Effects.Stun);
        }

        StartCoroutine(gameObject.KillSelf(timeToLive));
    }

    // Update is called once per frame
    void Update() {
        if (internalCounter <= riseTime) {
            transform.position += direction * Time.deltaTime;
            internalCounter += Time.deltaTime;
        }
    }
}
