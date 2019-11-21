using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementControl : MonoBehaviour {
    public int lHand = 1, rHand = 1;

    public string lHandCurrent, rHandCurrent;

    bool isCounting = false;

    // Start is called before the first frame update
    void Start() {

    }

    float internalCounter = 0;
    // Update is called once per frame
    void Update() {
        //I really dont wanna do movement yet

        lHandCurrent = Elements.ByteToName(lHand);
        rHandCurrent = Elements.ByteToName(rHand);

        //switch elements
        if (Input.GetKeyDown(KeyCode.Q)) {
            lHand <<= 1;
            lHand %= 32;
            if (lHand == 0) {
                lHand++;
            }
        } else if (Input.GetKeyDown(KeyCode.E)) {
            rHand <<= 1;
            rHand %= 32;
            if (rHand == 0) {
                rHand++;
            }
        }

        //if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKeyDown(KeyCode.Mouse1)) {
        //    print("Comb");
        //    //do combination
        //    Combination();
        //    internalCounter = 0;

        //} else if (internalCounter >= 0.2f) {
        //    if (Input.GetKeyDown(KeyCode.Mouse0)) {
        //        NoCombination(lHand);
        //    } else if (Input.GetKeyDown(KeyCode.Mouse1)) {
        //        NoCombination(rHand);
        //    }
        //    internalCounter = 0;
        //}

        if (isCounting) internalCounter += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if(internalCounter >= 0.2f) {
                internalCounter = 0;
            }
        }
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
            default:
                print("Something wrong");
                break;
        }
    }

    void NoCombination(int input) {
        switch (input) {
            case 1:
                print("Bubble sort");
                break;
            case 2:
                print("Fire ball");
                break;
            case 4:
                print("Fissure");
                break;
            case 8:
                print("Wind slash");
                break;
            case 16:
                print("Gay shock therapy");
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
        tsunami = Instantiate(tsunami, transform.position - transform.forward, transform.rotation);
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
