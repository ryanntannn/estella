using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//all projectile scripts inherit from this
public abstract class Projectile : MonoBehaviour {
    public abstract string elementName {
        get;
    }
    public abstract float speed {
        get;
    }
    
}
