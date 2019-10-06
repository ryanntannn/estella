using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandElement : Element {
    public override void onHit(GameObject other) {

    }

    public override KeyCode getKey() {
        return KeyCode.Mouse0;
    }
}
