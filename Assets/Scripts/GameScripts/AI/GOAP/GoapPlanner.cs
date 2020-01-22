using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapPlanner : MonoBehaviour{

	//avavliable actions
	GoapAction[] avaliableActions;
	//data provider
	IGoap goap;

	private void Start() {
		avaliableActions = GetComponents<GoapAction>();
	}

	private void Update() {
		
	}
}
