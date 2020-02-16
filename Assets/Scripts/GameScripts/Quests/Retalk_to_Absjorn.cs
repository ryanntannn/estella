using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retalk_to_Absjorn : MonoBehaviour
{
    Dialog dialog;
    int dialogIndex;
    public List<DialogLine> talkbackD;

    // Start is called before the first frame update
    void Start()
    {
        dialog = GetComponent<Dialog>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance.completedQuests.Contains(3))
        {
            dialogIndex = 1;
            dialog.dialogLines = talkbackD;
            dialog.giveQuestAfter = true;
            dialog.indexQuestAfter = 4;

        }


    }
}
