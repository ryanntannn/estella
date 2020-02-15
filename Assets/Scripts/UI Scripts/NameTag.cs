using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMesh))]
public class NameTag : MonoBehaviour
{
    MeshRenderer mrenderer;
    TextMesh textMesh;
    Transform player;
    Transform mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        mrenderer = GetComponent<MeshRenderer>();
        player = PlayerControl.Instance.transform;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.position, transform.position) > 20 && mrenderer.enabled)
        {
            mrenderer.enabled = false;
        }
        else if (Vector3.Distance(player.position, transform.position) < 20 && !mrenderer.enabled)
        {
            mrenderer.enabled = true;
        }

        if (mrenderer.enabled)
        {
            transform.LookAt(mainCamera);
            transform.localEulerAngles += new Vector3(0, 180, 0);
        }
    }
}
