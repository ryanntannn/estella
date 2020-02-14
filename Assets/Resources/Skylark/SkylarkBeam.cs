using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkylarkBeam : MonoBehaviour {
    public float delay = 1.5f;
    public Vector3 playerPos;

    bool m_ready = false;
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(GetReady());
    }

    IEnumerator GetReady() {
        yield return new WaitForSeconds(delay);
        m_ready = true;

        //look at player
        transform.LookAt(playerPos);
        transform.Rotate(0, Random.Range(-30.0f, 30.0f), 0);
        //lmao
        transform.position -= transform.up * 1;
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update() {
        if (m_ready) {
            transform.position += transform.forward * Time.deltaTime * 5;
            
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Player) {
            other.GetComponent<PlayerControl>().TakeDamage(30, transform.position);
        }
    }
}
