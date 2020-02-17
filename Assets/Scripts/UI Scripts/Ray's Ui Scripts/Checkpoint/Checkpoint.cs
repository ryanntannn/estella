using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class set the checkpoint of the object, when player dies it goes back to the last know location

public class Checkpoint : MonoBehaviour
{
    public GameObject player;

    private PlayerControl playerControl;
    private Animator animator;
    private Vector3 checkpointCoordinates;
    // Start is called before the first frame update
    void Start()
    {
     playerControl  =  player.GetComponent<PlayerControl>();
     animator = player.GetComponentInChildren<Animator>();
      
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown("k"))
        {
            print("Pressed K");
            ResetToLastKnownLocation();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Checkpoint")
        {
            checkpointCoordinates = collider.transform.position;
            print("Checkpoint Collided");
        }

    }

  public  void ResetToLastKnownLocation() //reset to last known location
    {
        RevivePlayer();
        player.transform.position = checkpointCoordinates;
        playerControl.enabled = true;
        animator.SetTrigger("Respawn");
    }


    void RevivePlayer() //reser theri health
    {
        playerControl.currentStamina = 100f;
        playerControl.currentHealth = 100f;
    }

}
