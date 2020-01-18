using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaScript : MonoBehaviour {
    public float ttl = 6;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(KillSelf());
    }

    IEnumerator KillSelf() {
        yield return new WaitForSeconds(ttl);
        DisablePS(transform);
    }

    void DisablePS(Transform _t) {
        foreach(Transform t in _t) {
            DisablePS(t);
            ParticleSystem temp = t.GetComponent<ParticleSystem>();
            if (temp != null) {
                temp.Stop();
            }
            Destroy(t.gameObject);
        }
        Destroy(_t.gameObject);
    }

    // Update is called once per frame
    void Update() {

    }
}
