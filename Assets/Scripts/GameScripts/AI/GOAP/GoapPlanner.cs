using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoapPlanner{
	public Queue<GoapAction> Plan(GameObject agent, GoapAction[] avaliableActions, List<KeyValuePair<string, object>> worldState, List<KeyValuePair<string, object>> goal) {
        //reset actions
        foreach(GoapAction action in avaliableActions) {
            action.DoReset();
        }

        List<GoapAction> usableActions = new List<GoapAction>();
        foreach (GoapAction action in avaliableActions) {
            if (action.CheckPreconditions(agent)) {
                usableActions.Add(action);
            }
        }

        List<Node> leaves = new List<Node>();

        Node start = new Node(null, 0, worldState, null);
        bool success = BuildTree(start, leaves, usableActions, goal);

        if (!success) {
            //no plan
            Debug.Log("Cannot find plan");
            return null;
        }

        //find cheapest leaf
        Node cheapest = leaves[0];
        for(int count = 1; count <= leaves.Count - 1; count++) {
            if(cheapest.cost > leaves[count].cost) {
                cheapest = leaves[count];
            }
        }

        List<GoapAction> result = new List<GoapAction>();
        Node n = cheapest;
        while(n != null) {
            if(n.action) result.Add(n.action);
            n = n.parent;
        }
        result.Reverse();

        Queue<GoapAction> returnQ = new Queue<GoapAction>();
        foreach(GoapAction a in result) {
            returnQ.Enqueue(a);
        }

        return returnQ;
    }

    bool BuildTree(Node parent, List<Node> leaves, List<GoapAction> usableActions, List<KeyValuePair<string, object>> goal) {
        bool foundPath = false;

        foreach(GoapAction action in usableActions) {
            //check if parent state has correct world state for action preconditions
            if(HasCorrectWorldState(parent.state, action.Preconditions)) {
                //simulate effects
                List<KeyValuePair<string, object>> currentWorldState = PopulateState(parent, action);
                Node node = new Node(parent, parent.cost + action.cost, currentWorldState, action);

                if(HasCorrectWorldState(currentWorldState, goal)) {
                    leaves.Add(node);
                    foundPath = true;
                }else {
                    List<GoapAction> actionsLeft = RemoveFromList(usableActions, action);
                    bool found = BuildTree(node, leaves, actionsLeft, goal);
                    if (found) foundPath = true;
                }
            }
        }

        return foundPath;
    }

    List<GoapAction> RemoveFromList(List<GoapAction> list, GoapAction remove) {
        List<GoapAction> returnList = new List<GoapAction>();
        foreach(GoapAction a in list) {
            if (!a.Equals(remove)) {
                returnList.Add(a);
            }
        }
        return returnList;
    }

    //check if action can be carried out
    bool HasCorrectWorldState(List<KeyValuePair<string, object>> parent, List<KeyValuePair<string, object>> child) {
        bool allMatch = true;
        foreach(KeyValuePair<string, object> precondition in child) {
            bool matchFound = false;
            //foreach precondition, check if it is met
            foreach (KeyValuePair<string, object> worldState in parent) {
                if (precondition.Equals(worldState)) {
                    //precondition found
                    matchFound = true;
                    break;
                }
            }
            if (!matchFound) allMatch = false;
        }
        return allMatch;
    }

    List<KeyValuePair<string, object>> PopulateState(Node parent, GoapAction child) {
        HashSet<KeyValuePair<string, object>> newState = new HashSet<KeyValuePair<string, object>>();
        foreach(KeyValuePair<string, object> state in parent.state) {
            newState.Add(state);
        }

        //check if child.effect affects preconditions, and set it properly
        foreach(KeyValuePair<string, object> change in child.Effects) {
            bool exists = false;
            foreach(KeyValuePair<string, object> s in parent.state) {
                if(s.Equals(change)) {
                    exists = true;
                    break;
                }
            }

            if (exists) {
                newState.RemoveWhere((KeyValuePair<string, object> kvp) => { return kvp.Key.Equals(change.Key); });
            }
            newState.Add(new KeyValuePair<string, object>(change.Key, change.Value));
          
        }

        //move to list and return
        List<KeyValuePair<string, object>> returnList = new List<KeyValuePair<string, object>>();
        foreach (KeyValuePair<string, object> kvp in newState) {
            returnList.Add(kvp);
        }
        return returnList;
    }

    private class Node {
        public Node parent;
        public float cost;
        public List<KeyValuePair<string, object>> state;
        public GoapAction action;

        public Node(Node _parent, float _cost, List<KeyValuePair<string, object>> _state, GoapAction _action) {
            parent = _parent;
            cost = _cost;
            state = _state;
            action = _action;
        }
    }
}
