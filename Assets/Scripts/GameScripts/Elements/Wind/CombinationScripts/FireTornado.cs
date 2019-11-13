using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTornado : TornadoScript {
    public float moveSpeed = 12;

    const int FREQ_OF_TRAIL = 10; //how often the trail is updated
    int currentTrailCount = 0;
    TrailRenderer tr;

    // Start is called before the first frame update
    void Start() {
        tr = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();

        //move tornado
        //idealy I want it to move a different direction but I'm stupid
        //Vector2 randomPos = Random.insideUnitCircle;
        //transform.Translate(new Vector3(randomPos.x, 0, randomPos.y) * Time.deltaTime * moveSpeed);
        //move in line
        transform.Translate(transform.forward * Time.deltaTime * moveSpeed);

        //leave trail that burns enemies
        if(++currentTrailCount >= FREQ_OF_TRAIL) {
            currentTrailCount = 0;
            //update trail
            tr.AddPosition(transform.position);
        }
    }

    public override void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().ReactWind(Element.Types.Power, transform.position);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().ReactFire(Element.Types.Stream);
        }
    }
}
