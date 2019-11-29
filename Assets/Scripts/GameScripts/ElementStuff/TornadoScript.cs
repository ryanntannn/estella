using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoScript : MonoBehaviour, ISteamable {
    public float timeToLive = 5;
    public bool isEmpowered = false;

    ParticleSystem[] ps;
    // Start is called before the first frame update
    void Start() {
        ps = GetComponentsInChildren<ParticleSystem>();
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
        transform.position += transform.forward * Time.deltaTime;
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
