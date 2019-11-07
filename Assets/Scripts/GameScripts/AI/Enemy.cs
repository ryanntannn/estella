using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    //element stuff
    public ParticleSystem onFirePs;
    public float fireTimeToLive = 0;

    //health stuff
    public float health = 10;

    public virtual void ReactFire(Element.Types type) {
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

    public virtual void ReactWind(Element.Types type, Transform other) {
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

    public virtual void ReactElectricity(Element.Types type) {
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

    public virtual void ReactEarth(Element.Types type) {
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

    public virtual void ReactWater(Element.Types type) {
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
}
