using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    public ParticleSystem onFirePs;
    public float fireTimeToLive = 0;
    public abstract void ReactFire();
}
