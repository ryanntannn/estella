using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Combination", menuName = "CombinationElement")]
public class CombinationElements : ScriptableObject {
    public string CombinationName;
    public int ID;

    public AnimationClip animation;
}
