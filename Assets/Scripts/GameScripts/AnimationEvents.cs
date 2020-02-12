using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Utility class that helps trigger animation events in the parent obj
public class AnimationEvents : MonoBehaviour {
	public GameObject knife;

	GameObject parent;
	Animator anim;
	// Start is called before the first frame update
	void Start() {
		parent = transform.parent.gameObject;
		anim = GetComponent<Animator>();
    }

    #region Player animation events
    void CastRegularAttack() {
        if (!anim.GetBool("IsFlipped")) {   //right hand
            ElementControlV2.Instance.RightHand.currentElement.CastRegularAttack();
        } else {
            ElementControlV2.Instance.LeftHand.currentElement.CastRegularAttack();
        }
    }

    void CastUltimateAttack() {
        if (!anim.GetBool("IsFlipped")) {   //right hand
            ElementControlV2.Instance.RightHand.currentElement.CastUltimateAttack();
        } else {
            ElementControlV2.Instance.LeftHand.currentElement.CastUltimateAttack();
        }
    }

	void Done() {
        if (!anim.GetBool("IsFlipped")) {   //right hand
            ElementControlV2.Instance.RightHand.currentElement.DoneCasting();
        }else {
            ElementControlV2.Instance.LeftHand.currentElement.DoneCasting();
        }
    }

    void CombinationDone() {
        ElementControlV2.Instance.isCasting = false;
    }

	void Done(float _ttl) {
		StartCoroutine(DoneDelay(_ttl));
	}

	IEnumerator DoneDelay(float _ttl) {
		yield return new WaitForSeconds(_ttl);
	}
	
	void ECTrigger(string message) {
        ElementControlV2.Instance.SendMessage(message);
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
    void KnightGoIdle() {
        parent.GetComponent<KnightScript>().GoIdle();
    }
    #endregion

    #region Archer animation events
    void GetArrow() {
        parent.GetComponent<ArcherScript>().GetArrow();
    }

    void ShootArrow() {
        parent.GetComponent<ArcherScript>().ShootArrow();
    }

    void DoneShooting() {
        parent.GetComponent<ArcherScript>().DoneShooting();
    }
    #endregion

    void DieTrigger() {
        Destroy(parent.GetComponent<Enemy>());
        Destroy(parent.gameObject, 5);
    }

    void EnemyDmgTrigger(float damage) {
        if (parent.name.Equals("EnemyKnight")) {
            parent.GetComponent<Enemy>().DealDamage(damage, 1);
        }else {
            parent.GetComponent<Enemy>().DealDamage(damage);
        }
    }
}
