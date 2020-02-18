using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Timelinetrigger : MonoBehaviour
{
    public PlayableDirector timeline;
    public GameObject Player;
    public SkylarkBoss skylarkstop;

    private bool HasPlayed;
    private double time;

    // Start is called before the first frame update
    void Start()
    {
        
       // timeline = GetComponent<PlayableDirector>();
        print("Name of timeline is " + timeline.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (HasPlayed == true)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                skylarkstop.enabled = true;
                Player.GetComponent<PlayerControl>().enabled = true;
            }

        }

    }

 void OnTriggerEnter(Collider player)
     {
        if (player.gameObject.tag == "Player" && HasPlayed == false)
        {
            print("Trying to play");
            timeline.Play();
            time = timeline.duration;
            Player.GetComponent<PlayerControl>().enabled = false;
            skylarkstop.enabled = false;
            HasPlayed = true;
        }
        
     }


}
