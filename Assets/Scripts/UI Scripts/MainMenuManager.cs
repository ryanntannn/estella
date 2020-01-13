using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    enum MainMenuState {
        TRANSITION,
        TITLE,
        SETTINGS
    }

    MainMenuState mainMenuState = MainMenuState.TITLE;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(mainMenuState == MainMenuState.TITLE)
        {
            if (Input.GetMouseButton(0))
            {
                mainMenuState = MainMenuState.TRANSITION;
                StartCoroutine(StartGame());
            }
        }
        else if (mainMenuState == MainMenuState.SETTINGS)
        {

        }
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(1);
    }
}
    