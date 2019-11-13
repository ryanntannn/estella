using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoScript : MonoBehaviour, IPower {
    //spin how fast
    public float rotationSpeed = 12;
    public float speedVarMin = -2, speedVarMax = 2;
    public float timeToLive = 5;    //how long before it disappears

    float totalRotation = 0;

    //I tried using kvp and dictionary, but they both don't let me change the damn values so here we are
    KeyAndValue<string, float> kvp = new KeyAndValue<string, float>();
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    public virtual void Update() {
        //dt
        float deltaTime = Time.deltaTime;

        //rotate
        transform.rotation = Quaternion.Euler(0, totalRotation, 0);
        totalRotation += (rotationSpeed + UnityEngine.Random.Range(speedVarMin, speedVarMax)) * deltaTime;

        //decrease timer
        timeToLive -= deltaTime;
        //check if still allowed to live
        if(timeToLive <= 0) {
            //get rid of it
            Destroy(gameObject);
        }

    }

    public virtual void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().ReactWind(Element.Types.Power, transform.GetChild(1).position);
        }else if (other.CompareTag("Bolt")) {
            string otherElement = other.GetComponent<Projectile>().elementName;
            if (otherElement.Equals("Fire")) return;    //make sure when a bolt of the same element don't trigger anything
            Combine(otherElement);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    void Combine(string otherElement) {
        //transform this to a power of this.element + bolt.element
        GameObject instance = Resources.Load<GameObject>("Elements/Wind/" + otherElement + "Tornado");  //load this shit up
        instance = Instantiate(instance, transform.position, Quaternion.identity);
    }

    public void AddValue(string element) {
        for(int count = 0; count <= kvp.Keys.Count - 1; count++) {
            if (kvp.Keys[count].Equals(element)) {
                kvp.Values[count] += Time.deltaTime;
                if(kvp.Values[count] >= 2) {
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
