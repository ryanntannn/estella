using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : ScriptableObject {
    public abstract string ElementName { get; }
    public abstract string SmallAttackTrigger { get; }
    public abstract string BigAttackTrigger { get; }

    public abstract void DoBasic(ElementControl agent, Hand hand);
    public abstract void DoBig(ElementControl agent, Hand hand);
    public virtual void KeyFrameTrigger() { }
}
