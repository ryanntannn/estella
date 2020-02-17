using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_EmissionSound : MonoBehaviour
{
    public ParticleSystem parentParticleSystem;

    private int currentNumberOfParticles = 0;

    public AudioSource BornSounds;
    public AudioSource DieSounds;



    // Start is called before the first frame update
    void Start()
    {
       // parentParticleSystem = this.GetComponent<ParticleSystem>();
        if (parentParticleSystem == null)
            Debug.LogError("Missing ParticleSystem!", this);

        print("Name of Parent Particle " + parentParticleSystem.name);
    }

    // Update is called once per frame
    void Update()
    {
        var amount = Mathf.Abs(currentNumberOfParticles - parentParticleSystem.particleCount);

        if (parentParticleSystem.particleCount < currentNumberOfParticles)
        {
            
           //Play Destroyed Sound
        }

        print("Particle Count "  + parentParticleSystem.particleCount);

        if (parentParticleSystem.particleCount > currentNumberOfParticles)
        {
            print("PlayBornSounds");
            BornSounds.Play();
            //Play born sound
        }

        currentNumberOfParticles = parentParticleSystem.particleCount;
    }
}
