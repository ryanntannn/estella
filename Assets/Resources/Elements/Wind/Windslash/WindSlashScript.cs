using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSlashScript : MonoBehaviour {
    public float damage = 20;

    bool readyToDie = false;
    //hashset faster than list when itterating
    HashSet<Collider> enemiesDamaged = new HashSet<Collider>();
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (!ElementControlV2.Instance.isCasting && !readyToDie) {
            readyToDie = true;
            OffPs(transform);
            Destroy(gameObject, 5);
        }

        transform.localPosition = Vector3.zero;
    }

    void OffPs(Transform _t) {
        ParticleSystem ps = _t.GetComponent<ParticleSystem>();
        if (ps) ps.Stop();
        foreach (Transform child in _t) {
            OffPs(child);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (enemiesDamaged.Contains(other)) return;
        if (other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == Layers.Enemy) {
            enemiesDamaged.Add(other);
        }
    }
}
