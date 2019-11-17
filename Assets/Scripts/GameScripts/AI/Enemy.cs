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

    public virtual void ReactFire(Element.Types type) {
        switch (type) {
            case Element.Types.Stream:
                fireTimeToLive += Time.deltaTime * 2 * (1 / resistanceLevel);
                health -= Time.deltaTime * (1 / resistanceLevel);
                break;
            case Element.Types.Bolt:
                //should only happen once
                break;
            case Element.Types.Power:
                break;
        }
    }

    public virtual void ReactWind(Element.Types type, Vector3 other) {
        Vector3 dir = (other - transform.position).normalized;
        switch (type) {
            case Element.Types.Stream:
                //slowly pushed back
                rb.velocity -= dir * (1 / resistanceLevel);
                break;
            case Element.Types.Bolt:
                //should only happen once
                //knock back violently
                rb.AddForce(dir * (1 / resistanceLevel), ForceMode.Impulse);
                break;
            case Element.Types.Power:
                //slowly get sucked in
                rb.velocity += dir * (1 / resistanceLevel);
                break;
        }
    }

    public virtual void ReactElectricity(Element.Types type) {
        switch (type) {
            case Element.Types.Stream:
                break;
            case Element.Types.Bolt:
                //should only happen once
                break;
            case Element.Types.Power:
                break;
        }
    }

    public virtual void ReactEarth(Element.Types type) {
        switch (type) {
            case Element.Types.Stream:
                break;
            case Element.Types.Bolt:
                //should only happen once
                break;
            case Element.Types.Power:
                break;
        }
    }

    public virtual void ReactWater(Element.Types type) {
        switch (type) {
            case Element.Types.Stream:
                break;
            case Element.Types.Bolt:
                //should only happen once
                break;
            case Element.Types.Power:
                break;
        }
    }
}
