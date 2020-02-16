using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//im hard coding this idc
public class MoveRyanToLocation : MonoBehaviour {
    public GameObject ryan;
    public GameObject[] enemies;

    private InteractableObject m_interectionHook;
    // Start is called before the first frame update
    void Start() {
        m_interectionHook = GetComponent<InteractableObject>();
    }

    // Update is called once per frame
    void Update() {
        if (m_interectionHook.isActivated) {
            ryan.GetComponentInChildren<Animator>().SetTrigger("WhenDie");
            //spawn in enemies
            foreach(GameObject e in enemies) {
                e.SetActive(true);
            }
            Destroy(this);
        }
    }
}
