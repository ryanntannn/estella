using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricNimbus : MonoBehaviour {
    public GameObject lineRenderer;

    float timeToLive = 5;
    float internalCounter = 0;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        float deltaTime = Time.deltaTime;
        timeToLive -= deltaTime;
        if (timeToLive <= 0) {
            Destroy(gameObject);
        }

        internalCounter += deltaTime;
        if(internalCounter >= 2) {
            RaycastHit[] hitInfo = Physics.CapsuleCastAll(transform.position, transform.position - transform.up * 5, 3, transform.up);
            foreach(RaycastHit hit in hitInfo) {
                if(hit.collider.gameObject.layer == Layers.Enemy) {
                    //lightning strike
                    GameObject instance = Instantiate(lineRenderer, transform.position, Quaternion.identity);
                    instance.GetComponent<LineRenderer>().SetPositions(new Vector3[] { transform.position + transform.up * 5, hit.collider.gameObject.transform.position });
                    Destroy(instance, 1);   //delte after 1 seconds
                }
            }
            internalCounter = 0;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Bolt")) {
            //transform this to a power of this.element + bolt.element
            GameObject instance = Resources.Load<GameObject>("Elements/Electricity/" + other.GetComponent<Projectile>().elementName + "Nimbus");  //load this shit up
            instance = Instantiate(instance, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        
    }
}
