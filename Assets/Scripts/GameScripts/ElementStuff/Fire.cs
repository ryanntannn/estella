using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Element {
    public override int ByteValue { get { return Elements.Fire; } }    //2

    public override void DoBasicAttack() {
        isBig = false;
    }

    public override void DoBigAttack() {
        isBig = true;
    }

    public override void KeyFrameTrigger() {
        if (isBig) {

        }else {

        }
    }

    //void DoFirepit() {
    //    //fire spout
    //    GameObject firepit = Resources.Load<GameObject>("Elements/Fire/Firepit");
    //    firepit = Instantiate(firepit, targetCircle.transform.position, Quaternion.identity);
    //}

    //void Fireball() {
    //    GameObject instance = Resources.Load<GameObject>("Elements/Fire/Fireball");
    //    instance = Instantiate(instance, transform.position, transform.rotation);
    //    if (lockOn.target && enableLockOn) {
    //        instance.GetComponent<FireBall>().target = lockOn.target;
    //    } else {
    //        Vector3 newRot = Camera.main.transform.eulerAngles;
    //        newRot.x = 0;
    //        instance.transform.rotation = Quaternion.Euler(newRot);
    //    }
    //}

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
