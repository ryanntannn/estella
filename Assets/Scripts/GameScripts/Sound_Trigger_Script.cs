using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Trigger_Script : MonoBehaviour
{
    public bool PlaysoundonEnter;
    public bool CheckforExit;
    public AudioSource soundsource;
    // Start is called before the first frame update
    void Start()
    {
        //  AudioSource soundsource = GetComponent<AudioSource>();
        soundsource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   void OnTriggerEnter( Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (PlaysoundonEnter == true && soundsource.isPlaying == false)
            {
                soundsource.Play();
            }

            if (PlaysoundonEnter == true && soundsource.isPlaying == false)
            {
                soundsource.Stop();
            }

        }
    }


    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && CheckforExit == true)
        {
         

            
                soundsource.Stop();
            

        }
    }
}



