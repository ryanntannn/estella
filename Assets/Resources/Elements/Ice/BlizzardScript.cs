using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlizzardScript : MonoBehaviour {
    GameObject player;
    List<Enemy> m_enemies = new List<Enemy>();
    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FreezeAll());
    }

    // Update is called once per frame
    void Update() {
		transform.position = player.transform.position + Vector3.up * 0.01f;

        m_enemies.ForEach(x => {
            x.TakeDamage(Time.deltaTime);
            x.DebuffEnemy(Time.deltaTime, Enemy.Effects.Slow);
        });
    }

    IEnumerator FreezeAll() {
        yield return new WaitForSeconds(5);
        m_enemies.ForEach(x => {
            x.TakeDamage(5);
            x.DebuffEnemy(5, Enemy.Effects.Freeze);
        });
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            m_enemies.Add(other.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == Layers.Enemy) {
            m_enemies.Remove(other.GetComponent<Enemy>());
        }
    }
}
