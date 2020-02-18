using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Trigger_Script : MonoBehaviour
{
    public bool PlaysoundonEnter;
    public bool CheckforExit;
    public AudioSource soundsource;
    public bool audiosourceForcedPlay = true;

   // private AudioFade audioFade;
    public float fadeTime = 4f; //the duration to fade in and out of sound
    public float fadeInMaxVolume =0.7f; //the max volume of the fade in 

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
                // soundsource.Play();

                StopAllCoroutines();
                StartCoroutine(AudioFade.FadeIn(soundsource, fadeTime,fadeInMaxVolume)); //using a coroutine we can fade in and out sound
            }


        }
    }


    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && CheckforExit == true && soundsource.isPlaying == true)
        {

            StopAllCoroutines();
            StartCoroutine(AudioFade.FadeOut(soundsource, fadeTime));
            
            

        }
    }

   
}



