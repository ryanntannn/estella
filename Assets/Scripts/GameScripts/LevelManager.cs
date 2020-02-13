using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<Quest> quests;
    public int activeQuest;
    public List<int> completedQuests;
    public PlayerControl playerControl;
    public IngameUI igui;
    public List<bool> subQuestCompleted;

    // Start is called before the first frame update
    void Start()
    {
        playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        igui = GameObject.Find("UI (1)").GetComponent<IngameUI>();
        ChangeQuest(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (activeQuest >= 0)
        {
            Debug.Log("Checking");
            if (QuestCheck()) CompleteQuest();
        }
    }

    bool QuestCheck() //Check if the active quest is completed;
    {

        for(int i = 0; i < quests[activeQuest].subQuests.Count; i++)
        {
            SubQuest sq = quests[activeQuest].subQuests[i];
            if (subQuestCompleted[i] != sq.QuestCheck())
            {
                subQuestCompleted[i] = sq.QuestCheck();
                WriteQuestString();
            }
        }

        foreach (SubQuest sq in quests[activeQuest].subQuests)
        {
            if (!sq.QuestCheck()) return false;
        }

        return true;
    }

    public void WriteQuestString()
    {
        string questString = "";
        foreach (SubQuest sq in quests[activeQuest].subQuests)
        {
            subQuestCompleted.Add(false);
            questString += sq.shortDesc + "\n";
        }

        igui.UpdateQuestString(questString);
    }

    public void ChangeQuest(int i)
    {
        if(i < 0)
        {
            activeQuest = -1;
            igui.UpdateQuestString("No Quest Active");
            subQuestCompleted = null;
            return;
        }
        
        activeQuest = i;

        WriteQuestString();
    }

    public void CompleteQuest()
    {
        igui.ShowPopUp("Quest Completed", 2.0f);
        activeQuest = -1;
        ChangeQuest(activeQuest);
    }
}
