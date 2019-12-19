using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {
    public float speed = 12;
    public GameObject fireDie;
    public GameObject target;
    public float timeToLive = 5;

    // Start is called before the first frame update
    void Start() {
        //die off after awhile
        StartCoroutine(gameObject.KillSelf(timeToLive));
    }

    // Update is called once per frame
    void Update() {
        float deltaTime = Time.deltaTime;
        if (target) {
            transform.LookAt(target.transform);
        }
        transform.position += transform.forward * speed * deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            Enemy enemyRef = other.GetComponent<Enemy>();
            enemyRef.currentDebuff = Enemy.Effects.Burn;
            enemyRef.debuffTimer += 5;
            Destroy(gameObject);
        }else if(other.gameObject.layer == Layers.Terrain) {
            Destroy(gameObject);
        }
    }
}
