using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cave : MonoBehaviour
{
    InteractableObject interactableObject;
    // Start is called before the first frame update
    void Start()
    {
        interactableObject = GetComponent<InteractableObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(LevelManager.Instance.completedQuests.Contains(8) && interactableObject.isActivated)
        {
            //Load Scene 3
        } else
        {
            interactableObject.isActivated = false;
        }
    }
}
