using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public float TimeToActivate;
    public bool IsActivated;
    //Time Left Before The Object is Activated
    private float TimeLeftToActivate;

    public void IsActivating()
    {
        if (!IsActivated)
        {
            TimeLeftToActivate -= Time.deltaTime;

            if(TimeLeftToActivate <= 0)
            {
                IsActivated = true;
            }
        }
    }
}
