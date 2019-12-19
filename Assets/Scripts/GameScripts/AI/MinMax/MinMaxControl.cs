using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attach this enemy ai that uses min max
public class MinMaxControl : MonoBehaviour {
    MinMaxAction[] actionsAvaliable;

    // Start is called before the first frame update
    void Start() {
        //get all actions avaliable to unit
        actionsAvaliable = GetComponents<MinMaxAction>();
    }

    // Update is called once per frame
    void Update() {
        MinMaxAction actionToDo = GetCheapest(GetDoable());
        if (actionToDo != null)
            actionToDo.Act();
    }

    List<MinMaxAction> GetDoable() {
        List<MinMaxAction> doable = new List<MinMaxAction>();
        foreach(MinMaxAction act in actionsAvaliable) {
            if (act.CheckIfDoable()) {
                doable.Add(act);
            }
        }
        return doable;
    }

    MinMaxAction GetCheapest(List<MinMaxAction> actions) {
        if(actions.Count <= 0) {
            return null;
        }

        MinMaxAction cheapest = actionsAvaliable[0];
        for(int count = 1; count <= actions.Count - 1; count++) {
            if(actions[count].Reward < cheapest.Reward) {
                cheapest = actions[count];
            }
        }
        return cheapest;
    }
}
