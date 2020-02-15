using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anita : MonoBehaviour
{
    Dialog dialog;
    NameTag nameTag;
    int dialogIndex;
    public List<DialogLine> findAlshazar;

    // Start is called before the first frame update
    void Start()
    {
        dialog = GetComponent<Dialog>();
        nameTag = GetComponentInChildren<NameTag>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance.completedQuests.Contains(2) && dialogIndex == 0)
        {
            nameTag.gameObject.GetComponent<TextMesh>().text = "<Anita>";
            dialogIndex = 1;
            dialog.dialogLines = findAlshazar;
            dialog.giveQuestAfter = true;
            dialog.indexQuestAfter = 4;
        }
    }
}
