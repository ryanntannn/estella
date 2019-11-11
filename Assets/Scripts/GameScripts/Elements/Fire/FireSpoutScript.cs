using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpoutScript : MonoBehaviour {
    public ParticleSystem firePS;

    float timeToLive = 5;
    //thing spouting out fire
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        timeToLive -= Time.deltaTime;
        if(timeToLive <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().ReactFire(Element.Types.Power);
        } else if (other.CompareTag("Bolt")) {
            //transform this to a power of this.element + bolt.element
            GameObject instance = Resources.Load<GameObject>("Elements/Fire/" + other.GetComponent<Projectile>().elementName + "Sprout");  //load this shit up
            instance = Instantiate(instance, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
