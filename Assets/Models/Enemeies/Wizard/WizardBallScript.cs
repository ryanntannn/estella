using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBallScript : MonoBehaviour {
    public float speed = 12;

    Enemy m_parent;
    public Enemy EnemyParent { set { m_parent = value; } }
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Player) {
            if(other.GetComponent<PlayerControl>().TakeDamage(10, transform.position)) {
                m_parent.anim.SetTrigger("WhenPlayerDie");
            }
        }else if(other.gameObject.layer == Layers.Terrain) {
            Destroy(gameObject);
        }
    }
}
