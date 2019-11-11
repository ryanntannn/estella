using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPuddle : MonoBehaviour {
    public float timeToLive = 5;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        timeToLive += Time.deltaTime;
        if (timeToLive <= 0) {
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        int otherLayer = other.gameObject.layer;
        if(otherLayer == Layers.Enemy) {
            other.GetComponent<Enemy>().ReactWater(Element.Types.Power);
        }else if (other.CompareTag("Bolt")) {
            GameObject instance = Resources.Load<GameObject>("Elements/Water/" + other.GetComponent<Projectile>().elementName + "Puddle");  //load this shit up
            instance = Instantiate(instance, transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));  //instanitaite it with a random rotation
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
