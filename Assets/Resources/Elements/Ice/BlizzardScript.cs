using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlizzardScript : MonoBehaviour {
    GameObject player;
    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FreezeAll());
    }

    // Update is called once per frame
    void Update() {
		transform.position = player.transform.position + Vector3.up * 0.01f;
    }

    IEnumerator FreezeAll() {
        yield return new WaitForSeconds(5);
        RaycastHit[] hits = Physics.BoxCastAll(transform.position + transform.up * 2.5f, Vector3.one * 2.5f, Vector3.up, Quaternion.identity, 2.5f, 1 << Layers.Enemy);
        foreach(RaycastHit hit in hits) {
            //we can do another check here, but it is redundant
            hit.collider.GetComponent<Enemy>().DebuffEnemy(5, Enemy.Effects.Freeze);
        }
		DisablePS(transform);
        Destroy(gameObject, 10);
    }

	void DisablePS(Transform _t) {
		ParticleSystem ps = _t.GetComponent<ParticleSystem>();
		if (ps) ps.Stop();
		foreach(Transform child in _t) {
			DisablePS(child);
		}
	}
}
