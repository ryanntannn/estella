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
        types[currentType].TypeStart(this);
    }

    public void Update() {
        types[currentType].TypeUpdate(this);
    }

    public void AlwaysUpdate() {
        types[currentType].AlwaysUpdate(this);
    }

    public void ShutDown() {
        //if (ps.isPlaying) {
        //    ps.Stop();
        //}
        enabled = false;
    }

    public abstract void OnHit(GameObject other);
    public abstract KeyCode GetKey();
}

