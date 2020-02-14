using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorrentScript : MonoBehaviour {
    public float multiplier = 1;

    // Start is called before the first frame update
    void Start() {
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().TakeDamage(10);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().TakeDamage(10 * Time.deltaTime);
            //drag to center slowly
            other.GetComponent<Enemy>().DebuffEnemy(Time.deltaTime, Enemy.Effects.Stun);
            other.GetComponent<Enemy>().currentSpeed = 0;

            Vector3 direction = transform.position - other.transform.position;
            other.transform.position += direction * Time.deltaTime * multiplier;
        }
    }
}
