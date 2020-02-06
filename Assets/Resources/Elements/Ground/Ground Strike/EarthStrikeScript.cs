using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthStrikeScript : MonoBehaviour {
    [HideInInspector]
    public float finalYPos = 0;
    public float riseSpeed = 12;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(RiseUp());
    }

    IEnumerator RiseUp() {
        while(transform.position.y < finalYPos) {
            transform.position += transform.up * Time.deltaTime * riseSpeed;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
			other.GetComponent<Enemy>().TakeDamage(20);
		}
    }
}
