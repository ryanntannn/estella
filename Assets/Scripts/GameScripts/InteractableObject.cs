using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public float timeToActivate;
    public bool isActivated;
    //Time Left Before The Object is Activated
    public float timeLeftToActivate;

    void Start()
    {
        timeLeftToActivate = timeToActivate;
    }

    public void IsActivating()
    {
        if (!isActivated)
        {
            timeLeftToActivate -= Time.deltaTime;

            if(timeLeftToActivate <= 0)
            {
                isActivated = true;
            }
        }
    }

    public void CancelActivate()
    {
        if (!isActivated)
        {
            timeLeftToActivate = timeToActivate;
        }
    }
}
