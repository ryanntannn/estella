using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandElement : Element { 
    public override void onHit(GameObject other) {
        
    }
    public override KeyCode getKey() {
        return KeyCode.Mouse1;
    }
}
