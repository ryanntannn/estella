using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustStormScript : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(StartSinking());
    }

    IEnumerator StartSinking() {
        yield return new WaitForSeconds(7);
        float timer = 0;
        while (timer < 3) {
            transform.position -= transform.up * Time.deltaTime * 5;
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update() {

    }
}
