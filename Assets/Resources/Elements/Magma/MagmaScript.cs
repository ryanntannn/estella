using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaScript : MonoBehaviour {
    //bc center from 0.5 to 5
    //goes from x = 1 to x = 10 in 0.5f seconds
    BoxCollider boxCollider;
    float dividant;

    // Start is called before the first frame update
    void Start() {
        boxCollider = GetComponent<BoxCollider>();
        dividant = 9 / 0.5f;
        gameObject.KillSelf(4);
    }

    // Update is called once per frame
    void Update() {
        float newSizeX = Mathf.Clamp(boxCollider.size.x + Time.deltaTime * dividant, 0, 10);
        float newCenterX = newSizeX / 2;
        Vector3 newCenter = boxCollider.center;
        newCenter.x = newCenterX;

        Vector3 newSize = boxCollider.size;
        newSize.x = newSizeX;

        boxCollider.size = newSize;
        boxCollider.center = newCenter;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().TakeDamage(10);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer == Layers.Enemy) {
            other.GetComponent<Enemy>().DebuffEnemy(Time.deltaTime * 2, Enemy.Effects.Burn);
        }
    }
}
