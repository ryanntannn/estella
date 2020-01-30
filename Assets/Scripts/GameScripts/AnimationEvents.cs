using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Utility class that helps trigger animation events in the parent obj
public class AnimationEvents : MonoBehaviour {
	public GameObject knife;

	GameObject parent;
	Animator anim;
	Shader def;
	//player stuff
	Hand lHand, rHand;
	ElementControl ec;
	// Start is called before the first frame update
	void Start() {
		parent = transform.parent.gameObject;
		anim = GetComponent<Animator>();
		if (parent.tag.Equals("Player")) {
			ec = parent.GetComponent<ElementControl>();
			lHand = ec.lHand;
			rHand = ec.rHand;
        }
    }

	#region Player animation events
	void DoBig() {
		if (anim.GetBool("IsUsingRightHand")) {
			rHand.currentElement.DoBig(ec, rHand);
		} else {
			lHand.currentElement.DoBig(ec, lHand);
		}
	}

	void SmallAttack() {
		if (anim.GetBool("IsUsingRightHand")) {
			rHand.currentElement.DoBasic(ec, rHand);
		} else {
            lHand.currentElement.DoBasic(ec, lHand);
		}
	}

	void Done() {
		ec.isCasting = false;
	}

    void ECTrigger(string message) {
        ec.SendMessage(message);
    }

	void DoneJumping() {
        parent.GetComponent<PlayerControl>().DoneJumping();
	}

    void StartJump() {
        parent.GetComponent<PlayerControl>().StartJump();
    }
	#endregion

	#region Mirage animation events
	void TakeKnifeOut() {
		knife.SetActive(true);
	}

	void ThrowKnifeAway() {
		knife.SetActive(false);
		GameObject instance = Instantiate(knife, knife.transform.position, Quaternion.Euler(-90, parent.transform.rotation.eulerAngles.y - 180, 0));
		instance.transform.localScale = Vector3.one * 5;
		instance.SetActive(true);
		//give it the script
		instance.AddComponent<MirageProjectile>();
	}

	void GetKnifeBack() {
		knife.SetActive(true);
	}

	void JumpBack() {
		parent.GetComponent<Mirage>().JumpBack();
	}

	void KnifeAttack() {

	}
    #endregion

    #region Knight animation events
    void EnemyDmgTrigger(float damage) {
        parent.GetComponent<Enemy>().DealDamage(damage);
    }

    void DieTrigger() {
        Destroy(parent.GetComponent<Enemy>());
        Destroy(parent.gameObject, 5);
    }
    #endregion
}
