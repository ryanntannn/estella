using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTornado : MonoBehaviour {
    public float rotationSpeed = 10;
    public float minRotVar = -2, maxrotVar = 2;
    public float moveSpeed = 5;

    float timeToLive = 10;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //rotate thing
        transform.Rotate(0, (rotationSpeed + Random.Range(minRotVar, maxrotVar)) * Time.deltaTime, 0);

        //decease this
        timeToLive -= Time.deltaTime;
        if(timeToLive <= 0) {   //when time to die
            Destroy(gameObject);    //die
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.ReactFire(Element.Types.Bolt);
            enemy.ReactWind(Element.Types.Power, transform);
        }
    }
}
