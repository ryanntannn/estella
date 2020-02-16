using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class FissureScript : MonoBehaviour {
    public float riseTime = 2;
    public float timeToLive = 10;

    const float TOTAL_HEIGHT = 2;
    Vector3 direction;
    float internalCounter = 0;
    bool onFire = false;
    BoxCollider fireCollider;
    public Material metorShader;
    // Start is called before the first frame update
    void Start() {
        Vector3 properPosition = transform.position + transform.up * TOTAL_HEIGHT;
        direction = (properPosition - transform.position) / riseTime;
        StartCoroutine(gameObject.KillSelf(timeToLive));
        fireCollider = GetComponents<BoxCollider>().First(x => x.isTrigger);
    }

    // Update is called once per frame
    void Update() {
        if (internalCounter <= riseTime) {
            transform.position += direction * Time.deltaTime;
            internalCounter += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) {
        switch (other.tag) {
            case "BubbleShot":
                if (!onFire) {
                    //mud
                }
                Destroy(other.gameObject);
                break;
            case "FireBall":
                //flaming hot
                onFire = true;
                transform.GetChild(0).GetComponent<MeshRenderer>().material = metorShader;
                fireCollider.enabled = true;
                Destroy(other.gameObject);
                break;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer == Layers.Enemy && onFire) {
            other.GetComponent<Enemy>().DebuffEnemy(Time.deltaTime * 1.5f, Enemy.Effects.Burn);
        }
    }
}
