using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoScript : MonoBehaviour, ISteamable {
    public float timeToLive = 5;
    public bool isEmpowered = false;

    ParticleSystem[] ps;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start() {
        ps = GetComponentsInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        timeToLive -= Time.deltaTime;
        if(timeToLive <= 0) {
            foreach(ParticleSystem ps in ps) {
                if (ps.isPlaying) ps.Stop();
                ps.transform.parent = null;
                Destroy(ps.gameObject, 5);
            }
            Destroy(gameObject);
        }

        //move it forward
        rb.velocity = transform.forward * Time.deltaTime * 100;
    }

    public void SetSteamy(bool state) {
        isEmpowered = state;
        if (state) {
            timeToLive += 10;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().DebuffEnemy(5, Enemy.Effects.Slow);
        }
    }
}
