using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityElement : Element {
    public override string ElementName => "Electricity";

    RaycastHit hitinfo;
    public Animator anim;
    public float warpDuration = 2;
    public Shader translucentShd, defShd;

    float internalCounter = 0;
    public override void BoltType() {
        //shoot out one ball
        currentMana = Mathf.Clamp(currentMana + Time.deltaTime, 0, maxMana);
        if (Input.GetKeyDown(button) && currentMana >= 1) {
            currentMana -= boltCost;  //cost one
            //dash forward 
            anim.SetTrigger("whenWarp");
            if (!defShd) defShd = transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.shader;
            ShowSelf(false);
        }

        if (anim.GetBool("isZapping")) {
            transform.position += transform.forward * Time.deltaTime * 10;
            internalCounter += Time.deltaTime;
            if(internalCounter >= warpDuration) {
                internalCounter = 0;
                anim.SetBool("isZapping", false);
                ShowSelf(true);
            }
        }

    }

    void ShowSelf(bool state) {
        //foreach (Transform child in transform.GetChild(0)) {
        //    child.gameObject.SetActive(state);
        //}
        transform.ChangeShader(state ? translucentShd : defShd);
    }

    public override void PowerType() {
        //regen mana
        currentMana = Mathf.Clamp(currentMana + Time.deltaTime, 0, maxMana);

        //check if got enough mana
        if (currentMana >= powerCost) {
            //when holding down button
            if (Input.GetKey(button)) {
                //show the target area
                //raycast out from center of camera
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out hitinfo, range * 5, 1 << Layers.Terrain)) {
                    Physics.Raycast(Camera.main.transform.position + ray.direction * range * 5, -transform.up, out hitinfo, 100);   //100 just incase player shoot out of bounds
                }

                //draw circle at hitinfo.point
                targetCircle.SetActive(true);
                targetCircle.transform.position = hitinfo.point;
            } else if (Input.GetKeyUp(button)) {
                targetCircle.SetActive(false);
                //instaniate torado
                Instantiate(power, hitinfo.point, Quaternion.identity);
            }
        }//check mana if
    }

    public override void StreamType() {
        float deltaTime = Time.deltaTime;
        //check if stream is alr active
        //if not activate it
        if (Input.GetKeyDown(button) && !streamPS.isPlaying) {
            streamPS.Play();
        }

        //same thing here but opposite
        if (Input.GetKeyUp(button) && !streamPS.isStopped) {
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
            if (Physics.Raycast(ray, out hitInfo, range)) { //check for hit
                if (hitInfo.collider.gameObject.layer == Layers.Enemy) {    //enemy
                    hitInfo.collider.GetComponent<Enemy>().ReactElectricity(Types.Stream);
                }
            }
        } else {
            //refil mana if it is less than max
            if (currentMana < maxMana) {
                currentMana = Mathf.Clamp(currentMana + deltaTime, 0, maxMana);
            }
        }
    }

    public void Warp() {
        anim.SetBool("isZapping", true);
    }
}
