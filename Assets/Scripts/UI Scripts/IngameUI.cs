using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    PlayerControl playerControl;
    Slider healthBar;
    // Start is called before the first frame update
    void Start()
    {
        playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        healthBar = GameObject.Find("healthbar").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = playerControl.currentHealth / playerControl.maxHealth;
    }
}
