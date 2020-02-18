using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour {
    //element stuff
    public enum Effects { None, Stun, Burn, Freeze, Slow }
    public ParticleSystem onFirePs;
    public float debuffTimer = 0;
    public Effects currentDebuff = Effects.None;

    //stats
    public string enemyName; // Used to track deaths in LevelManager
    public float health = 10;
    public float speed = 12;
    public float resistanceLevel = 1;
    public float unfreezeThreshold = 2;

    public bool stunable = true;
    public bool slowable = true;
    public bool freezable = true;

    public Rigidbody rb;
    public Animator anim;
    public PlayerControl player;
    [HideInInspector]
    public float currentSpeed = 12;
    [HideInInspector]
    public float currentResistance = 1;
    [HideInInspector]
    public float currentFreezeThreshold = 2;

    //pathfinding
    public MapGrid map { get; private set; }

    public void Start() {
        if(!rb) rb = GetComponent<Rigidbody>();
        if(!anim) anim = transform.GetComponentInChildren<Animator>();
        if (!player) player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        currentSpeed = speed;
        currentResistance = resistanceLevel;
        currentFreezeThreshold = unfreezeThreshold;
    }

    public void ReferenceMap(MapGrid _map) {
        map = _map;
        _map.enemies.Add(this);
    }

    public void Update() {
        float deltaTime = Time.deltaTime;

        if (debuffTimer <= 0) {
            currentDebuff = Effects.None;
        } else {
            debuffTimer = Mathf.Clamp(debuffTimer - deltaTime, 0, 100);
        }

        //check debuffs
        switch (currentDebuff) {
            case Effects.None:
                currentSpeed = speed;
                currentResistance = resistanceLevel;
                break;
            case Effects.Stun:
                if (stunable) {
                    //cannot move
                    currentSpeed = 0;
                }
                break;
            case Effects.Burn:
                TakeDamage(deltaTime * (1 / currentResistance));
                break;
            case Effects.Freeze:
                if (freezable) {
                    //cannot move
                    currentSpeed = 0;
                }
                break;
            case Effects.Slow:
                if (slowable) {
                    currentSpeed = speed * 0.7f;
                }
                break;
            default:
                break;
        }
    }

    public void DebuffEnemy(float duration, Effects effect) {
        debuffTimer = duration;
        currentDebuff = effect;
    }

    public bool TakeDamage(float damage) {
        if (currentDebuff == Effects.Freeze) {
            currentFreezeThreshold -= damage;
            if (currentFreezeThreshold <= 0) {
                currentFreezeThreshold = unfreezeThreshold;
                currentDebuff = Effects.None;
            }
        } else {
            health -= damage;
            anim.SetTrigger("WhenHit");
            //scuffed af but /shrug
            Vector3 targetDir = transform.position - ElementControlV2.Instance.transform.position;
            float angle = (Vector3.SignedAngle(targetDir, -transform.forward, Vector3.up) + 180) % 360;
            anim.SetFloat("HitY", Mathf.Sin(angle));
            anim.SetFloat("HitX", Mathf.Cos(angle));

            if (health <= 0) {
                currentSpeed = 0;
                speed = 0;

                anim.SetTrigger("WhenDie");
                LevelManager.Instance.EnemyDie(enemyName);
                foreach(MonoBehaviour m in gameObject.transform) {
                    if (m == this) continue;
                    Destroy(m);
                }

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Damage done to player should route through here
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="needRange">put 1 if need to be in range</param>
    public void DealDamage(float amount, int needRange) {
        if ((player.transform.position - transform.position).magnitude <= 1.5f || needRange != 1) {
            if (player.TakeDamage(amount, transform.position)) {
                StartCoroutine(TriggerAfterDelay(1));
            }
        }
    }

    /// <summary>
    /// Damage done to player should route through here, assuming no range constaint
    /// </summary>
    /// <param name="amount"></param>
    public void DealDamage(float amount) {
        if (player.TakeDamage(amount, transform.position)) {
            StartCoroutine(TriggerAfterDelay(1));
        }
    }

    IEnumerator TriggerAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        anim.SetTrigger("WhenPlayerDie");
    }
}
