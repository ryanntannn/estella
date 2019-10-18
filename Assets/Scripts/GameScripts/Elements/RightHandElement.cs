using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandElement : Element { 
    public override void OnHit(GameObject other) {
        
    }
    public override KeyCode GetKey() {
        return KeyCode.Mouse1;
    }
}
