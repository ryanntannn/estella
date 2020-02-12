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
    TextMeshProUGUI bigpopupt;
    TextMeshProUGUI questText;
    GameObject leftele;
    GameObject rightele;
    GameObject holdF;
    Image holdFImage;
    int activeLele = 0;
    int activeRele = 0;
    bool interactingWithSomething = false;

    // Start is called before the first frame update
    void Start()
    {
        playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        elementControl = playerControl.GetComponent<ElementControl>();
        healthBar = GameObject.Find("healthbar").GetComponent<Slider>();
        sprintBar = GameObject.Find("sprintbar").GetComponent<Slider>();
        energyBar = GameObject.Find("energybar").GetComponent<Slider>();
        popupt = GameObject.Find("PopupT").GetComponent<TextMeshProUGUI>();
        bigpopupt = GameObject.Find("Bigpopup").GetComponent<TextMeshProUGUI>();
        questText = GameObject.Find("QuestText").GetComponent<TextMeshProUGUI>();
        popup = GameObject.Find("Popup").GetComponent<RectTransform>();
        leftele = GameObject.Find("lefteleholder");
        rightele = GameObject.Find("righteleholder");
        holdF = GameObject.Find("Hold F");
        holdFImage = GameObject.Find("Hold F Fill").GetComponent<Image>();
        holdF.SetActive(false);
        ShowPopUp("Test", 1f);
        ShowBigPopUp("Test", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = playerControl.currentHealth / playerControl.maxHealth;
        sprintBar.value = playerControl.currentStamina / playerControl.maxStamina;
        energyBar.value = elementControl.currentMana / elementControl.maxMana;
        if (holdF.activeSelf)
        {
            if (!playerControl.currentInteractableObject.isActivated)
            {
                holdFImage.fillAmount = playerControl.currentInteractableObject.timeToActivate - playerControl.currentInteractableObject.timeLeftToActivate /
                    playerControl.currentInteractableObject.timeToActivate;
            }
            else
            {
                HideHoldF();
            }
        }
    }

    void DrawInteractableObject()
    {
        if(playerControl.currentInteractableObject == !interactingWithSomething)
        {
            interactingWithSomething = !interactingWithSomething;
            //TODO INTERACTION POPUP;
        }
    }

    public void ShowHoldF()
    {
        holdF.SetActive(true);
    }

    public void HideHoldF()
    {
        holdF.SetActive(false);
    }

    public void ShowPopUp(string content, float duration)
    {
        popupt.text = content;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(popup.DOAnchorPos(new Vector2(150, 200), 0.5f));
        Sequence sequence2 = DOTween.Sequence();
        sequence2.Append(popup.DOAnchorPos(new Vector2(-150, 200), 0.5f)).PrependInterval(0.5f + duration);
    }

    public void ShowBigPopUp(string content, float duration)
    {
        bigpopupt.text = content;
        RectTransform bigpopup = bigpopupt.gameObject.GetComponent<RectTransform>();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(bigpopup.DOScaleX(1, 0.2f)).Append(bigpopup.DOScaleY(1, 0.2f));
        Sequence sequence2 = DOTween.Sequence();
        sequence2.Append(bigpopup.DOScaleX(0, 0.2f)).Append(bigpopup.DOScaleY(0, 0.2f)).PrependInterval(0.2f + duration);
    }

    public void ChangeElement (bool lefthand, int index)
    {
        if (lefthand)
        {
            leftele.transform.GetChild(activeLele).gameObject.SetActive(false);
            leftele.transform.GetChild(index).gameObject.SetActive(true);
            leftele.transform.GetChild(index).DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f)
                , 0.5f, 4, 0.2f);
            activeLele = index;
        }
        else
        {
            rightele.transform.GetChild(activeRele).gameObject.SetActive(false);
            rightele.transform.GetChild(index).gameObject.SetActive(true);
            rightele.transform.GetChild(index).DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f)
                , 0.5f, 4, 0.2f);
            activeRele = index;
        }
    }

    public void UpdateQuestString(string str)
    {
        questText.text = str;
    }
}
