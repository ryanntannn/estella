using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class IngameUI : MonoBehaviour
{
    PlayerControl playerControl;
    ElementControl elementControl;
    Slider healthBar;
    Slider sprintBar;
    Slider energyBar;
    TextMeshProUGUI popupt;
    RectTransform popup;

    // Start is called before the first frame update
    void Start()
    {
        playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        elementControl = playerControl.GetComponent<ElementControl>();
        healthBar = GameObject.Find("healthbar").GetComponent<Slider>();
        sprintBar = GameObject.Find("sprintbar").GetComponent<Slider>();
        energyBar = GameObject.Find("energybar").GetComponent<Slider>();
        popupt = GameObject.Find("PopupT").GetComponent<TextMeshProUGUI>();
        popup = GameObject.Find("Popup").GetComponent<RectTransform>();
        ShowPopUp("Test", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = playerControl.currentHealth / playerControl.maxHealth;
        sprintBar.value = playerControl.currentStamina / playerControl.maxStamina;
        energyBar.value = elementControl.currentMana / elementControl.maxMana;
    }

    public void ShowPopUp(string content, float duration)
    {
        popupt.text = content;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(popup.DOAnchorPos(new Vector2(150, 200), 0.5f));
        Sequence sequence2 = DOTween.Sequence();
        sequence2.Append(popup.DOAnchorPos(new Vector2(-150, 200), 0.5f)).PrependInterval(0.5f + duration);
    }
}
