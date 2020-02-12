using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSlashScript : MonoBehaviour {

    bool readyToDie = false;
    [HideInInspector]
    public ElementControl ec;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(!ec.isCasting && !readyToDie) {
            readyToDie = true;
            OffPs(transform);
            Destroy(gameObject, 5);
        }

        transform.localPosition = Vector3.zero;
    }

    void OffPs(Transform _t) {
        ParticleSystem ps = _t.GetComponent<ParticleSystem>();
        if (ps) ps.Stop();
        foreach(Transform child in _t) {
            OffPs(child);
        }
    }
}
