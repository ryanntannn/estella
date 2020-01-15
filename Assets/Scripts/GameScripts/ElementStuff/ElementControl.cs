using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementControl : MonoBehaviour {
    public Hand lHand, rHand;
    public float delay = 0.3f;
    public Animator anim;
    public bool isCasting = false;
    //targing
    [HideInInspector]
    public LockOnTarget lockOn;
    public bool enableLockOn = true;
    public GameObject targetCircle;
    public bool showTargetCircle = true;

    PlayerControl pc;
    Coroutine lHandCr, rHandCr;

    Transform createdByPlayer;
    // Start is called before the first frame update
    void Start() {
        lockOn = GetComponent<LockOnTarget>();
        pc = GetComponent<PlayerControl>();
        createdByPlayer = GameObject.Find("CreatedbyPlayer").transform;

        targetCircle.SetActive(showTargetCircle);
        rHand.waitingOnOther = false;
        lHand.waitingOnOther = false;
    }

    // Update is called once per frame
    void Update() {
        //make sure not in menu
        if (!pc.isInRadialMenu) {
            //check hand binds
            if (Input.GetKeyDown(lHand.bind) && !isCasting && !lHand.waitingOnOther) lHandCr = StartCoroutine(WaitDelay(lHand));
            if (Input.GetKeyDown(rHand.bind) && !isCasting && !rHand.waitingOnOther) rHandCr = StartCoroutine(WaitDelay(rHand));

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

        //start casting
        if (Input.GetKey(KeyCode.LeftAlt)) hand.currentElement.DoBig(this, hand);
        else hand.currentElement.DoBasic(this, hand);
    }
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
                GameObject steampit = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Elements/Steam/SteamPit"), agent.targetCircle.transform.position, Quaternion.identity);
                //steampit.transform.parent = createdByPlayer;
                agent.isCasting = false;
                break;
            case Water | Earth:
				//DoMud();
				GameObject golem = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Elements/Mud/MudGolem"), agent.targetCircle.transform.position, agent.transform.rotation);
				agent.isCasting = false;
				break;
            case Water | Wind:
				//DoBlizzard();
				GameObject blizzard = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Elements/Ice/Blizzard"), agent.transform.position, agent.transform.rotation);
				agent.isCasting = false;
                break;
            case Water | Electricity:
                //DoShock();
                break;
            case Fire | Earth:
                //DoMagma();
                break;
            case Fire | Wind:
                //DoBlaze();
                break;
            case Fire | Electricity:
                //DoPlasma();
                break;
            case Earth | Wind:
                //DoDust();
                break;
            case Earth | Electricity:
                //DoMagnetize();
                break;
            case Wind | Electricity:
                //DoStorm();
                break;
            default:
                MonoBehaviour.print("Something wrong");
                break;
        }
    }
}
