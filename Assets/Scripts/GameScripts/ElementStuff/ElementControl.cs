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
    public float currentMana = 100, maxMana = 100;
    public float manaRegenRate = 1.0f;

    PlayerControl pc;
    Coroutine lHandCr, rHandCr;

    Transform createdByPlayer;
    // Start is called before the first frame update
    void Start() {
        pc = GetComponent<PlayerControl>();
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
            if (lHand.waitingOnOther && rHand.waitingOnOther) {
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

        if (!isCasting) {
            currentMana = Mathf.Clamp(currentMana + Time.deltaTime * manaRegenRate, 0, maxMana);
        }
    }

    void SetTargetCircle() {
        float range = 10;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Debug.DrawRay(Camera.main.transform.position, ray.direction * range, Color.red);
        //check if infront got anything
        if (!Physics.Raycast(ray, out hitInfo, range, 1 << Layers.Terrain)) {
            Vector3 newPos = Camera.main.transform.position + ray.direction * range;
            Physics.Raycast(newPos, -Vector3.up, out hitInfo, 100, 1 << Layers.Terrain);
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
		if ((Input.GetKey(KeyCode.LeftAlt) && currentMana >= hand.currentElement.BigAttackCost) || currentMana >= hand.currentElement.SmallAttackCost) {

			isCasting = true;

			//StartCoroutine(TurnTowards());
			Vector3 a = transform.rotation.eulerAngles;
			a.y = Camera.main.transform.rotation.eulerAngles.y;
			transform.rotation = Quaternion.Euler(a);

			//animations
			anim.SetBool("IsUsingRightHand", !hand.flipAnimation);
			anim.SetTrigger(Input.GetKey(KeyCode.LeftAlt) ? hand.currentElement.BigAttackTrigger : hand.currentElement.SmallAttackTrigger);

		}
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
        currentMana -= Elements.GolemCost;
        GameObject golem = Instantiate(Resources.Load<GameObject>("Elements/Mud/MudGolem"), targetCircle.transform.position, targetCircle.transform.rotation);
    }

    public void DoPlasma() {
        currentMana -= Elements.PlasmaCost;
        //average position of both hands
        Vector3 avg = (rHand.handPos.position + lHand.handPos.position) / 2;
        GameObject plasma = Instantiate(Resources.Load<GameObject>("Elements/Plasma/Plasma"), transform.position + transform.forward * 1.5f, transform.rotation);
    }

    public void DoSteam() {
        currentMana -= Elements.SteamCost;
        GameObject steampit = Instantiate(Resources.Load<GameObject>("Elements/Steam/SteamPit"), targetCircle.transform.position, Quaternion.identity);
    }

    public void DoIce() {
        currentMana -= Elements.IceCost;
        GameObject blizzard = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Elements/Ice/Blizzard"), transform.position, transform.rotation);
    }

    public void DoMagma() {
        currentMana -= Elements.MagmaCost;
        GameObject earthSplinter = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Elements/Magma/EarthShatter"), targetCircle.transform.position, targetCircle.transform.rotation);
    }

    public void DoFireTornado() {
        currentMana -= Elements.BlazeCost;
        GameObject fireTornado = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Elements/Blaze/FireTornado"), targetCircle.transform.position, targetCircle.transform.rotation);
    }

    public void DoDust() {
        currentMana -= Elements.DustCost;
        GameObject dustStorm = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Elements/Dust/DustStorm"), targetCircle.transform.position, targetCircle.transform.rotation);
    }

    public void DoMagnet() {
        currentMana -= Elements.MagnetiseCost;
        GameObject blackhole = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Elements/Magnetise/Blackhole"), targetCircle.transform.position, targetCircle.transform.rotation);
    }

    public void DoStorm() {
        currentMana -= Elements.StormCost;
        GameObject storm = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Elements/Storm/Storm"), targetCircle.transform.position, targetCircle.transform.rotation);
    }

    public void StartShock() {
        currentMana -= Elements.ShockCost;
        //find enemies in sphere
        Collider[] hits = Physics.OverlapSphere(targetCircle.transform.position, 3, 1 << Layers.Enemy);
        StartCoroutine(DoShock(100, hits, 2));
    }

    public IEnumerator DoShock(float totalDamage, Collider[] enemies, float totalDuration) {
        anim.speed = 0;
        
        //calculate damage to deal to each enemy
        float indvDmg = totalDamage / enemies.Length;
        //calculate downtime before going to next enemy
        float indvTime = totalDuration / enemies.Length;

        //if no enemies just teleport
        if(enemies.Length <= 0) {
            transform.position = targetCircle.transform.position + targetCircle.transform.up;
        }

        //start moving
        for(int count = 0; count <= enemies.Length - 1; count++) {
            transform.position = enemies[count].transform.position - enemies[count].transform.forward;
            enemies[count].GetComponent<Enemy>().TakeDamage(indvDmg);
            yield return new WaitForSeconds(indvTime);
        }

        anim.speed = 1;
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

    public const int
        SteamCost = 20,
        GolemCost = 20,
        IceCost = 30,
        ShockCost = 30,
        MagmaCost = 30,
        BlazeCost = 40,
        PlasmaCost = 90,
        DustCost = 40,
        MagnetiseCost = 45,
        StormCost = 45;


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
                if (agent.currentMana < SteamCost) break;
                //debuff enemy / buff own attack
                agent.anim.SetTrigger("WhenSteamPit");
                return;
            case Water | Earth:
                //DoMud();
                if (agent.currentMana < GolemCost) break;
                agent.anim.SetTrigger("SummonGolem");
                return;
            case Water | Wind:
                //DoBlizzard();
                if (agent.currentMana < IceCost) break;
                agent.anim.SetTrigger("WhenIce");
                return;
            case Water | Electricity:
                if (agent.currentMana < ShockCost) break;
                agent.anim.SetTrigger("WhenShock");
                //DoShock();
                return;
            case Fire | Earth:
                if (agent.currentMana < MagmaCost) break;
                //DoMagma();
                agent.anim.SetTrigger("WhenMagma");
                return;
            case Fire | Wind:
                if (agent.currentMana < BlazeCost) break;
                //DoBlaze();
                agent.anim.SetTrigger("WhenFireTornado");
                return;
            case Fire | Electricity:
                if (agent.currentMana < PlasmaCost) break;
                //DoPlasma();
                agent.anim.SetTrigger("WhenShootPlasma");
                return;
            case Earth | Wind:
                if (agent.currentMana < DustCost) break;
                //DoDust();
                agent.anim.SetTrigger("WhenDustStorm");
                return;
            case Earth | Electricity:
                if (agent.currentMana < MagnetiseCost) break;
                //DoMagnetize();
                agent.anim.SetTrigger("WhenMagnetise");
                return;
            case Wind | Electricity:
                if (agent.currentMana < StormCost) break;
                //DoStorm();
                agent.anim.SetTrigger("WhenStorm");
                return;
            default:
                MonoBehaviour.print("Something wrong");
                break;
        }

        agent.isCasting = false;
    }
}
