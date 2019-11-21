using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : MonoBehaviour {
    //element stuff
    public ParticleSystem onFirePs;
    float fireTimeToLive = 0;
    public float resistanceLevel = 1;

    //stats
    public float health = 10;
    public float speed = 12;
    protected Rigidbody rb;
    protected float slowTimer = 0;
    protected float currentSpeed = 12;

    public virtual void Start() {
        rb = GetComponent<Rigidbody>();
        currentSpeed = speed;
    }

    public virtual void Update() {
        if(fireTimeToLive > 0) {
            if (!onFirePs.isPlaying) onFirePs.Play();
            fireTimeToLive = Mathf.Clamp(fireTimeToLive - Time.deltaTime, 0, Mathf.Infinity);
            health -= Time.deltaTime;
        }else {
            if (!onFirePs.isStopped) onFirePs.Stop();
        }

        if(slowTimer > 0) {
            slowTimer = Mathf.Clamp(slowTimer - Time.deltaTime, 0, 20);
            currentSpeed = speed * 0.5f;    //TO BE CHANGED
        } else {
            currentSpeed = speed;
        }
    }

    public virtual void SlowEnemy(float duration) {
        slowTimer += duration;
    }
  
    public virtual void SetOnFire(float duration) {
        fireTimeToLive += duration;
    }
}
