using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour {
    public float range = 3;
    public bool drawDebugLines = true;
    public enum RayCastMode { Single, Double, Triple }
    public RayCastMode mode = RayCastMode.Triple;

    float colliderSize = 1;
    // Start is called before the first frame update
    void Start() {
        BoxCollider collider = GetComponent<BoxCollider>(); //collider
        if (collider) {
            colliderSize = collider.size.x / 2;
        }
    }

    private void LateUpdate() {
        HandleObs();
    }

    void HandleObs() {
        Quaternion currentRotation = transform.rotation;
        switch (mode) {
            case RayCastMode.Single:
                //raycast forward
                if (Physics.Raycast(transform.position, transform.forward, range)) {
                    //new rotation
                    Quaternion newRot = Quaternion.Euler(0, 10, 0) * currentRotation;
                    currentRotation = Quaternion.Lerp(currentRotation, newRot, Time.deltaTime);
                }
                break;
            case RayCastMode.Double:
                bool rightRay = Physics.Raycast(transform.position + transform.right * colliderSize, transform.forward, range);
                bool leftRay = Physics.Raycast(transform.position - transform.right * colliderSize, transform.forward, range);
                //if right ray hit and left ray don't hit
                if (rightRay && !leftRay) {
                    //turn towards left
                }else //if left ray hit and right ray don't hit
                if (leftRay && !rightRay) {
                    //turn towards right
                }else if(!leftRay && !rightRay){
                    //turn some direction I guess
                }
                break;
            case RayCastMode.Triple:
                break;
            default:
                break;
        }
        transform.rotation = currentRotation;
    }

    private void OnDrawGizmos() {
        if (drawDebugLines) {
            BoxCollider collider = GetComponent<BoxCollider>(); //collider
            if (collider) {
                colliderSize = collider.size.x / 2;
            }

            //draw rays
            switch (mode) {
                case RayCastMode.Single:
                    //forward raycast
                    Debug.DrawRay(transform.position, transform.forward * range, Color.red);
                    break;
                case RayCastMode.Double:
                    //forward raycast to the sides
                    //right
                    Debug.DrawRay(transform.position + transform.right * colliderSize, transform.forward * range, Color.red);
                    //left
                    Debug.DrawRay(transform.position - transform.right * colliderSize, transform.forward * range, Color.red);
                    break;
                case RayCastMode.Triple:
                    //forward ray + tilited left and right rays
                    Debug.DrawRay(transform.position, (transform.forward + transform.right) * range, Color.red);
                    Debug.DrawRay(transform.position, (transform.forward - transform.right) * range, Color.red);
                    //forward ray
                    goto case RayCastMode.Single;
                default:
                    break;
            }
        }
    }
}
