using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//parent class for all actions
public abstract class GoapAction : MonoBehaviour {
	protected List<KeyValuePair<string, object>> m_preconditions = new List<KeyValuePair<string, object>>();

}
