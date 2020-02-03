using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    PlayerControl playerControl;
    ElementControl elementControl;
    Slider healthBar;
    Slider sprintBar;
    Slider energyBar;
    // Start is called before the first frame update
    void Start()
    {
        playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        elementControl = playerControl.GetComponent<ElementControl>();
        healthBar = GameObject.Find("healthbar").GetComponent<Slider>();
        sprintBar = GameObject.Find("sprintbar").GetComponent<Slider>();
        energyBar = GameObject.Find("energybar").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = playerControl.currentHealth / playerControl.maxHealth;
        sprintBar.value = playerControl.currentStamina / playerControl.maxStamina;
        energyBar.value = elementControl.currentMana / elementControl.maxMana;
    }
}
