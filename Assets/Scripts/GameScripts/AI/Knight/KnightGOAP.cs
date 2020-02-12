using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightGOAP : MonoBehaviour, IGoap {
    public void ActionsFinished() {

    }

    public List<KeyValuePair<string, object>> CreateGoalState() {
        List<KeyValuePair<string, object>> goalState = new List<KeyValuePair<string, object>>();
        goalState.Add(new KeyValuePair<string, object>("damagePlayer", true));
        return goalState;
    }

    public List<KeyValuePair<string, object>> GetWorldState() {
        List<KeyValuePair<string, object>> worldState = new List<KeyValuePair<string, object>>();
        worldState.Add(new KeyValuePair<string, object>("isAlive", GetComponent<Enemy>().health > 0));
        worldState.Add(new KeyValuePair<string, object>("hasYelled", false));
        worldState.Add(new KeyValuePair<string, object>("damagePlayer", false));
        return worldState;
    }

    public bool MoveAgent(GoapAction nextAction) {
        transform.position = Vector3.MoveTowards(transform.position, nextAction.target.transform.position, Time.deltaTime);
        return (gameObject.transform.position - nextAction.target.transform.position).magnitude <= 0.5f;
    }

    public void PlanAborted(GoapAction aborter) {

    }

    public void PlanFailed(List<KeyValuePair<string, object>> failedGoal) {
        Debug.Log("Plan failed");
    }

    public void PlanFound(List<KeyValuePair<string, object>> goal, Queue<GoapAction> actions) {

    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
