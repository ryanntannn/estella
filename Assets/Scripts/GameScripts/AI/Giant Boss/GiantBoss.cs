using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBoss : MonoBehaviour
{
    public Enemy bossBody;
    public BossArm[] bossArms;
    public GameObject player;

    public bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        bossBody = transform.GetChild(0).GetComponent<Enemy>();
        bossArms = transform.GetComponentsInChildren<BossArm>();
        foreach(BossArm ba in bossArms)
        {
            ba.giantBoss = this;
        }
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking)
        {
            int randArm = (int)Random.Range(0, 4);
            int randMove = 0;//(int)Random.Range(0, 2);
            //Attack
            if (randMove == 0)
            {
                Debug.Log(randArm + " " + randMove);
                bossArms[randArm].Slam(player.transform.position);
            }
            else
            {
                bossArms[randArm].Swing(player.transform.position);
            }

            isAttacking = true;
        }
    }
}
