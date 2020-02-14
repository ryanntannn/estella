using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GiantBoss : MonoBehaviour
{
    public Enemy bossBody;
    public BossArm[] bossArms;
    public GameObject player;
    public InteractableObject altar;

    public bool isAttacking = false;
    public bool isAggro = false;

    bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        bossBody = transform.GetChild(0).GetComponent<Enemy>();
        bossArms = transform.GetComponentsInChildren<BossArm>();
        altar = transform.GetComponentInChildren<InteractableObject>();
        foreach(BossArm ba in bossArms)
        {
            ba.giantBoss = this;
        }
        player = GameObject.Find("Player");
    }

    public void AwakeBoss()
    {
        bossBody.GetComponent<Rigidbody>().DOMove(bossBody.transform.position + new Vector3(0, 13, 0), 4f);
        foreach (BossArm ba in bossArms)
        {
            ba.AwakePillars();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (altar.isActivated && !activated)
        {
            activated = true;
            Destroy(altar.gameObject);
            AwakeBoss();
        }

        if (isAggro)
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
}
