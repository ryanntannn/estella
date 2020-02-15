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

    #region Skeleington animation events
    void SkeleDoneAttacking() {
        parent.GetComponent<Skelington>().DoneAttacking();
    }
    #endregion

    #region Skylark animation events
    void SkylarkAnimationEvent(string _message) {
        parent.GetComponent<SkylarkBoss>().SendMessage(_message);
    }

    void SkylarkDone() {
        parent.GetComponent<SkylarkBoss>().PushIdle();
    }
    #endregion

    #region FriendlyGolem
    void GolemReady() {
        parent.GetComponent<MudGolem>().Ready();
    }
    #endregion

    void DieTrigger() {
        foreach(Component c in parent.GetComponents<MonoBehaviour>()) {
            Destroy(c);
        }
        Destroy(parent.gameObject, 5);
    }

    //doesn't need range
    void EnemyDmgNoRange(float damage) {
        parent.GetComponent<Enemy>().DealDamage(damage);
    }

    void EnemyDmgNeedRange(float damage) {
        parent.GetComponent<Enemy>().DealDamage(damage, 1);
    }

    void StartSinking(float _duration) {
        StartCoroutine(SinkDown(_duration));
    }

    IEnumerator SinkDown(float _duration) {
        float timer = 0;
        anim.playbackTime = 0;
        while (timer < _duration) {
            timer += Time.deltaTime;
            parent.transform.position -= parent.transform.up * Time.deltaTime * 0.3f;
            yield return null;
        }

        Destroy(parent);
    }
}
