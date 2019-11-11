using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : Enemy {
    public GameObject waterPS;

    private void Update() {
        if (fireTimeToLive > 0) {
            fireTimeToLive -= Time.deltaTime;
            if (!onFirePs.isPlaying) onFirePs.Play();
        }else {
            if (!onFirePs.isStopped) onFirePs.Stop();
        }
    }

    public override void ReactFire(Element.Types type) {
        switch (type) {
            case Element.Types.Stream:
                fireTimeToLive += Time.deltaTime * 2;   //last twice as long
                break;
            case Element.Types.Bolt:
                fireTimeToLive += 5;    //deadass just add 5
                //should only happen once
                break;
            case Element.Types.Power:
                //same thing here
                break;
        }
    }

    public override void ReactWind(Element.Types type, Transform other) {
        switch (type) {
            case Element.Types.Stream:
                //slowly tilt away from direction of the wind
                Vector3 directon = other.position - transform.position;

                break;
            case Element.Types.Bolt:
                //violently tilt away from direction wind
                
                break;
            case Element.Types.Power:
                //tilt towards tornado
                break;
        }
    }

    public override void ReactElectricity(Element.Types type) {
        switch (type) {
            case Element.Types.Stream:
                break;
            case Element.Types.Bolt:
                //should only happen once
                break;
            case Element.Types.Power:
                break;
        }
    }

    public override void ReactEarth(Element.Types type) {
        switch (type) {
            case Element.Types.Stream:
                break;
            case Element.Types.Bolt:
                //should only happen once
                break;
            case Element.Types.Power:
                break;
        }
    }

    public override void ReactWater(Element.Types type) {
        switch (type) {
            case Element.Types.Stream:
                //nothing here
                break;
            case Element.Types.Bolt:
                //apply slow and small knockback
                break;
            case Element.Types.Power:
                break;
        }
    }

   
}
