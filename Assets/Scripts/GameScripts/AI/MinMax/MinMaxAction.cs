using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinMaxAction : MonoBehaviour {
    public abstract float Reward { get; }

    public abstract void Act();
    public abstract bool CheckIfDoable();
}
