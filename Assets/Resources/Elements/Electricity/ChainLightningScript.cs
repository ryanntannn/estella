using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningScript : MonoBehaviour {
    public Hand hand;
    GameObject initalTarget;
    public int maxTargets = 3;

    public LineRenderer lr;
    List<GameObject> targets = new List<GameObject>();
    List<Vector3> positions = new List<Vector3>();
    public LockOnTarget lockOn;

    // Start is called before the first frame update
    void Start() {
        //lr
        if(!lr) lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update() {
        initalTarget = lockOn.target;

        targets.Clear();
        positions.Clear();

        if (!Input.GetKey(hand.bind)) {
            hand.elementControl.isCasting = false;
            Destroy(gameObject);
        }
        positions.Add(Vector3.zero);

        if (initalTarget) {
            FindEnemy(initalTarget);
        } else {
            RaycastHit hitInfo;
            //raycast forward
            if (Physics.Raycast(transform.position, hand.transform.forward, out hitInfo, 5, 1 << Layers.Enemy)) {
                FindEnemy(hitInfo.collider.gameObject);
            } else {
                //make it go stright
                positions.Add(hand.transform.forward * 5);
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
