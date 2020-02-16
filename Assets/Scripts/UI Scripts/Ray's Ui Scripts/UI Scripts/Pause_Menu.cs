using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Pause_Menu : MonoBehaviour
{//This script controls the pausing of the game
    public static bool isPause;
    public GameObject PauseMenuUI;
    public GameObject player;
    public GameObject OptionsUI;

    Scene currentscene;

    // Update is called once per frame
    void Update()
    {

        
       if (SceneManager.GetActiveScene().name == "Level") { 
       if (Input.GetKeyDown(KeyCode.Escape) && OptionsUI.activeSelf == false) //allow player to press escape to open up pause menu only when: 
        {                                                                    //Is in level, not in options menu.
            if (isPause == false)
            {
                Pause();

            }
            else
            {
                Resume();
            }

        }

        }
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        player.GetComponent<MonoBehaviour>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        isPause = true;
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        player.GetComponent<MonoBehaviour>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        isPause = false;
    }

    public void QuitToMenu()
    {
        PauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(0);

    }
    public bool getpausevalue()
    {

        return isPause;
    }


    public void Pause_End()  //code for freezing the game in situations such as end screen
    {
        PauseMenuUI.SetActive(false);
        player.GetComponent<MonoBehaviour>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        isPause = true;
    }

    public void Pause_Restart()  //code for freezing the game in situations such as end screen
    {
        PauseMenuUI.SetActive(false);
        player.GetComponent<MonoBehaviour>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        isPause = true;
    }

}
