using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShot : MonoBehaviour {
    public float speed = 12;
    public float timeToLive = 10;
    public GameObject waterDie;

    public GameObject target;
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(gameObject.KillSelf(timeToLive));
    }

    // Update is called once per frame
    void Update() {
        if (target) {
            transform.LookAt(target.transform.position);
        }
        transform.position += transform.forward * speed * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            //deal damage
            other.GetComponent<Enemy>().TakeDamage(5);
            //slow enemy
            other.GetComponent<Enemy>().DebuffEnemy(5, Enemy.Effects.Slow);
            //then go make some puddle

            //then die
            GameObject instance = Instantiate(waterDie, transform.position, transform.rotation);
            Destroy(instance, 1);
            Destroy(gameObject);
        } else if(other.gameObject.layer == Layers.Terrain) {
            GameObject instance = Instantiate(waterDie, transform.position, transform.rotation);
            Destroy(instance, 1);
            Destroy(gameObject);
        }
    }
}
