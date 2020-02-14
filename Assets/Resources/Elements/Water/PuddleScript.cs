using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PuddleScript : MonoBehaviour {
    public float ttl = 5;

    Coroutine killing;
	public bool isElectrified = false;
	// Start is called before the first frame update
	void Start() {
        killing = StartCoroutine(KillSelf(ttl));
	}

    IEnumerator KillSelf(float _ttl) {
        yield return new WaitForSeconds(_ttl);
        Destroy(GetComponent<Collider>());
        float timer = 0;
        while (timer < 5) {
            transform.position -= transform.up * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

	// Update is called once per frame
	void Update() {

	}

	public void Electrify() {
        if (!isElectrified) {
            isElectrified = true;
            Material[] temp = new Material[2];
            temp[0] = transform.GetChild(0).GetComponent<MeshRenderer>().materials[0];
            //add shader to materials
            temp[1] = Resources.Load<Material>("Elements/Water/WaterSparks");
            transform.GetChild(0).GetComponent<MeshRenderer>().materials = temp;
            StopCoroutine(killing);
            StartCoroutine(KillSelf(10));
        }
    }

    private void OnTriggerStay(Collider other) {
        try {
            if (other.gameObject.layer == Layers.Enemy) {
                if (isElectrified) {
                    //ministun
                    other.GetComponent<Enemy>().DebuffEnemy(Time.deltaTime / 2, Enemy.Effects.Stun);
                } else {
                    other.GetComponent<Enemy>().DebuffEnemy(Time.deltaTime, Enemy.Effects.Slow);
                }
            }
        } catch (Exception) { };
    }
}
