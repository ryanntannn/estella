using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berry : MonoBehaviour
{
    InteractableObject interactableObject;

    public enum BerryType
    {
        HEALTH, STAMINA, MANA
    }

    public BerryType berryType;

    public int ammount;

    // Start is called before the first frame update
    void Start()
    {
        interactableObject = GetComponent<InteractableObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (interactableObject.isActivated)
        {
            if(berryType == BerryType.HEALTH)
            {
                PlayerControl.Instance.currentHealth += ammount;
                if(PlayerControl.Instance.currentHealth > PlayerControl.Instance.maxHealth)
                {
                    PlayerControl.Instance.currentHealth = PlayerControl.Instance.maxHealth;
                }
            }
            else if (berryType == BerryType.STAMINA)
            {
                PlayerControl.Instance.currentStamina += ammount;
                if (PlayerControl.Instance.currentStamina > PlayerControl.Instance.maxStamina)
                {
                    PlayerControl.Instance.currentStamina = PlayerControl.Instance.maxStamina;
                }
            }
            else if (berryType == BerryType.MANA)
            {
                ElementControlV2.Instance.currentMana += ammount;
                if (ElementControlV2.Instance.currentMana > ElementControlV2.Instance.maxMana)
                {
                    ElementControlV2.Instance.currentMana = ElementControlV2.Instance.maxMana;
                }
            }
            Destroy(gameObject);
        }
    }
}
