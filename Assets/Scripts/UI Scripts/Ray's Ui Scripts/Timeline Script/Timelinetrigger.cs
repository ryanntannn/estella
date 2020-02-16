using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Timelinetrigger : MonoBehaviour
{
    public PlayableDirector timeline;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        
       // timeline = GetComponent<PlayableDirector>();
        print("Name of timeline is " + timeline.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 void OnTriggerEnter(Collider player)
     {
        if (player.gameObject.tag == "Player")
        {
            print("Trying to play");
            timeline.Play();

        }
        
     }
}
