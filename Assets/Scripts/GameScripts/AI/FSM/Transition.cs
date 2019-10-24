using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Transition : ScriptableObject {
    public State trueState, falseState;
    public abstract bool CheckTransition(FSMController agent);
}
