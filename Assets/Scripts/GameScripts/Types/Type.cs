using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Type : ScriptableObject {
    public abstract void start(Element element);
    public abstract void update(Element element);
    public abstract void alwaysUpdate(Element element);
}
