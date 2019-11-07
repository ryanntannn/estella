using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoScript : MonoBehaviour {
    //spin how fast
    public float speed = 12;
    public float speedVarMin = -2, speedVarMax = 2;
    public float timeToLive = 5;    //how long before it disappears

    float totalRotation = 0;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //dt
        float deltaTime = Time.deltaTime;

        //rotate
        transform.rotation = Quaternion.Euler(0, totalRotation, 0);
        totalRotation += (speed + Random.Range(speedVarMin, speedVarMax)) * deltaTime;

        //decrease timer
        timeToLive -= deltaTime;
        //check if still allowed to live
        if(timeToLive <= 0) {
            //get rid of it
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().ReactWind(Element.Types.Power, transform.GetChild(1));
        }
    }
}
