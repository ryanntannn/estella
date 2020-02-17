using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cave : MonoBehaviour
{
    InteractableObject interactableObject;
    public bool debug;
    bool activated;
    // Start is called before the first frame update
    void Start()
    {
        interactableObject = GetComponent<InteractableObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (debug && interactableObject.isActivated && !activated)
        {
            StartCoroutine(LoadAsyncOperation());
            activated = true;
        }

        if(LevelManager.Instance.completedQuests.Contains(8) && interactableObject.isActivated && !activated)
        {
            //Load Scene 3
            StartCoroutine(LoadAsyncOperation());
            activated = true;
        }
        else
        {
            interactableObject.isActivated = false;
        }
    }

    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(3);

        yield return new WaitForEndOfFrame();
    }
}
