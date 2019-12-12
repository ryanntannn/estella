using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine {
    public Queue<State> fsm;
    public delegate void State(GameObject agent);
}
