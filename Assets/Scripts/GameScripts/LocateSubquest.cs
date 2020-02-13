using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LocateSubquest : SubQuest
{
    public string locationName;

    public override bool QuestCheck()
    {
        return false;
    }
}
