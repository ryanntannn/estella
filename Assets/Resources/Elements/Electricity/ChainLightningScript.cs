using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningScript : MonoBehaviour {
    public bool isRightHand = false;
    public GameObject initalTarget;
    public int maxTargets = 3;

    Transform hand;
    LineRenderer lr;
    List<GameObject> targets = new List<GameObject>();
    List<Vector3> positions = new List<Vector3>();

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
        targets.Clear();
        positions.Clear();

        if (!Input.GetKey(isRightHand ? KeyCode.Mouse1 : KeyCode.Mouse0)) {
            Destroy(gameObject);
        }
        positions.Add(transform.position);

        if (initalTarget) {
            FindEnemy(initalTarget);
        } else {
            //make it go stright
            positions.Add(transform.forward * 3);
        }

        lr.positionCount = positions.Count;
        lr.SetPositions(positions.ToArray());
    }

    void FindEnemy(GameObject target) {
        float radius = 3;
        Vector3 targetPos = target.transform.position;
        targets.Add(target);
        positions.Add(targetPos);
        if (targets.Count <= maxTargets) {
            //find more targets
            Collider[] hitInfo = Physics.OverlapSphere(targetPos, radius, 1 << Layers.Enemy);
            foreach(Collider collision in hitInfo) {
                if (!targets.Contains(collision.gameObject)) {
                    FindEnemy(collision.gameObject);
                    break;
                }
            }
        }

    }
}
