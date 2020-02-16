using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public List<Quest> quests;
    public int activeQuest;
    public List<int> completedQuests;
    public PlayerControl playerControl;
    public IngameUI igui;
    public List<bool> subQuestCompleted = new List<bool>();
    public List<int> subQuestAmounts = new List<int>();

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
            if (QuestCheck()) CompleteQuest();
        }
    }

    bool QuestCheck() //Check if the active quest is completed;
    {

        for(int i = 0; i < quests[activeQuest].subQuests.Count; i++)
        {
            SubQuest sq = quests[activeQuest].subQuests[i];
            if (sq.questType == SubQuestType.Collect)
            {
                if (subQuestCompleted[i] != sq.QuestCheck())
                {
                    subQuestCompleted[i] = sq.QuestCheck();
                    WriteQuestString();
                    igui.ShowPopUp("Objective Completed", 2.0f);
                }
            } else if (sq.questType == SubQuestType.Kill)
            {
                if(!subQuestCompleted[i] && (subQuestAmounts[i] == sq.amount))
                {
                    subQuestCompleted[i] = true;
                    WriteQuestString();
                    igui.ShowPopUp("Objective Completed", 2.0f);
                }
            }
        }

        foreach (bool c in subQuestCompleted)
        {
            if (!c) return false;
        }

        return true;
    }

    public void EnemyDie(string enemyName) {
        if (activeQuest >= 0)
        {
            for (int i = 0; i < quests[activeQuest].subQuests.Count; i++)
            {
                SubQuest sq = quests[activeQuest].subQuests[i];
                if (sq.questType == SubQuestType.Kill)
                {
                    if ((sq as KillSubQuest).enemyName.Equals(enemyName))
                    {
                        subQuestAmounts[i]++;
                        WriteQuestString();
                    }
                }
            }
        }
    }

    public void PlayerEnterLocation(string locationName)
    {
        if (activeQuest >= 0)
        {
            for (int i = 0; i < quests[activeQuest].subQuests.Count; i++)
            {
                SubQuest sq = quests[activeQuest].subQuests[i];
                if (sq.questType == SubQuestType.Locate)
                {
                    if ((sq as LocateSubquest).locationName.Equals(locationName) && !subQuestCompleted[i])
                    {
                        subQuestCompleted[i] = true;
                        WriteQuestString();
                        igui.ShowPopUp("Objective Completed", 2.0f);
                    }
                }
            }
        }
    }

    public void WriteQuestString()
    {
        string questString = "";
        for (int i = 0; i < quests[activeQuest].subQuests.Count; i++)
        {
            SubQuest sq = quests[activeQuest].subQuests[i];
            questString += sq.shortDesc;
            if (subQuestCompleted[i] == false)
            {
                questString += " [" + subQuestAmounts[i] + "/" + sq.amount + "]";
            } else
            {
                questString += " [Completed]";
            }
            questString += "\n";
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

        subQuestAmounts = new List<int>();
        subQuestCompleted = new List<bool>();

        activeQuest = i;

        foreach (SubQuest sq in quests[activeQuest].subQuests)
        {
            subQuestCompleted.Add(false);
            subQuestAmounts.Add(0);
        }

        WriteQuestString();
    }

    public void CompleteQuest()
    {
        igui.ShowPopUp("Quest Completed", 2.0f);
        completedQuests.Add(activeQuest);
        activeQuest = quests[activeQuest].nextQuest;
        ChangeQuest(activeQuest);
    }

    public void RestartLevel() {
        SceneManager.SetActiveScene(SceneManager.GetActiveScene());
    }
}
