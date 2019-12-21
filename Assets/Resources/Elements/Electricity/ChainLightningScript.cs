using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningScript : MonoBehaviour {
    public bool isRightHand = false;
    GameObject initalTarget;
    public int maxTargets = 3;

    Transform hand;
    LineRenderer lr;
    List<GameObject> targets = new List<GameObject>();
    List<Vector3> positions = new List<Vector3>();
    public LockOnTarget lockOn;

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
        initalTarget = lockOn.target;

        targets.Clear();
        positions.Clear();

        if (!Input.GetKey(isRightHand ? KeyCode.Mouse1 : KeyCode.Mouse0)) {
            Destroy(gameObject);
        }
        positions.Add(transform.position);

        if (initalTarget) {
            FindEnemy(initalTarget);
        } else {
            RaycastHit hitInfo;
            //raycast forward
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 5, 1 << Layers.Enemy)) {
                FindEnemy(hitInfo.collider.gameObject);
            } else {
                //make it go stright
                positions.Add(transform.position + transform.forward * 5);
            }
        }

        lr.positionCount = positions.Count;
        lr.SetPositions(positions.ToArray());
    }

    void FindEnemy(GameObject target) {
        float radius = 3;
        Vector3 targetPos = target.transform.position;
        targets.Add(target);
        positions.Add(targetPos);
        target.GetComponent<Enemy>().DebuffEnemy(Time.deltaTime, Enemy.Effects.Stun);

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
