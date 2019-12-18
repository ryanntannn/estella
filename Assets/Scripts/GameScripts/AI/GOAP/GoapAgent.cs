using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapAgent : MonoBehaviour {
    FiniteStateMachine fsm = new FiniteStateMachine();
    IGoap dataProvider;
    GoapPlanner planner = new GoapPlanner();
    GoapAction[] actions;

    // Start is called before the first frame update
    void Start() {
        dataProvider = GetComponent<IGoap>();
        actions = GetComponents<GoapAction>();
        //planner.PlanActions(gameObject, actions, dataProvider.GetWorldState());
    }

    // Update is called once per frame
    void Update() {

    }
}
