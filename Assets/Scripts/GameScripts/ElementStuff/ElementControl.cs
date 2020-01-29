using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementControl : MonoBehaviour {
    [HideInInspector]
    public Hand lHand, rHand;
    public float delay = 0.3f;
    public Animator anim;
    public bool isCasting = false;
    //targing
    public GameObject targetCircle;
    public bool showTargetCircle = true;

    PlayerControl pc;
    Coroutine lHandCr, rHandCr;

    Transform createdByPlayer;
    // Start is called before the first frame update
    void Start() {
        pc = GetComponent<PlayerControl>();
        //createdByPlayer = GameObject.Find("CreatedbyPlayer").transform;
        targetCircle.SetActive(showTargetCircle);
        rHand.waitingOnOther = false;
        lHand.waitingOnOther = false;
        rHand = pc.rHand;
        lHand = pc.lHand;
    }

    // Update is called once per frame
    void Update() {
        //make sure not in menu
        if (!pc.isInRadialMenu) {
            //check hand binds
            if (Input.GetKeyDown(lHand.bind) && !isCasting && !lHand.waitingOnOther) lHandCr = StartCoroutine(WaitDelay(lHand));
            if (Input.GetKeyDown(rHand.bind) && !isCasting && !rHand.waitingOnOther) rHandCr = StartCoroutine(WaitDelay(rHand));

            //combinations
            if(lHand.waitingOnOther && rHand.waitingOnOther) {
                StopCoroutine(rHandCr);
                StopCoroutine(lHandCr);
                lHand.waitingOnOther = false;
                rHand.waitingOnOther = false;
                isCasting = true;
                Elements.Combination(lHand.currentElement, rHand.currentElement, this);
            }
        }
        //target circle
        SetTargetCircle();
    }

    void SetTargetCircle() {
        float range = 10;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Debug.DrawRay(Camera.main.transform.position, ray.direction * range, Color.red);
        if (!Physics.Raycast(ray, out hitInfo, range, 1 << Layers.Terrain)) {
            Vector3 newPos = Camera.main.transform.position + ray.direction * range;
            Physics.Raycast(newPos, -Vector3.up, out hitInfo, 100, 1<< Layers.Terrain);
        }
        targetCircle.transform.position = hitInfo.point;
        Vector3 lookRotation = Camera.main.transform.rotation.eulerAngles;
        targetCircle.transform.rotation = Quaternion.Euler(0, lookRotation.y, 0);
    }

    //cast after delay
    IEnumerator WaitDelay(Hand hand) {
        hand.waitingOnOther = true;
        yield return new WaitForSeconds(delay);
        hand.waitingOnOther = false;
        isCasting = true;

		StartCoroutine(TurnTowards());

		//animations
		anim.SetBool("IsUsingRightHand", !hand.flipAnimation);
        anim.SetTrigger(Input.GetKey(KeyCode.LeftAlt) ? hand.currentElement.BigAttackTrigger : hand.currentElement.SmallAttackTrigger);
    }

	IEnumerator TurnTowards() {
		//look at same direction as camera
		//rot of cam
		Quaternion camRot = Camera.main.transform.rotation;
		while (isCasting) {
			//change player rotation
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, camRot.eulerAngles.y, 0), Time.deltaTime * 2.5f);
			yield return null;
		}
	}

	#region Combinations
	public void SummonGolem() {
        GameObject golem = Instantiate(Resources.Load<GameObject>("Elements/Mud/MudGolem"), targetCircle.transform.position, targetCircle.transform.rotation);
    }

    public void DoPlasma() {
        //average position of both hands
        Vector3 avg = (rHand.handPos.position + lHand.handPos.position) / 2;
        GameObject plasma = Instantiate(Resources.Load<GameObject>("Elements/Plasma/Plasma"), transform.position + transform.forward * 1.5f, transform.rotation);
    }

    public void DoSteam() {
        GameObject steampit = Instantiate(Resources.Load<GameObject>("Elements/Steam/SteamPit"), targetCircle.transform.position, Quaternion.identity);
    }

	public void DoIce() {
		GameObject blizzard = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Elements/Ice/Blizzard"), transform.position, transform.rotation);
	}

	public void DoMagma() {
		GameObject earthSplinter = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Elements/Magma/EarthShatter"), targetCircle.transform.position, targetCircle.transform.rotation);
	}

	public void DoFireTornado() {
		GameObject fireTornado = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Elements/Blaze/FireTornado"), targetCircle.transform.position, targetCircle.transform.rotation);
	}

	public void DoDust() {
		GameObject dustStorm = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Elements/Dust/DustStorm"), targetCircle.transform.position, targetCircle.transform.rotation);
	}

	public void DoMagnet() {
		GameObject blackhole = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Elements/Magnetise/Blackhole"), targetCircle.transform.position, targetCircle.transform.rotation);
	}

	public void DoStorm() {
		GameObject storm = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Elements/Storm/Storm"), targetCircle.transform.position, targetCircle.transform.rotation);
	}
	#endregion
}

public static class Elements {
    public const int
        Water = 1,
        Fire = 2,
        Earth = 4,
        Wind = 8,
        Electricity = 16;

    public static string ByteToName(int input) {
        switch (input) {
            case 1: return "Water";
            case 2: return "Fire";
            case 4: return "Earth";
            case 8: return "Wind";
            case 16: return "Electricity";
            default: return "Unknown";
        }
    }

    public static int NameToByte(string input) {
        switch (input) {
            case "Water": return Water;
            case "Fire": return Fire;
            case "Earth": return Earth;
            case "Wind": return Wind;
            case "Electricity": return Electricity;
            default: return -1;
        }
    }

    public static void Combination(Element eLHand, Element eRHand, ElementControl agent) {
        int lHandInt = NameToByte(eLHand.ElementName);
        int rHandInt = NameToByte(eRHand.ElementName);
        //switch to int and work with bits so that the order doesn't matter
        //i.e fire + water is the same as water + fire
        switch (lHandInt | rHandInt) {
            case Water | Fire:
                //debuff enemy / buff own attack
                agent.anim.SetTrigger("WhenSteamPit");
                break;
            case Water | Earth:
                //DoMud();
                agent.anim.SetTrigger("SummonGolem");
				break;
            case Water | Wind:
				//DoBlizzard();
				agent.anim.SetTrigger("WhenIce");
                break;
            case Water | Electricity:
				//TODO Shock
                //DoShock();
                break;
            case Fire | Earth:
				//DoMagma();
				agent.anim.SetTrigger("WhenMagma");
                break;
            case Fire | Wind:
				//DoBlaze();
				agent.anim.SetTrigger("WhenFireTornado");
				break;
            case Fire | Electricity:
                //DoPlasma();
                agent.anim.SetTrigger("WhenShootPlasma");
                break;
            case Earth | Wind:
				//DoDust();
				agent.anim.SetTrigger("WhenDustStorm");
                break;
            case Earth | Electricity:
				//DoMagnetize();
				agent.anim.SetTrigger("WhenMagnetise");
                break;
            case Wind | Electricity:
				//DoStorm();
				agent.anim.SetTrigger("WhenStorm");
				break;
            default:
                MonoBehaviour.print("Something wrong");
                break;
        }
    }
}
