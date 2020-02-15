using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudPitScript : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(GoDown());
    }

    IEnumerator GoDown() {
        yield return new WaitForSeconds(10);
        float timer = 0;
        while(timer < 7) {
            transform.position -= transform.up * Time.deltaTime * 2;
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().DebuffEnemy(Time.deltaTime, Enemy.Effects.Slow);
        }
    }
}
