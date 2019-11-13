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

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Bolt")) {
            string otherElement = other.GetComponent<Projectile>().elementName;
            if (otherElement.Equals("Fire")) return;    //make sure when a bolt of the same element don't trigger anything
            //transform this to a power of this.element + bolt.element
            GameObject instance = Resources.Load<GameObject>("Elements/Earth/" + otherElement + "Fissure");  //load this shit up
            instance = Instantiate(instance, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
