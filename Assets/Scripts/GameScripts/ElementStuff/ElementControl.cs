using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementControl : MonoBehaviour {
    public int lHand = 1, rHand = 2;
    [Tooltip("Displays current element")]
    public string lHandCurrent, rHandCurrent;
    public float delay = 0.3f;

    public KeyCode rightHand = KeyCode.Mouse0, leftHand = KeyCode.Mouse1;

    bool doneAlr = false;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //I really dont wanna do movement yet

        lHandCurrent = Elements.ByteToName(lHand);
        rHandCurrent = Elements.ByteToName(rHand);

        if (Input.GetKeyDown(rightHand)) {
            StartCoroutine(Input.GetKey(KeyCode.LeftAlt) ? DoBigBoy(lHand) : NoCombination(lHand));
        }
        if (Input.GetKeyDown(leftHand)) {
            StartCoroutine(Input.GetKey(KeyCode.LeftAlt) ? DoBigBoy(rHand) : NoCombination(rHand));
        }

        if(Input.GetKey(rightHand) && Input.GetKey(leftHand) && !doneAlr) {
            StopAllCoroutines();
            print("Comb");
            doneAlr = true;
            Combination();
        }

        if(Input.GetKeyUp(rightHand) || Input.GetKeyUp(leftHand)) {
            doneAlr = false;
        }

    }

    //I swear if you put some dumbass number
    /// <summary>
    /// <para>Change the hand's element to a new number</para>
    /// <para>Please use Element.(name of new element)</para>
    /// </summary>
    /// <param name="isRight"></param>
    /// <param name="newInput"></param>
    public void ChangeElement(bool isRight, int newInput) {
        if (isRight) rHand = newInput;
        else lHand = newInput;
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
            case 1:
                BubbleShot();
                break;
            case 2:
                Fireball();
                break;
            case 4:
                Fissure();
                break;
            case 8:
                WindSlash();
                break;
            case 16:
                ShockChain();
                break;
            default:
                print("Something wrong");
                break;
        }
    }

    #region Combination stuff
    void DoSteam() {

    }

    void DoMud() {

    }

    void DoBlizzard() {

    }

    void DoShock() {

    }

    void DoMagma() {

    }

    void DoDust() {

    }

    void DoPlasma() {

    }

    void DoStorm() {

    }

    void DoMagnetize() {

    }

    void DoBlaze() {

    }
    #endregion

    #region Big boy single elements
    void DoTsunami() {
        //load in tsunami behind player
        GameObject tsunami = Resources.Load<GameObject>("Elements/Water/Tsunami");
        tsunami = Instantiate(tsunami, transform.position - transform.forward - transform.up, transform.rotation);
    }

    void DoFirepit() {

    }

    void DoFlash() {

    }

    void DoTornado() {

    }

    void DoGroundBreaker() {

    }
    #endregion

    #region Regular stuff
    void BubbleShot() {
        GameObject instance = Resources.Load<GameObject>("Elements/Water/BubbleShot");
        instance = Instantiate(instance, transform.position, transform.rotation);
    }

    void Fireball() {

    }

    void Fissure() {

    }

    void WindSlash() {

    }

    void ShockChain() {

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
