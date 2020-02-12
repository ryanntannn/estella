using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleScript : MonoBehaviour {

	public bool isElectrified = false;
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public void Electrify() {
		isElectrified = true;
		//add shader to materials
		transform.GetChild(0).GetComponent<MeshRenderer>().materials[1] = Resources.Load<Material>("Elements/Water/WaterSparks");
	}
}
