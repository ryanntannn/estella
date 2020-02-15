using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SceneLoad : MonoBehaviour
{
    //This method is to create a loading screen while the background is loading to ensure that all assets are loaded first (this is needed
    //beacause my level is big.)
    public GameObject loadingscreen;
    public Slider slider;
    public TextMeshProUGUI percentage_text;

    public void Loadlevel(int sceneindex)
    {
        StartCoroutine(LoadAsynchronously(sceneindex));
        
    }

    IEnumerator LoadAsynchronously(int sceneindex) //starts a co-routine
    {
       

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneindex);  //loads the next scene 'sceneindex' while still in current scene
        loadingscreen.SetActive(true);
        while (!operation.isDone) //get info of done.
        {
            float progress = Mathf.Clamp01(operation.progress / .9f); //isdone will return true when progress is 0.9. This ensures that we get values 0~1
            
            percentage_text.text = (progress * 100).ToString("F0") + "%" ;
            Debug.Log(operation);
            slider.value = progress;

            yield return null; //restart the routine when it is not done.
        }
        if (operation.isDone)
        {
            loadingscreen.SetActive(false);
        }
        
    }

}
