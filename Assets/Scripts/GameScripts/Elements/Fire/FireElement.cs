using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElement : Element {
    public override string ElementName => "Fire";

    public override void BoltType() {
        //shoot out one ball
        currentMana = Mathf.Clamp(currentMana + Time.deltaTime, 0, maxMana);
        if (Input.GetKeyDown(button) && currentMana >= 1) {
            currentMana -= boltCost;  //cost one
            //instansiate
            GameObject instance = Instantiate(bolt, transform.position, transform.rotation);//need to do the rotation properly
        }
    }

    public override void PowerType() {

    }

    public override void StreamType() {
        float deltaTime = Time.deltaTime;
        //check if stream is alr active
        //if not activate it
        if (Input.GetKeyDown(button) && !streamPS.isPlaying) {
            streamPS.Play();
        }

        //same thing here but opposite
        if(Input.GetKeyUp(button) && !streamPS.isStopped) {
            streamPS.Stop();
        }

        //here we use stream.isplaying to check if player is attacking
        //scuffed, but it works and syncs nicely with animation
        if (streamPS.isPlaying && currentMana > 0) {
            //drain mana
            currentMana = Mathf.Clamp(currentMana - deltaTime * streamDrain, 0, maxMana);
            //raycast out and check for target
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hitInfo;
            if(Physics.Raycast(ray, out hitInfo, range)) { //check for hit
                if(hitInfo.collider.gameObject.layer == Layers.Enemy) {    //enemy
                    hitInfo.collider.GetComponent<Enemy>().ReactFire(Types.Stream);
                }else if (hitInfo.collider.CompareTag("Power")) {

                }
            }
        }else {
            //refil mana if it is less than max
            if(currentMana < maxMana) {
                currentMana = Mathf.Clamp(currentMana + deltaTime, 0, maxMana);
            }      
        }
    }
}
