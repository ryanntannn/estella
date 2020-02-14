using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkylarkWraithScript : MonoBehaviour {
    private BallManager m_ballManager;
    public BallManager Manager { set { m_ballManager = value; } }
    public float ttl = 10;
    public float damage = 20;

    private Coroutine m_killCr;

    public void StartKillingSelf() {
        m_killCr = StartCoroutine(KillSelf());
    }

    IEnumerator KillSelf() {
        yield return new WaitForSeconds(ttl);
        m_ballManager.Balls.Remove(this);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Player) {
            other.GetComponent<PlayerControl>().TakeDamage(damage, transform.position);

            //make immediate
            StopCoroutine(m_killCr);
            m_ballManager.Balls.Remove(this);
            Destroy(gameObject);
        }
    }
}
