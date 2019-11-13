using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireNimbus : ElectricNimbus {
    // Start is called before the first frame update
    void Start() {
        timeToLive = 10;
    }

    // Update is called once per frame
    public override void Update() {
        float deltaTime = Time.deltaTime;
        timeToLive -= deltaTime;
        if (timeToLive <= 0) {
            Destroy(gameObject);
        }

        internalCounter += deltaTime;
        if (internalCounter >= 2) {
            RaycastHit[] hitInfo = Physics.CapsuleCastAll(transform.position, transform.position - transform.up * 5, 3, transform.up);
            foreach (RaycastHit hit in hitInfo) {
                if (hit.collider.gameObject.layer == Layers.Enemy) {
                    //lightning strike
                    GameObject instance = Instantiate(lineRenderer, transform.position, Quaternion.identity);
                    instance.GetComponent<LineRenderer>().SetPositions(new Vector3[] { transform.position + transform.up * 5, hit.collider.gameObject.transform.position });
                    Destroy(instance, 1);   //delte after 1 seconds
                    //actually dealing damage
                    hit.collider.GetComponent<Enemy>().ReactElectricity(Element.Types.Bolt);
                    hit.collider.GetComponent<Enemy>().ReactFire(Element.Types.Bolt);
                }
            }
            internalCounter = 0;
        }
    }
}
