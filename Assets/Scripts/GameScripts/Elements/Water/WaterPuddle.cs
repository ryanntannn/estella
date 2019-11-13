using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPuddle : MonoBehaviour {
    public float timeToLive = 5;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    public virtual void Update() {
        timeToLive += Time.deltaTime;
        if (timeToLive <= 0) {
            Destroy(gameObject);
        }
        
    }

    public virtual void OnTriggerEnter(Collider other) {
        int otherLayer = other.gameObject.layer;
        if(otherLayer == Layers.Enemy) {
            other.GetComponent<Enemy>().ReactWater(Element.Types.Power);
        }else if (other.CompareTag("Bolt")) {
            string otherElement = other.GetComponent<Projectile>().elementName;
            if (otherElement.Equals("Fire")) return;    //make sure when a bolt of the same element don't trigger anything
            GameObject instance = Resources.Load<GameObject>("Elements/Water/" + otherElement + "Puddle");  //load this shit up
            instance = Instantiate(instance, transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));  //instanitaite it with a random rotation
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
