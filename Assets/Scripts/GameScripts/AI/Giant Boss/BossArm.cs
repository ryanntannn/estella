using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossArm : MonoBehaviour
{
    public Vector3 originalPosition;
    public Rigidbody rigidbody;
    public GiantBoss giantBoss;

    bool takenDamage = false;

    public enum State
    {
        IDLE, SWING, SLAM, DORMANT
    }

    public State state;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.IDLE)
        {
            transform.eulerAngles += new Vector3(0, 10, 0) * Time.deltaTime;
        }
    }

    public void AwakePillars()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(rigidbody.DOMove(originalPosition + new Vector3(0,19.6f,0), 4f)).OnComplete(() =>
        {
            state = State.IDLE;
        });
    }

    public void Slam(Vector3 target)
    {
        state = State.SLAM;
        rigidbody.DOMove(target + new Vector3(0,3,0), 2f);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(rigidbody.DOMove(target + new Vector3(0, -2, 0), 0.1f)).PrependInterval(2.2f);
        Sequence sequence2 = DOTween.Sequence();
        sequence2.Append(rigidbody.DOMove(target + new Vector3(0, 3, 0), 0.3f)).PrependInterval(2.8f);
        Sequence sequence3 = DOTween.Sequence();
        sequence3.Append(rigidbody.DOMove(originalPosition, 2f)).PrependInterval(3.1f).OnComplete(() =>
        {
            doneAttacking();
            takenDamage = false;
        });
    }

    public void Swing(Vector3 target)
    {
        state = State.SWING;
        rigidbody.DOMove(target, 2f);
    }

    public void doneAttacking()
    {
        state = State.IDLE;
        giantBoss.isAttacking = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerControl>() && !takenDamage)
        {
            giantBoss.bossBody.DealDamage(40.0f);
            takenDamage = true;
        }
    }
}
