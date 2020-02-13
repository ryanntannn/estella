using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine {
    public State currentState;
    public delegate void State(GameObject agent);
}

public class FiniteStateMachineWithStack {
    private Stack<State> m_inStack = new Stack<State>();
    public void Act(GameObject agent) {
        m_inStack.Peek()(agent);
    }
    public delegate void State(GameObject agent);
    public void PopStack() { m_inStack.Pop(); }
    public void PushStack(State _item) { m_inStack.Push(_item); }
}
