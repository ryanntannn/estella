using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementControl : MonoBehaviour {
    public int lHand = 1, rHand = 1;

    public string lHandCurrent, rHandCurrent;

    // Start is called before the first frame update
    void Start() {

    }

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

        //no combinationing
        if (!Input.GetKey(KeyCode.LeftAlt)) {
            if (Input.GetKey(KeyCode.Mouse0)) {

            } else if (Input.GetKey(KeyCode.Mouse1)) {

            }
        } else {
            if (Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1)) {
                //do combination
                Combination();
            }
        }
    }

    void Combination() {
        switch (lHand | rHand) {
            case Elements.Water | Elements.Fire:
                print("Steam");
                break;
            case Elements.Water | Elements.Earth:
                print("Mud");
                break;
            case Elements.Water | Elements.Wind:
                print("Blizzard");
                break;
            case Elements.Water | Elements.Electricity:
                print("Shock");
                break;
            case Elements.Fire | Elements.Earth:
                print("Magma");
                break;
            case Elements.Fire | Elements.Wind:
                print("Blaze");
                break;
            case Elements.Fire | Elements.Electricity:
                print("Plasma");
                break;
            case Elements.Earth | Elements.Wind:
                print("Dust");
                break;
            case Elements.Earth | Elements.Electricity:
                print("Magnitize");
                break;
            case Elements.Wind | Elements.Electricity:
                print("Strom");
                break;
            case Elements.Wind:
                print("Big wind");
                break;
            case Elements.Water:
                print("Big water");
                break;
            case Elements.Fire:
                print("Big fire");
                break;
            case Elements.Earth:
                print("Big earth");
                break;
            case Elements.Electricity:
                print("Fat zap");
                break;
            default:
                print("Something wrong");
                break;
        }
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
}
