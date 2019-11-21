using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Utility class that helps trigger animation events in the parent obj
public class AnimationEvents : MonoBehaviour {
    public ParticleSystem warpPS;
    public GameObject knife;
    public Shader warpShd;

    GameObject parent;
    Animator anim;
    Shader def;
    // Start is called before the first frame update
    void Start() {
        parent = transform.parent.gameObject;
        anim = GetComponent<Animator>();
    }

    #region Player animation events
    void Warp() {

    }
    #endregion

    #region Mirage animation events
    void TakeKnifeOut() {
        knife.SetActive(true);
    }

    void ThrowKnifeAway() {
        knife.SetActive(false);
        GameObject instance = Instantiate(knife, knife.transform.position, Quaternion.Euler(-90, parent.transform.rotation.eulerAngles.y - 180, 0));
        instance.transform.localScale = Vector3.one * 5;
        instance.SetActive(true);
        //give it the script
        instance.AddComponent<MirageProjectile>();
    }

    void GetKnifeBack() {
        knife.SetActive(true);
    }

    void JumpBack() {
        parent.GetComponent<Mirage>().JumpBack();
    }
    #endregion
}
