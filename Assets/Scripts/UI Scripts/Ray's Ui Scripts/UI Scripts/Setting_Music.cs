using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

//This script is tied to the settings and will send volume info to the audio mixer(music box)
public class Setting_Music : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetMasterVoulume(float volume) //setting effect master vol
    {
       
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVoulume(float volume)//setting effect music vol
    {
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume) //setting effect SFX vol (guns and stuff)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
