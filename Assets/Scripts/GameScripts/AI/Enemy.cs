using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : MonoBehaviour {
    //element stuff
    public ParticleSystem onFirePs;
    public float fireTimeToLive = 0;
    public float resistanceLevel = 1;

    //stats
    public float health = 10;
    public float speed = 12;
    protected Rigidbody rb;

    public virtual void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Update() {
        if(fireTimeToLive > 0) {
            if (!onFirePs.isPlaying) onFirePs.Play();
            fireTimeToLive = Mathf.Clamp(fireTimeToLive - Time.deltaTime, 0, Mathf.Infinity);
            health -= Time.deltaTime;
        }else {
            if (!onFirePs.isStopped) onFirePs.Stop();
        }

    }
  
}
