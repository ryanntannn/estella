using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
   public GameObject Player; //set player so text spawn lookng at player
    public Vector3 offset = new Vector3(0, 1, 0); //show above enemies head
    public Vector3 randomizePosition = new Vector3(0.5f, 0, 0);
    // Start is called before the first frame update
    void Start()
    { 
        transform.localPosition += offset; //set spawn position
        transform.localPosition += new Vector3(Random.Range(-randomizePosition.x, randomizePosition.x), //random x axis
                                               Random.Range(-randomizePosition.y, randomizePosition.y), //random y axis
                                               Random.Range(-randomizePosition.z, randomizePosition.z)); //random z axis

        GameObject[] playerobj = GameObject.FindGameObjectsWithTag("MainCamera");
        Player = playerobj[0]; //there is only 1 player object, initialise player object
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(Player.transform);
        
    }
}
