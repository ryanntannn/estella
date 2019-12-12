using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudGolem : MonoBehaviour, ISteamable {
    public float timeToRise = 2;
    public float yValue;
    public bool isSteamed = false;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(transform.position.y != yValue) {
            Vector3 newPos = transform.position + transform.up * Time.deltaTime;
            newPos.y = Mathf.Clamp(newPos.y, Mathf.NegativeInfinity, yValue);
            transform.position = newPos;
        }
    }

    public void SetSteamy(bool state) {
        isSteamed = state;
    }
}
