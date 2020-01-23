using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapAgent : MonoBehaviour {

    FiniteStateMachine fsm = new FiniteStateMachine();
    FiniteStateMachine.State idle, walkToTarget, performAction;

    GoapAction[] actionsAvaliable;
    IGoap dataProvider;
    GoapPlanner planner = new GoapPlanner();
    Queue<GoapAction> actionQ = new Queue<GoapAction>();
    // Start is called before the first frame update
    void Start() {
        dataProvider = GetComponent<IGoap>();
        actionsAvaliable = GetComponents<GoapAction>();
        InitStates();
        fsm.currentState = idle;
    }

    void InitStates() {
        idle = (gameObject) => {
            List<KeyValuePair<string, object>> worldState = dataProvider.GetWorldState();
            List<KeyValuePair<string, object>> goal = dataProvider.CreateGoalState();

            Queue<GoapAction> plan = planner.Plan(gameObject, actionsAvaliable, worldState, goal);

            if (plan != null) {
                actionQ = plan;
                dataProvider.PlanFound(goal, plan);
                fsm.currentState = performAction;
            }else {
                dataProvider.PlanFailed(goal);
                fsm.currentState = idle;
            }
        };

        walkToTarget = (gameObject) => {
            GoapAction action = actionQ.Peek();
            if(action.NeedRange() && !action.target) {
                Debug.LogError("Action requires a target");
                fsm.currentState = idle;
                return;
            }

            //move to target
            if (dataProvider.MoveAgent(action)) {
                fsm.currentState = performAction;
            }
        };

        performAction = (gameObject) => {
            if(actionQ.Count <= 0) {
                //no actions
                fsm.currentState = idle;
                dataProvider.ActionsFinished();
                return;
            }

            GoapAction action = actionQ.Peek();
            if (action.IsDone()) {
                actionQ.Dequeue();
            }

            if(actionQ.Count > 0) {
                action = actionQ.Peek();
                bool inRange = action.NeedRange() ? action.InRange() : true;
                if (inRange) {
                    if (!action.Act(gameObject)) {
                        //didn't work
                        fsm.currentState = idle;
                        dataProvider.PlanAborted(action);
                    }
                }else {
                    //move to player
                    fsm.currentState = walkToTarget;
                }
            }else {
                //no actions left
                fsm.currentState = idle;
                dataProvider.ActionsFinished();
            }
        };
    }

    // Update is called once per frame
    void Update() {
        fsm.currentState(gameObject);
    }
}
