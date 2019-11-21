using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShot : MonoBehaviour {
    public float speed = 12;
    public GameObject waterDie;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            //slow enemies
            other.GetComponent<Enemy>().SlowEnemy(5);
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
