using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementControl : MonoBehaviour {
    int lHand = 1, rHand = 2;
    [Tooltip("Displays current element")]
    public string lHandCurrent, rHandCurrent;
    public float delay = 0.3f;
    public bool enableLockOn = true;
    public GameObject targetCircle;
    public bool showTargetCircle = true;
    public Animator anim;

    PlayerControl pc;

    KeyCode rightHand = KeyCode.Mouse0, leftHand = KeyCode.Mouse1;

    Transform parent;

    bool doneAlr = false;
    LockOnTarget lockOn;
    //zap zap
    bool isFlash = false;

    //fire

    // Start is called before the first frame update
    void Start() {
        lockOn = GetComponent<LockOnTarget>();
        pc = GetComponent<PlayerControl>();
        parent = GameObject.Find("CreatedbyPlayer").transform;

        lHand = PlayerPrefs.GetInt("lHand", 1);
        rHand = PlayerPrefs.GetInt("rHand", 2);
        rightHand = pc.rightHandButton;
        leftHand = pc.leftHandButton;
        targetCircle.SetActive(showTargetCircle);
    }

    // Update is called once per frame
    void Update() {
        lHandCurrent = Elements.ByteToName(lHand);
        rHandCurrent = Elements.ByteToName(rHand);

        rHand = PlayerPrefs.GetInt("rHand");
        lHand = PlayerPrefs.GetInt("lHand");

        if (!pc.isInRadialMenu) {
            if (Input.GetKeyDown(rightHand)) {
                StartCoroutine(Input.GetKey(KeyCode.LeftAlt) ? DoBigBoy(lHand) : NoCombination(lHand));
            }
            if (Input.GetKeyDown(leftHand)) {
                StartCoroutine(Input.GetKey(KeyCode.LeftAlt) ? DoBigBoy(rHand) : NoCombination(rHand));
            }

            if (Input.GetKey(rightHand) && Input.GetKey(leftHand) && !doneAlr) {
                StopAllCoroutines();
                doneAlr = true;
                Combination();
            }

            if (Input.GetKeyUp(rightHand) || Input.GetKeyUp(leftHand)) {
                doneAlr = false;
            }
        }

        if (isFlash) {
            StartCoroutine(DoFlash());
        } else {
            StopCoroutine(DoFlash());
        }

        SetTargetCircle();
    }

    void SetTargetCircle() {
        float range = 10;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Debug.DrawRay(Camera.main.transform.position, ray.direction * range, Color.red);
        if (!Physics.Raycast(ray, out hitInfo, range, Layers.Terrain)) {
            Vector3 newPos = Camera.main.transform.position + ray.direction * range;
            Physics.Raycast(newPos, -Vector3.up, out hitInfo, 100, Layers.Terrain);
        }
        targetCircle.transform.position = hitInfo.point;
        Vector3 lookRotation = Camera.main.transform.rotation.eulerAngles;
        targetCircle.transform.rotation = Quaternion.Euler(0, lookRotation.y, 0);
    }

    void Combination() {
        switch (lHand | rHand) {
            case Elements.Water | Elements.Fire:
                DoSteam();
                break;
            case Elements.Water | Elements.Earth:
                DoMud();
                break;
            case Elements.Water | Elements.Wind:
                DoBlizzard();
                break;
            case Elements.Water | Elements.Electricity:
                DoShock();
                break;
            case Elements.Fire | Elements.Earth:
                DoMagma();
                break;
            case Elements.Fire | Elements.Wind:
                DoBlaze();
                break;
            case Elements.Fire | Elements.Electricity:
                DoPlasma();
                break;
            case Elements.Earth | Elements.Wind:
                DoDust();
                break;
            case Elements.Earth | Elements.Electricity:
                DoMagnetize();
                break;
            case Elements.Wind | Elements.Electricity:
                DoStorm();
                break;
            default:
                print("Something wrong");
                break;
        }
    }

    IEnumerator DoBigBoy(int input) {
        yield return new WaitForSeconds(delay);
        switch (input) {
            case Elements.Wind:
                DoTornado();
                break;
            case Elements.Water:
                DoTsunami();
                break;
            case Elements.Fire:
                DoFirepit();
                break;
            case Elements.Earth:
                DoGroundBreaker();
                break;
            case Elements.Electricity:
                DoFlash();
                break;
        }
    }

    IEnumerator NoCombination(int input) {
        yield return new WaitForSeconds(delay);
        switch (input) {
            case Elements.Water:
                BubbleShot();
                break;
            case Elements.Fire:
                anim.SetTrigger("WhenShootFireBall");
                break;
            case Elements.Earth:
                Fissure();
                break;
            case Elements.Wind:
                WindSlash();
                break;
            case Elements.Electricity:
                ShockChain();
                break;
            default:
                print("Something wrong");
                break;
        }
    }

    #region Combination stuff
    void DoSteam() {
        //debuff enemy / buff own attack
        GameObject steampit = Resources.Load<GameObject>("Elements/Steam/SteamPit");
        steampit = Instantiate(steampit, targetCircle.transform.position, Quaternion.identity);
        steampit.transform.parent = parent;
    }

    void DoMud() {
        //summon golem
        GameObject golem = Resources.Load<GameObject>("Elements/Mud/MudGolem");
        golem = Instantiate(golem, targetCircle.transform.position - Vector3.up * 3, Quaternion.identity);
        golem.GetComponent<MudGolem>().yValue = targetCircle.transform.position.y;
        golem.transform.parent = parent;
    }

    void DoBlizzard() {
        //summon one big cloud?
        GameObject cloud = Resources.Load<GameObject>("Elements/Ice/Blizzard");
        cloud = Instantiate(cloud, transform.position, Quaternion.identity);
        cloud.transform.parent = parent;
    }

    void DoShock() {
        GameObject shock = Resources.Load<GameObject>("Elements/Shock/Shock");
        shock = Instantiate(shock, transform.position, transform.rotation);
        shock.transform.parent = parent;
    }

    void DoMagma() {
        GameObject magma = Resources.Load<GameObject>("Elements/Magma/EarthSplinter");
        magma = Instantiate(magma, targetCircle.transform.position - targetCircle.transform.up * 5, targetCircle.transform.rotation);
        magma.transform.parent = parent;
    }

    void DoDust() {
        GameObject dust = Resources.Load<GameObject>("Elements/Dust/Dust");
        dust = Instantiate(dust, targetCircle.transform.position, Quaternion.identity);
        dust.transform.parent = parent;
    }

    void DoPlasma() {
        //laguna
        GameObject plasma = Resources.Load<GameObject>("Elements/Plasma/Plasma");
        plasma = Instantiate(plasma, targetCircle.transform.position, Quaternion.identity);
        plasma.transform.parent = parent;
    }

    void DoStorm() {
        //spark wraith
        GameObject storm = Resources.Load<GameObject>("Elements/Storm/Storm");
        storm = Instantiate(storm, targetCircle.transform.position, Quaternion.identity);
        storm.transform.parent = parent;
    }

    void DoMagnetize() {
        //play some shit
        GameObject blackhole = Resources.Load<GameObject>("Elements/Magnetise/Blackhole");
        blackhole = Instantiate(blackhole, targetCircle.transform.position, Quaternion.identity);
        blackhole.transform.parent = parent;
    }

    void DoBlaze() {
        GameObject blaze = Resources.Load<GameObject>("Elements/Blaze/Blaze");
        blaze = Instantiate(blaze, targetCircle.transform.position, Quaternion.identity);
        blaze.transform.parent = parent;
    }
    #endregion

    #region Big boy single elements
    void DoTsunami() {
        //load in tsunami behind player
        GameObject tsunami = Resources.Load<GameObject>("Elements/Water/Tsunami");
        tsunami = Instantiate(tsunami, transform.position - transform.forward - transform.up, transform.rotation);
        tsunami.transform.parent = parent;
    }

    void DoFirepit() {
        //fire spout
        GameObject firepit = Resources.Load<GameObject>("Elements/Fire/Firepit");
        firepit = Instantiate(firepit, targetCircle.transform.position, Quaternion.identity);
        firepit.transform.parent = parent;
    }

    IEnumerator DoFlash() {
        isFlash = true;
        float delay = 0.25f;
        //dash forward 
        //sof
        float range = 2.5f;
        //raycast in circle
        RaycastHit[] hitInfo = Physics.CapsuleCastAll(transform.position + transform.forward * range, transform.position + transform.forward * range, range, transform.forward, range, 1 << Layers.Enemy);
        foreach (RaycastHit hit in hitInfo) {
            Vector2 randomPos = Random.insideUnitCircle;
            transform.position = hit.transform.position + new Vector3(randomPos.x, transform.position.y, randomPos.y);
            // Vector3 dir = hit.transform.position - transform.position;
            //transform.rotation = Quaternion.LookRotation(dir, transform.up);
            yield return new WaitForSeconds(delay);
        }
        isFlash = false;
    }

    void DoTornado() {
        //instaniate tornado
        GameObject tornado = Resources.Load<GameObject>("Elements/Wind/Tornado");
        tornado = Instantiate(tornado, targetCircle.transform.position, targetCircle.transform.rotation);
        tornado.transform.parent = parent;
    }

    void DoGroundBreaker() {
        GameObject groundBreaker = Resources.Load<GameObject>("Elements/Ground/GroundBreaker");
        groundBreaker = Instantiate(groundBreaker, targetCircle.transform.position, Quaternion.identity);
        groundBreaker.transform.parent = parent;
    }
    #endregion

    #region Regular stuff
    void BubbleShot() {
        GameObject instance = Resources.Load<GameObject>("Elements/Water/BubbleShot");
        instance = Instantiate(instance, transform.position, transform.rotation);
        if (lockOn.target && enableLockOn) {
            instance.GetComponent<BubbleShot>().target = lockOn.target;
        } else {
            Vector3 newRot = Camera.main.transform.eulerAngles;
            newRot.x = 0;
            instance.transform.rotation = Quaternion.Euler(newRot);
        }
        instance.transform.parent = parent;
    }

    public void Fireball() {
        GameObject instance = Resources.Load<GameObject>("Elements/Fire/Fireball");
        instance = Instantiate(instance, transform.position, transform.rotation);
        if (lockOn.target && enableLockOn) {
            instance.GetComponent<FireBall>().target = lockOn.target;
        } else {
            Vector3 newRot = Camera.main.transform.eulerAngles;
            newRot.x = 0;
            instance.transform.rotation = Quaternion.Euler(newRot);
        }
        instance.transform.parent = parent;

    }

    void Fissure() {
        GameObject fissure = Resources.Load<GameObject>("Elements/Ground/FissureAttack");
        fissure = Instantiate(fissure, targetCircle.transform.position - Vector3.up * 2, targetCircle.transform.rotation);
        fissure.transform.parent = parent;

    }

    void WindSlash() {
        //get some cool effects here


    }

    void ShockChain() {
        //set the shits
        GameObject lightning = Resources.Load<GameObject>("Elements/Electricity/ChainLightning");
        lightning = Instantiate(lightning);
        lightning.GetComponent<ChainLightningScript>().isRightHand = rHand == Elements.Electricity;
        lightning.GetComponent<ChainLightningScript>().lockOn = lockOn;
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
}

public static class AnimTypes {
    public static readonly int
        ShootBolt1 = 1,
        ShootBolt2 = 2;
}
