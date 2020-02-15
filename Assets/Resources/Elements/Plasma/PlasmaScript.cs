using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaScript : MonoBehaviour {
    public float ttl = 6;
	//1.8 until 5.4
	bool doingDmg = false;
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(KillSelf());
		StartCoroutine(CanDoDmg());
    }
	IEnumerator CanDoDmg() {
		yield return new WaitForSeconds(1.8f);
		doingDmg = true;
		yield return new WaitForSeconds(3.6f);
		doingDmg = false;
	}

    IEnumerator KillSelf() {
        yield return new WaitForSeconds(ttl);
        Helper.StopParticleSystem(transform);
        // Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update() {
		if (doingDmg) {
			//raycast out and do dmg
			RaycastHit[] hits = Physics.SphereCastAll(transform.position, 1.5f, transform.forward, 18);
			foreach(RaycastHit hitInfo in hits) {
				if(hitInfo.collider.gameObject.layer == Layers.Enemy) {
					hitInfo.collider.GetComponent<Enemy>().TakeDamage(100);
				}
			}
		}
    }
}
