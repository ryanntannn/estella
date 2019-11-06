using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : Enemy {
    private void Update() {
        if (fireTimeToLive > 0) {
            fireTimeToLive -= Time.deltaTime;
            if (!onFirePs.isPlaying) onFirePs.Play();
        }else {
            if (!onFirePs.isStopped) onFirePs.Stop();
        }
    }

    public override void ReactFire() {
        fireTimeToLive += Time.deltaTime * 2;   //last twice as long
    }
}
