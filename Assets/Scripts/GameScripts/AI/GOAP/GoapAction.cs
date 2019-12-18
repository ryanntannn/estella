using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//parent class for all actions
public abstract class GoapAction : MonoBehaviour {
    private HashSet<KeyValuePair<string, object>> m_preconditions = new HashSet<KeyValuePair<string, object>>();
    private HashSet<KeyValuePair<string, object>> m_effects = new HashSet<KeyValuePair<string, object>>();

    public HashSet<KeyValuePair<string, object>> Preconditions { get { return m_preconditions; } }
    public HashSet<KeyValuePair<string, object>> Effects { get { return m_effects; } }

    //cost of action
    public float Cost = 1;

    //reset everything
    public abstract void GoapReset();

    //check if action is done
    public abstract bool IsDone();

    //check for preconditions
    public abstract bool CheckPrecons();

    //work
    //returns whether action was carried out successfully
    public abstract bool Act();

    //add new precondition
    public void AddPrecondition(string key, object value) {
        m_preconditions.Add(new KeyValuePair<string, object>(key, value));
    }

    //remove precondition
    public bool RemovePrecondition(string key) {
        foreach(KeyValuePair<string, object> kvp in m_preconditions) {
            if (kvp.Key.Equals(key)) {
                m_preconditions.Remove(kvp);
                return true;
            }
        }
        return false;
    }

    //add new effect
    public void AddEffect(string key, object value) {
        m_effects.Add(new KeyValuePair<string, object>(key, value));
    }

    //remove effect
    public bool RemoveEffect(string key) {
        foreach(KeyValuePair<string, object> kvp in m_effects) {
            if (kvp.Key.Equals(key)) {
                m_effects.Remove(kvp);
                return true;
            }
        }
        return false;
    }
}
