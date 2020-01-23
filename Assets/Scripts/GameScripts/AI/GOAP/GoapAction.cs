using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//parent class for all actions
public abstract class GoapAction : MonoBehaviour {
    protected List<KeyValuePair<string, object>> m_preconditions = new List<KeyValuePair<string, object>>();
    protected List<KeyValuePair<string, object>> m_effects = new List<KeyValuePair<string, object>>();
    public float cost = 1;
    public GameObject target = null;

    public abstract void DoReset();
    public abstract bool IsDone();
    public abstract bool CheckPreconditions(GameObject agent);
    public abstract bool Act(GameObject agent);
    public abstract bool NeedRange();
    public abstract bool InRange();

    public List<KeyValuePair<string, object>> Preconditions { get { return m_preconditions; } }
    public List<KeyValuePair<string, object>> Effects { get { return m_effects; } }
}
