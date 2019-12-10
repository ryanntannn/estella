using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningScript : MonoBehaviour {
    public bool isRightHand = false;
    public GameObject initalTarget;

    Transform hand;
    LineRenderer lr;
    // Start is called before the first frame update
    void Start() {
        //find position of hand
        hand = GameObject.FindGameObjectWithTag("Player").transform.FindChildWithTag(isRightHand ? "RightHand" : "LeftHand");
        //lr
        lr = GetComponent<LineRenderer>();
        transform.parent = hand;
        transform.localPosition = Vector3.zero;

    }

    // Update is called once per frame
    void Update() {
        if (!Input.GetKey(isRightHand ? KeyCode.Mouse1 : KeyCode.Mouse0)) {
           Destroy(gameObject);
        }
        lr.SetPosition(0, transform.position);

        if (initalTarget) {
            FindEnemy(initalTarget);
        }
    }

    void FindEnemy(GameObject target) {
        lr.SetPosition(1, target.transform.position);
    }
}
