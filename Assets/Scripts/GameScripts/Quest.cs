using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Quest : ScriptableObject
{
    public string shortDesc;
    public string longDesc;
    public List<SubQuest> subQuests;
    public int nextQuest;
}
