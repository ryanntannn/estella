using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FissureScript : MonoBehaviour {

    public float timeToLive = 5;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    public virtual void Update() {
        timeToLive -= Time.deltaTime;
        if(timeToLive <= 0) {
            Destroy(gameObject);
        }

        //should prolly replace this with animation
        if (transform.position.y <= 0) {
            transform.position += Vector3.up * Time.deltaTime * 2;
        }
        //change this please
    }

    public virtual void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Bolt")) {
            //transform this to a power of this.element + bolt.element
            GameObject instance = Resources.Load<GameObject>("Elements/Earth/" + collision.gameObject.GetComponent<Projectile>().elementName + "Fissure");  //load this shit up
            instance = Instantiate(instance, transform.position, Quaternion.identity);
            Destroy(collision.gameObject.gameObject);
            Destroy(gameObject);
        }
    }
}
