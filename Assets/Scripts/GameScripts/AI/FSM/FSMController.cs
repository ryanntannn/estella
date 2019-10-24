using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

//main agent for an ai agent
public class FSMController : MonoBehaviour {

    NavMeshAgent navAgent;

    public State currentState;  //track state
    // Start is called before the first frame update
    void Start() {
        //temp, I just wanna test shit out
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {

    }
}
