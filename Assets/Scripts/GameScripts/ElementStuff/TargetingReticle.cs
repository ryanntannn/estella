using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingReticle : Singleton<TargetingReticle> { 
    public bool isShowing = true;

    private GameObject m_mesh;
    // Start is called before the first frame update
    void Start() {
        if (transform.childCount > 0) {
            m_mesh = transform.GetChild(0).gameObject;
        }
    }

    // Update is called once per frame
    void Update() {
        SetPosition();
        if(m_mesh)
            m_mesh.SetActive(isShowing);
    }

    void SetPosition() {
        float range = 10;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Debug.DrawRay(Camera.main.transform.position, ray.direction * range, Color.red);
        //check if infront got anything
        if (!Physics.Raycast(ray, out hitInfo, range, 1 << Layers.Terrain)) {
            Vector3 newPos = Camera.main.transform.position + ray.direction * range;
            Physics.Raycast(newPos, -Vector3.up, out hitInfo, 100, 1 << Layers.Terrain);
        }
        transform.position = hitInfo.point;
        Vector3 lookRotation = Camera.main.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, lookRotation.y, 0);
    }
}
