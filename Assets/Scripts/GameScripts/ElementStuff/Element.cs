using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour {
    public abstract int ByteValue { get; }  //will work like how unity does its layers
    public abstract void DoBasicAttack();
    public abstract void DoBigAttack();
    public virtual void KeyFrameTrigger() { }
    protected bool isBig = false;
}
