﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrikeScript : MonoBehaviour {
    float radius = 8;
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
            if (hitInfo.collider.gameObject.CompareTag("Puddle")) {
                //start shocking that shit
                hitInfo.collider.GetComponent<PuddleScript>().Electrify();
            } else
            if (hitInfo.collider.gameObject.CompareTag("Tornado")) {
                //sphere cast all
                Collider[] hits = Physics.OverlapSphere(transform.position, radius, 1 << Layers.Enemy);
                foreach (Collider hit in hits) {
                    hit.GetComponent<Enemy>().TakeDamage(10);
                }
                //play effect
                //kill tornado
                Destroy(hitInfo.collider.gameObject);
            } else
            if (hitInfo.collider.gameObject.CompareTag("Fissure")) {
                Collider[] hits = Physics.OverlapSphere(transform.position, radius, 1 << Layers.Enemy);
                foreach (Collider hit in hits) {
                    hit.GetComponent<Enemy>().TakeDamage(10);
                }

                GameObject effect = Instantiate(Resources.Load<GameObject>("Elements/Meteor/vfx_MeteorImpact"), hitInfo.collider.transform.position, Quaternion.identity);
                Destroy(effect, 5);
                Destroy(hitInfo.collider.gameObject);
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
