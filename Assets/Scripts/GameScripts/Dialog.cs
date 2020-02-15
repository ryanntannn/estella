using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
public class Dialog : MonoBehaviour
{
    public List<DialogLine> dialogLines;
    public InteractableObject interactableObject;
    bool isInDialog = false;
    public bool giveQuestAfter;
    public int indexQuestAfter;
    PlayerControl pc;

    // Start is called before the first frame update
    void Start()
    {
        interactableObject = GetComponent<InteractableObject>();
        pc = PlayerControl.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInDialog && interactableObject.isActivated == true)
        {
            Debug.Log("StartDialog");
            StartDialog();
            isInDialog = true;
        }
    }

    public void StartDialog()
    {
        StartCoroutine("SayDialog", 0);
        pc.enabled = false;
    }

    IEnumerator SayDialog(int index)
    {
        IngameUI.Instance.UpdateDialogText(dialogLines[index].content);
        yield return new WaitForSeconds(dialogLines[index].waitTime);
        NextDialog(index + 1);
    }

    void NextDialog(int index)
    {
        if (index == dialogLines.Count)
        {
            //Dialog is done
            if (giveQuestAfter)
            {
                LevelManager.Instance.ChangeQuest(indexQuestAfter);
            }

            pc.enabled = true;
            isInDialog = false;
            IngameUI.Instance.UpdateDialogText("");
            interactableObject.ResetAll();
        }
        else
        {
            StartCoroutine("SayDialog", index);
        }
    }
}
