using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour {
    public int currentType;
    public Type[] types;

    public void Start() {
        //if (ps.isPlaying) {
        //    ps.Stop();  //this can be skipped if we just turn off play on awake for the particle system
        //    //but just incase you know ;)
        //}
        types[currentType].start(this);
    }

    public void Update() {
        types[currentType].update(this);
    }

    public void alwaysUpdate() {
        types[currentType].alwaysUpdate(this);
    }

    public void shutDown() {
        //if (ps.isPlaying) {
        //    ps.Stop();
        //}
        enabled = false;
    }

    public abstract void onHit(GameObject other);
    public abstract KeyCode getKey();
}

