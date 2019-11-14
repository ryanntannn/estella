using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Utility class that helps trigger animation events in the parent obj
public class AnimationEvents : MonoBehaviour {
    public ParticleSystem warpPS;

    GameObject parent;
    // Start is called before the first frame update
    void Start() {
        parent = transform.parent.gameObject;
    }
    
    void Warp() {
        parent.GetComponent<ElectricityElement>().Warp();
    }

    void WarpStart() {
        if (!warpPS.isPlaying) warpPS.Play();
    }

    void WarpEnd() {
        if (!warpPS.isStopped) warpPS.Stop();
    }
}
