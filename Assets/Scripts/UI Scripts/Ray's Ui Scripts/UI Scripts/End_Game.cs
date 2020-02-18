using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End_Game : MonoBehaviour
{
    public Enemy skylarkBoss;
    public GameObject player;
    public GameObject victoryScreen;
    public AudioSource victoryMusic;
    public AudioSource fightMusic;
    private Canvas_Elements canvas;

    private bool hasBeenCalled = false;
    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectOfType<Canvas_Elements>();
    }

    // Update is called once per frame
    void Update()
    {
        if (skylarkBoss.health <= 0 && hasBeenCalled == false)
        {
            endGame();
            hasBeenCalled = true;

        }
    }

    void endGame()
    {
        //playerControl.enabled = false;
        victoryMusic.Play();
        victoryScreen.SetActive(true);
        canvas.UpdateGuiSettings("Main Menu");
        fightMusic.Stop();
      
        player.GetComponent<MonoBehaviour>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;


    }

    public void ChangeScenetoMain()
    {
        SceneManager.LoadScene(0);

    }
}
