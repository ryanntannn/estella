using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapPlanner {
    public Queue<GoapAction> PlanActions(GameObject agent, 
        HashSet<GoapAction> actions, 
        HashSet<KeyValuePair<string, object>> worldState, 
        HashSet<KeyValuePair<string, object>> goal) {
        //reset all actions
        foreach(GoapAction action in actions) {
            action.GoapReset();
        }

        //find which actions are doable
        HashSet<GoapAction> doableActions = new HashSet<GoapAction>();
        foreach(GoapAction action in actions) {
            if (action.CheckPrecons()) {
                doableActions.Add(action);
            }
        }

        List<ActionNode> tree = new List<ActionNode>(); //tracking all nodes
        bool solutionFound = BuildTree(null, doableActions, goal, ref tree);

        return new Queue<GoapAction>();
    }

    public bool BuildTree(ActionNode parent,
        HashSet<GoapAction> doableActions,
        HashSet<KeyValuePair<string, object>> goal,
        ref List<ActionNode> tree) {

        bool foundSolution = false;
        //go through all doable actions
        foreach(GoapAction action in doableActions) {
            //simulate effects
            if (parent != null) {

            }
        }

        return foundSolution;
    }
}

//much like an astar node, but without worldpos, walkable, gridPos, all that stuff
public class ActionNode {
    public ActionNode parent;
    public float Reward = 0;
    public HashSet<KeyValuePair<string, object>> state;
    public GoapAction action;

    public ActionNode(ActionNode _parent, float _reward, HashSet<KeyValuePair<string, object>> _state, GoapAction _action) {
        parent = _parent;
        Reward = _reward;
        state = _state;
        action = _action;
    }
}
