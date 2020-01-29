using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningScript : MonoBehaviour {
    public Hand hand;
    GameObject initalTarget;
    public int maxTargets = 3;

    LineRenderer lr;
    List<GameObject> targets = new List<GameObject>();
    List<Vector3> positions = new List<Vector3>();
    float initalPlaybackSpeed = 1;

    // Start is called before the first frame update
    void Start() {
        //lr
        lr = transform.GetChild(0).GetComponent<LineRenderer>();
        Animator anim = hand.transform.GetChild(0).GetComponent<Animator>();
        initalPlaybackSpeed = anim.speed;
        anim.speed = 0;
    }

    // Update is called once per frame
    void Update() {
        targets.Clear();
        positions.Clear();

        if (!Input.GetKey(hand.bind)) {
            hand.transform.GetChild(0).GetComponent<Animator>().speed = initalPlaybackSpeed;
            Destroy(gameObject);
        }
        positions.Add(Vector3.zero);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(transform.position, hand.transform.forward * 5, Color.blue);
        RaycastHit hitInfo;
        //raycast forward
        if (Physics.Raycast(transform.position, ray.direction, out hitInfo, 5, 1 << Layers.Enemy)) {
            FindEnemy(hitInfo.collider.gameObject);
        } else {
			//make it go stright at cross hair
			//positions.Add(ray.direction * 5);
			positions.Add(hand.transform.up * 5);
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
            foreach (Collider collision in hitInfo) {
                if (!targets.Contains(collision.gameObject)) {
                    FindEnemy(collision.gameObject);
                    break;
                }
            }
        }

    }
}
