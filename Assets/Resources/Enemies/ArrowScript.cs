using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {
    [HideInInspector]
    public Transform firingPoint;
    [HideInInspector]
    public Enemy dataProvider;

    Rigidbody rb;
    Transform original;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        original = transform.parent;
    }

    // Update is called once per frame
    void Update() {
        if (transform.parent == original) {
            transform.position = transform.parent.position;
            transform.LookAt(firingPoint);
        } else {
            //raycast out and find anything
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position + transform.forward * 0.7f, transform.forward, out hitInfo, 2, ~(1 << Layers.Enemy))) {
                switch (hitInfo.collider.gameObject.layer) {
                    case Layers.Terrain:
                    case Layers.Obstacles:
                        transform.position = hitInfo.point - transform.forward * 0.35f;
                        transform.parent = hitInfo.collider.transform;
                        StartCoroutine(KillSelf(5));
                        break;
                    case Layers.Player:
                        dataProvider.DealDamage(20);
                        Destroy(gameObject);
                    break;
                    default:
                    break;
                }
            }
        }

    }

    IEnumerator KillSelf(float _ttl) {
        //disable gravity and collider
        rb.velocity = Vector3.zero;
        Destroy(GetComponent<Collider>());
        yield return new WaitForSeconds(_ttl);
        //wait for awhile and let it fall off the map
        rb.useGravity = true;
        yield return new WaitForSeconds(_ttl);
        //free resources
        Destroy(gameObject);
    }
}
