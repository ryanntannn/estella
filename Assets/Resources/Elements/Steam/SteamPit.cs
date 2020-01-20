using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamPit : MonoBehaviour {
    public float radius = 4;
    public float ttl = 10;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(KillSelf());
    }

    IEnumerator KillSelf() {
        yield return new WaitForSeconds(ttl);
        DisablePs(transform);
        Destroy(gameObject, 5);
    }

    void DisablePs(Transform _t) {
        ParticleSystem ps = _t.GetComponent<ParticleSystem>();
        if (ps) ps.Stop();
        foreach (Transform child in _t) {
            DisablePs(child);
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerEnter(Collider other) {
        ISteamable st = other.GetComponent<ISteamable>();
        if(st != null) {
            st.SetSteamy(true);
        }
    }
}
