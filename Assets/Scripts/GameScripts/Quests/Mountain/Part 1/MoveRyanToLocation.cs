using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRyanToLocation : MonoBehaviour {
    public GameObject toMove;
    public GameObject moveTo;

    private InteractableObject m_interectionHook;
    // Start is called before the first frame update
    void Start() {
        m_interectionHook = GetComponent<InteractableObject>();
    }

    // Update is called once per frame
    void Update() {
        if (m_interectionHook.isActivated) {
            toMove.SetActive(false);
            moveTo.SetActive(true);
            Destroy(this);
        }
    }
}
