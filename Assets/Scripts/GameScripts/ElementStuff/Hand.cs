using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attach this to main gameObject
public class Hand : MonoBehaviour {
    public string handTag;
    public Transform handPos;
    public Element currentElement;
    public KeyCode bind;
    public bool waitingOnOther = false;
    public bool flipAnimation = false;
    [HideInInspector]
    public ElementControl elementControl;

    // Start is called before the first frame update
    void Start() {
        if (!handPos) {
            handPos = transform.FindChildWithTag(handTag);
        }
        elementControl = GetComponent<ElementControl>();
    }

    // Update is called once per frame
    void Update() {

    }
}
