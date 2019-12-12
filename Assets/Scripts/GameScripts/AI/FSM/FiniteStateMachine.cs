using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine {
    public State currentState;
    public delegate void State(GameObject agent);
}
