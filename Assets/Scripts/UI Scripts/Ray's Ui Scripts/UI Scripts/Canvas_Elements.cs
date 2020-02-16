using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Canvas_Elements : MonoBehaviour
{
    //This script helps to enable/disable certain ui elements depending on the needs of the game. Thus allowing this canvas to be used 
    // accross mutiple platforms as it will be loaded onto the scene with DontDestroyOnLoad.

  
    public GameObject menuPage;
    public GameObject pausePage;
    public GameObject OptionsPage;
    public GameObject BackgroundMenu;

    bool update = false;
    Scene currentscene;
    string currentlevel = "Main Menu";
    void Start()
    {
        
        
     
    }
   public void UpdateGuiSettings(string sceneName)
    {
        currentlevel = sceneName;
        Scene currentscene = SceneManager.GetActiveScene();
        currentscene = SceneManager.GetActiveScene();

        if (sceneName == "Main Menu")
        {
            BackgroundMenu.SetActive(true);
            menuPage.SetActive(true);
        }
        if (sceneName == "Level")
        {
            print(currentscene.name);
            BackgroundMenu.SetActive(false);
            menuPage.SetActive(false);
        }
    }

    public void UpdateOptionsGuiSettings() //for the back button in Options to behave differently in both scenes
    {
        string changeScene = currentlevel; //get info of current level from starting page and quit. 

        //Note: Turning off option page is already set in Option buttion using Unity setactive.
        if (changeScene == "Main Menu" && menuPage.activeSelf == false) //in menu and menuPage is turned off
        {

            menuPage.SetActive(true); //turn on menu page and turn off option page
           
        }
       
        if (changeScene == "Level" && pausePage.activeSelf == false) //in level and pause page is turned off, we want to set opposite
        {
            pausePage.SetActive(true); //same as above logic
           
        }
       
    }

    void Update()
    {
        /*
        Scene currentscene = SceneManager.GetActiveScene();
        currentscene = SceneManager.GetActiveScene();
      
        if (currentscene.name == "Main Menu")
        {
            BackgroundMenu.SetActive(true);
            menuPage.SetActive(true);
        }
        if (currentscene.name == "Level")
        {
            print(currentscene.name);
            BackgroundMenu.SetActive(false);
            menuPage.SetActive(false);
        }
            */
    }

   

}
