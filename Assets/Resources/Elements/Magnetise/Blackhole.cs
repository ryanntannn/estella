using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour {
    public float timeToLive = 5;
    public float multiplier = 1;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(timeToLive > 0) {
            timeToLive -= Time.deltaTime;
        }else {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            //drag to center slowly
            //other.GetComponent<Enemy>().DebuffEnemy(Time.deltaTime, Enemy.Effects.Magnatised);
            Vector3 direction = transform.position - other.transform.position;
            other.transform.position += direction * Time.deltaTime * multiplier;
        }
    }
}
