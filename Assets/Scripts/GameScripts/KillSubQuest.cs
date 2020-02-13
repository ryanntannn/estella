using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class KillSubQuest : SubQuest
{
    public string enemyName;

    public override bool QuestCheck()
    {
        return false;
    }
}
