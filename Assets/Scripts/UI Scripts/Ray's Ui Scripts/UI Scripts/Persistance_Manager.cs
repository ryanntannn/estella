using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistance_Manager : MonoBehaviour
{
    //This is a script to create instance and prevent them from being affected by loadingscenes
    // Start is called before the first frame update

    public static Persistance_Manager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
