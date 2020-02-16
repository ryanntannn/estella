using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldMan : MonoBehaviour
{
    Dialog dialog;
    int dialogIndex;
    public List<DialogLine> findAnita;
    public List<DialogLine> goToQuarryDialog;
    // Start is called before the first frame update
    void Start()
    {
        dialog = GetComponent<Dialog>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance.completedQuests.Contains(1) && dialogIndex == 0)
        {
            dialogIndex = 1;
            dialog.dialogLines = findAnita;
            dialog.giveQuestAfter = true;
            dialog.indexQuestAfter = 3;
        }

        if (LevelManager.Instance.completedQuests.Contains(5) && dialogIndex == 1)
        {
            dialogIndex = 2;
            dialog.dialogLines = goToQuarryDialog;
            dialog.giveQuestAfter = true;
            dialog.indexQuestAfter = 7;
        }
    }
}
