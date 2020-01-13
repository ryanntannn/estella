using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour {
    public float range = 3;
    public bool drawDebugLines = true;
    public enum RayCastMode { Single, Double, Triple }
    public RayCastMode mode = RayCastMode.Triple;
    public LayerMask obs;

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
                if (Physics.Raycast(transform.position, transform.forward, range, layerMask: obs)) {
                    //new rotation
                    Quaternion newRot = Quaternion.Euler(0, 10, 0) * currentRotation;
                    currentRotation = Quaternion.Lerp(currentRotation, newRot, Time.deltaTime);
                }
                break;
            case RayCastMode.Double:
                RaycastHit rightHitInfo, leftHitInfo;
                bool rightRay = Physics.Raycast(transform.position + transform.right * colliderSize, transform.forward, out rightHitInfo, range, layerMask: obs);
                bool leftRay = Physics.Raycast(transform.position - transform.right * colliderSize, transform.forward, out leftHitInfo, range, layerMask: obs);
                //if right ray hit and left ray don't hit
                if (rightRay && !leftRay) {
                    //turn towards left
                    Quaternion newRot = Quaternion.Euler(0, -10, 0) * currentRotation;
                    currentRotation = Quaternion.Lerp(currentRotation, newRot, Time.deltaTime);
                } else //if left ray hit and right ray don't hit
                if (leftRay && !rightRay) {
                    //turn towards right
                    Quaternion newRot = Quaternion.Euler(0, 10, 0) * currentRotation;
                    currentRotation = Quaternion.Lerp(currentRotation, newRot, Time.deltaTime);
                } else if (!leftRay && !rightRay) {
                    //turn towards direction whose ray is longer
                    Quaternion newRot = Quaternion.Euler(0, (rightHitInfo.distance > leftHitInfo.distance) ? 10 : -10, 0) * currentRotation;
                    currentRotation = Quaternion.Lerp(currentRotation, newRot, Time.deltaTime);
                }
                break;
            case RayCastMode.Triple:
                //left to right
                float direction = 0;
                bool gotSomething = false;
                for (int count = -1; count <= 1; count++) {
                    RaycastHit hitInfo;
                    if (!Physics.Raycast(transform.position, transform.forward + transform.right * count, out hitInfo, range, layerMask: obs)) {
                        direction += count;
                    }else if(count != 0) {
                        direction += hitInfo.distance * count;
                        gotSomething = true;
                    } else {
                        gotSomething = true;
                    }
                }
                //determine which direction to turn to
                if (gotSomething) {
                    Quaternion dir = Quaternion.Euler(0, 10 * (direction >= 0 ? 1 : -1), 0) * currentRotation;
                    currentRotation = Quaternion.Lerp(currentRotation, dir, Time.deltaTime);
                }
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
                    for (int count = -1; count <= 1; count++) {
                        Debug.DrawRay(transform.position, (transform.forward + transform.right * count) * range, Color.red);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
