using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk_Sound : MonoBehaviour
{//This script takes the player object tries to play sound when walking

    public AudioSource sourceName;
    public GameObject player;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = player.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //play when player is moving and sound not playing yet
        if ((animator.GetCurrentAnimatorStateInfo(0).IsName("Running") || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) && sourceName.isPlaying == false)
        {
            sourceName.Play();

        }
        else
        if (!(animator.GetCurrentAnimatorStateInfo(0).IsName("Running") || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) && sourceName.isPlaying == true)
        {
            sourceName.Stop();

        }
    }
}
