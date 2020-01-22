using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//singleton
public class GoapDataProvider {
	private static GoapDataProvider m_instance;

	static GoapDataProvider instance {
		get {
			if (m_instance == null) m_instance = new GoapDataProvider();
			return m_instance;
		}
	}

	public List<KeyValuePair<string, object>> GetWorldState() {
		List<KeyValuePair<string, object>> returnKvp = new List<KeyValuePair<string, object>>();
		//returnKvp.Add(new KeyValuePair<string, object>(""))
		return returnKvp;
	}
}
