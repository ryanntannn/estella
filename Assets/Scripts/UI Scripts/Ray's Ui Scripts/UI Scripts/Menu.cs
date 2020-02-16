using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void LoadScene(string level)
    {
        SceneManager.LoadScene(level);
    }


    public void QuitGame()
    {
        print("Quit App");
        Application.Quit();
    }
}
