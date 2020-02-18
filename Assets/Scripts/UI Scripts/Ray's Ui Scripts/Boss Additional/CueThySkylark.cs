using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueThySkylark : MonoBehaviour
{
    public GameObject Skylark;

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
        if (collider.gameObject.tag == "Player")
        {
            if (Skylark.activeSelf == false)
            {
                Skylark.SetActive(true);

            }
        }
    }
}
