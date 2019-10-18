using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Type : ScriptableObject {
    public float effectTime;    //this dictates how long the effect would last
    //effectTime = 1 would result in the effect ending as soon as trigger stops
    //effectTime = 2 would result in the effect ending after the amount of time it was triggered
    //hard to explain, just play around to find it
    public GameObject thing;

    public abstract void TypeStart(Element element);
    public abstract void TypeUpdate(Element element);
    public abstract void AlwaysUpdate(Element element);
}
