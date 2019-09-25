using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHit : MonoBehaviour {
    [HideInInspector]
    public float timer = 0;
    public ParticleSystem ps;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(timer >= 0) {
            timer -= Time.deltaTime;
            //lower the timer value
            if (ps.isStopped) {
                ps.Play();//start effect
            }//ensure it only happens once
        } else {
            if (ps.isPlaying) {
                ps.Stop();//stop effect
            }
        }
        //this should make it so that when timer is >0, we play the effect, if not we don't

    }
}
