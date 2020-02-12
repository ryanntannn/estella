using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InteractSubquest : SubQuest
{
    public string InteractableObjectName;

    public override void QuestCheck()
    {
        if (GameObject.Find(InteractableObjectName).GetComponent<InteractableObject>().isActivated)
        {
             completed = true;
        }
    }
}
