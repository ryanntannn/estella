using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameTagScript : MonoBehaviour
{
    IngameUI igui;

    private bool single = false;
    // Start is called before the first frame update
    void Start()
    {
        igui = GameObject.Find("UI (1)").GetComponent<IngameUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (single == false)
        {
            ShowName();
        }
        single = true;
    }

    void ShowName()
    {
        igui.ShowBigPopUp("-" + "Skylark the Usurper" + "-", 4f);
    }
}
