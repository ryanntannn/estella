using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlizzardScript : MonoBehaviour {
    public float lerpValue = 1;

    GameObject player;
    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FreezeAll());
    }

    // Update is called once per frame
    void Update() {
        Vector3 targetDestination = player.transform.position;
        targetDestination.y = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, targetDestination, Time.deltaTime * lerpValue);
    }

    IEnumerator FreezeAll() {
        yield return new WaitForSeconds(5);
        RaycastHit[] hits = Physics.BoxCastAll(transform.position + transform.up * 2.5f, Vector3.one * 2.5f, Vector3.up, Quaternion.identity, 2.5f, 1 << Layers.Enemy);
        foreach(RaycastHit hit in hits) {
            //we can do another check here, but it is redundant
            hit.collider.GetComponent<Enemy>().DebuffEnemy(5, Enemy.Effects.Freeze);
        }
        Destroy(gameObject);
    }
}
