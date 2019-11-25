using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : MonoBehaviour {
    //element stuff
    public enum Effects { None, Stun, Drenched, Burn, Freeze, Knockback, Slow, Magnatised }
    public ParticleSystem onFirePs;
    public float debuffTimer = 0;
    public Effects currentDebuff = Effects.None;

    //stats
    public float health = 10;
    public float speed = 12;
    public float resistanceLevel = 1;
    public float unfreezeThreshold = 2;

    protected Rigidbody rb;
    protected float currentSpeed = 12;
    protected float currentResistance = 1;
    protected float currentFreezeThreshold = 2;

    public virtual void Start() {
        rb = GetComponent<Rigidbody>();
        currentSpeed = speed;
        currentResistance = resistanceLevel;
        currentFreezeThreshold = unfreezeThreshold;
    }

    public virtual void Update() {
        if (debuffTimer <= 0) {
            currentDebuff = Effects.None;
        }

        //check debuffs
        switch (currentDebuff) {
            case Effects.None:
                currentSpeed = speed;
                currentResistance = resistanceLevel;
                break;
            case Effects.Stun:
                //cannot move
                currentSpeed = 0;
                break;
            case Effects.Drenched:
                //take more damage
                currentResistance = Mathf.Clamp(currentResistance - 2, 0, Mathf.Infinity);
                break;
            case Effects.Burn:
                if (!onFirePs.isStopped) onFirePs.Play();
                health -= Time.deltaTime * (1 / currentResistance);
                break;
            case Effects.Freeze:
                //cannot move
                currentSpeed = 0;
                break;
            case Effects.Knockback:
                break;
            case Effects.Slow:
                currentSpeed *= 0.5f;
                break;
            case Effects.Magnatised:
                //bullet magnetism
                float range = 5;
                //cast and look for projectiles
                RaycastHit[] hitInfo = Physics.CapsuleCastAll(transform.position - transform.up, transform.position + transform.up, range, transform.up, 5);
                foreach (RaycastHit hit in hitInfo) {
                    if (hit.collider.CompareTag("Bolt")) {
                        //drag bolt closer
                        Vector3 direction = transform.position - hit.transform.position;
                        hit.transform.position += direction * Time.deltaTime;
                    }
                }
                break;
            default:
                break;
        }
    }

    public virtual void DebuffEnemy(float duration, Effects effect) {
        debuffTimer = duration;
        currentDebuff = effect;
    }

    public void TakeDamage(float damage) {
        if(currentDebuff == Effects.Freeze) {
            currentFreezeThreshold -= damage;
            if(currentFreezeThreshold <= 0) {
                currentFreezeThreshold = unfreezeThreshold;
                currentDebuff = Effects.None;
            }
        } else {
            health -= damage;
        }
    }
}
