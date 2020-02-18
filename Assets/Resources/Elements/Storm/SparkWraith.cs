using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkWraith : MonoBehaviour {
    public GameObject target;
    public float speed = 5;
    public Vector3 center;

    bool m_dying = false;
    Coroutine cr;
    // Start is called before the first frame update
    void Start() {
        cr = StartCoroutine(KillSelf());
    }

    IEnumerator KillSelf() {
        yield return new WaitForSeconds(20);
        m_dying = true;
        transform.LookAt(center - Vector3.up);
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update() {
        if (!m_dying) {
            if (!target) {
                //idle
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(center - transform.position, transform.up), Time.deltaTime);
                transform.position += transform.forward * Time.deltaTime * speed;
                
            } else {
                //look at target
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position, transform.up), Time.deltaTime * 360);
                transform.position += transform.forward * Time.deltaTime * speed;
                StopCoroutine(cr);
                cr = StartCoroutine(KillSelf());
            }
        }else {
            transform.position += transform.forward * Time.deltaTime * speed * 2;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().TakeDamage(10);
            Destroy(gameObject);
        }
    }
}
