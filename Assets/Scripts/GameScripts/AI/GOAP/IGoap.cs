using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoap {
	List<KeyValuePair<string, object>> GetWorldState();
}
