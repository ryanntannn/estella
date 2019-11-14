using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Utility class that helps trigger animation events in the parent obj
public class AnimationEvents : MonoBehaviour {
    public ParticleSystem warpPS;

    GameObject parent;
    Animator anim;
    // Start is called before the first frame update
    void Start() {
        parent = transform.parent.gameObject;
        anim = GetComponent<Animator>();
    }

    void Update() {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Zap")) {

        }
    }
    
    void Warp() {
        parent.GetComponent<ElectricityElement>().Warp();
    }

    void WarpStart() {
        anim.SetBool("isZapping", true);
        if (!warpPS.isPlaying) warpPS.Play();
    }

    void WarpEnd() {
        if (!warpPS.isStopped) warpPS.Stop();
    }
}
