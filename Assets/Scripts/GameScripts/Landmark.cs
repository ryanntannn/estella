using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmark : MonoBehaviour
{
    public float radius;
    public string landMarkName;
    IngameUI igui;
    GameObject player;
    [SerializeField]
    private bool inArea = false;
    public bool debug = false;

    // Start is called before the first frame update
    void Start()
    {
        igui = GameObject.Find("UI (1)").GetComponent<IngameUI>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(!inArea && Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            inArea = true;
            igui.ShowBigPopUp("-" + landMarkName + "-", 4f);
        }

        if(inArea && Vector3.Distance(transform.position, player.transform.position) > radius)
        {
            inArea = false;
        }
    }

    private void OnDrawGizmos() {
        if (debug) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
