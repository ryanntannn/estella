using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animated_Material : MonoBehaviour
{//Animater materials during runtime
    public Material[] materials;

    public float changetime;

    private int currentmat;
    public MeshRenderer meshobject;
    private float timecheck;
    void Start()
    {
      //  MeshRenderer meshobject = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timecheck += Time.deltaTime;

        if (timecheck >= changetime)
        {
            if (currentmat + 1 > materials.Length)
            {
                currentmat = 0;
                meshobject.material = materials[currentmat];
                timecheck = 0;
            }
            else
            {
                meshobject.material = materials[currentmat++];
                timecheck = 0;
            }
        }
        
    }
}
