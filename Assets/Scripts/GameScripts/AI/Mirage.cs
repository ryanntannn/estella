using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//mirage boss fight
public class Mirage : MonoBehaviour {
    public float moveSpeed = 3;
    public float lengthOfDash = 6;

    GameObject player;
    bool canDash = true;
    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update() {
        if(canDash) StartCoroutine("DoSkill");
    }

    IEnumerator DoSkill() {
        //dash forward
        //pick a direction
        Vector2 dir = Random.insideUnitCircle;
        Vector3 direction = new Vector3(dir.x, 0, dir.y);
        Vector3 originPos = transform.position;
        //raycast forward to make sure nothing infront
        RaycastHit hitInfo;
        if (!Physics.Raycast(transform.position, direction, out hitInfo, lengthOfDash, Layers.Terrain)) {
            //as long as it is still smaller than dash length
            while ((transform.position - originPos).magnitude <= lengthOfDash) {
                //play dash animation
                //go in that direction
                transform.Translate(direction * moveSpeed * Time.deltaTime);
                yield return null;
            }

            canDash = false;
        }
    }
}
