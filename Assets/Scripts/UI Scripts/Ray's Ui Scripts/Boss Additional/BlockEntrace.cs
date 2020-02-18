using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEntrace : MonoBehaviour
{//This is a blocker for the player so stop player from runnig away in boss fight
    public GameObject block;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        block.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.GetComponentInChildren<PlayerControl>().currentHealth <= 0) && block.activeSelf == true)
        {
            block.SetActive(false);

        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            block.SetActive(true);
        }
    }


}
