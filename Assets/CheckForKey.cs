using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForKey : MonoBehaviour
{

     public GameObject MagicBarrier;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && LevelManager.Instance.completedQuests.Contains(3) )
        {
            MagicBarrier.SetActive(false);
            IngameUI.Instance.ShowPopUp("Magic Barrier Dispeled", 3.0f);
        }
    }
}
