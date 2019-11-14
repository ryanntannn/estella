using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPuddle : MonoBehaviour, IPower {
    public float timeToLive = 5;

    KeyAndValue<string, float> kvp = new KeyAndValue<string, float>();
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
            if (otherElement.Equals("Water")) return;    //make sure when a bolt of the same element don't trigger anything
            Combine(otherElement);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    void Combine(string otherElement) {
        GameObject instance = Resources.Load<GameObject>("Elements/Water/" + otherElement + "Puddle");  //load this shit up
        instance = Instantiate(instance, transform.position, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0));  //instanitaite it with a random rotation
    }

    public void AddValue(string element) {
        for (int count = 0; count <= kvp.Keys.Count - 1; count++) {
            if (kvp.Keys[count].Equals(element)) {
                kvp.Values[count] += Time.deltaTime;
                if (kvp.Values[count] >= 2) {
                    Combine(element);
                    Destroy(gameObject);
                }
                return;
            }
        }
        kvp.Keys.Add(element);
        kvp.Values.Add(0);
    }
}
