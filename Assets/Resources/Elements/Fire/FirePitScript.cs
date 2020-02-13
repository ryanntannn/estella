using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePitScript : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.CreatedObjects) {
            if (other.CompareTag("BubbleShot")) {
                GameObject steampit = Instantiate(Resources.Load<GameObject>("Elements/Steam/SteamPit"), transform.position, transform.rotation);
                Helper.StopParticleSystem(transform);
                Destroy(gameObject, 10);
                Destroy(this);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().DebuffEnemy(Time.deltaTime * 2, Enemy.Effects.Burn);
        }
    }
}
