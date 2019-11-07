using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour {
    public abstract string ElementName {
        get;
    }
    public enum Types { Stream, Bolt, Power }
    public Types currentType = Types.Stream;
    [HideInInspector]
    public KeyCode button = KeyCode.Mouse0;

    public bool isRightHand = true;
    public ParticleSystem streamPS;
    public GameObject bolt, power;

    public float currentMana = 10, maxMana = 10;
    public float manaRegenRate = 1;    //per second
    public float range = 3;
    public float boltCost = 1;
    public float streamDrain = 1;
    public float powerCost = 2;

    public GameObject targetCircle;

    private void Update() {
        switch (currentType) {
            case Types.Stream:
                StreamType();
                break;
            case Types.Bolt:
                BoltType();
                break;
            case Types.Power:
                PowerType();
                break;
            default:
                break;
        }
    }

    public virtual void Enable() {  //start / onAwake
        enabled = true;
    }

    public virtual void Disable() { //shutdown hook
        enabled = false;
    }

    public abstract void StreamType();

    public abstract void BoltType();

    public abstract void PowerType();
}
