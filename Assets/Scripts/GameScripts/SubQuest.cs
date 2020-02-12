using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SubQuestType
{
    Locate, Kill, Collect
}

[CreateAssetMenu]
public abstract class SubQuest : ScriptableObject
{
    public string shortDesc;
    public string longDesc;
    public SubQuestType questType;
    public List<GameObject> objective;
    public int amount;
    public bool completed;

    public abstract void QuestCheck();
}
