using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Data provider
public interface IGoap {
    //give planner world state to check for precons
    HashSet<KeyValuePair<string, object>> GetWorldState();

    //give planner new goal
    HashSet<KeyValuePair<string, object>> CreateGoalState();

    void PlanNotFound(HashSet<KeyValuePair<string, object>> failedGoal);

    void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions);

    void ActionsFinished();

    void ActionAborted(GoapAction aborter);

    bool MoveAgentState(GoapAction nextAction);
}
