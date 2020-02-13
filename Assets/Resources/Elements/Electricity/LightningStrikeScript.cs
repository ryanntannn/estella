using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrikeScript : MonoBehaviour {
	// Start is called before the first frame update
	void Start() {
		Vector3 newPos = transform.position;
		newPos.y = 0;
		transform.position = newPos;
		StartCoroutine(KillSelf(2));

		//raycast up and do damage
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position, Vector3.up, out hitInfo, 100)) {
			transform.position = hitInfo.point;
			print(hitInfo.point);
			if (hitInfo.collider.gameObject.CompareTag("Puddle")) {
				//start shocking that shit
				hitInfo.collider.GetComponent<PuddleScript>().Electrify();
			} else
			if (hitInfo.collider.gameObject.layer == Layers.Enemy) {
				//when enemy
				hitInfo.collider.GetComponent<Enemy>().DebuffEnemy(3, Enemy.Effects.Stun);
				hitInfo.collider.GetComponent<Enemy>().TakeDamage(3);
			}
		} 
	}

	IEnumerator KillSelf(float _ttl) {
		yield return new WaitForSeconds(_ttl);
		Helper.StopParticleSystem(transform);
		Destroy(gameObject, 3);
	}

	// Update is called once per frame
	void Update() {

	}
}
