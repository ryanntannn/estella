using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Settings_Video : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    //This is for Video settings
    Resolution[] resolutions;

  
    void Start()
    {
        resolutions = Screen.resolutions; //get all available resolutions

       // resolutionDropdown.ClearOptions(); //clear all preset resolutions

        List<string> options = new List<string>(); //create a dynamic list

        int currentResolutionIndex =0; //without setting current reso index players will start of with the smallest screen size

        for (int i = 0; i < resolutions.Length; i++)
        {

            string option = resolutions[i].width + " X " + resolutions[i].height; //display width X height of resolutions

            options.Add(option); //add options to a set of options in list

            if (resolutions[i].width == Screen.width && 
                resolutions[i].height == Screen.height )
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options); //converts list into a displayed UI elemnt;
        resolutionDropdown.value = currentResolutionIndex; //give dropdown the correct resolution;
        resolutionDropdown.RefreshShownValue();//refresh the box;
    }


    public void setResolution(int resolutionIndex) //we want to set the resolution after player inputs in settings
    {
        Resolution resolution = resolutions[resolutionIndex]; //get the dropw index and match resolutions array
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen); //set resolution to desired resolution
    }

    public void setQuality(int qualityIndex) //set to the graphics portion of the settings
    {
        Debug.Log("Quality has been Changed to " + qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }


}
