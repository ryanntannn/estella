using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//takes cares of balls for me
public class BallManager : MonoBehaviour {
    public GameObject ballPfb;
    public int numberOfBalls = 5;
    public float speedOfRotation = 50;
    public float speedOfExpension = 1;

    List<SkylarkWraithScript> m_balls = new List<SkylarkWraithScript>();
    public List<SkylarkWraithScript> Balls { get { return m_balls; } set { m_balls = value; } }
    float m_step;
    GameObject parent;
    float m_dist = 1;
    float m_yRot = 0;
    // Start is called before the first frame update
    void Start() {
        if (!ballPfb) ballPfb = Resources.Load<GameObject>("Skylark/SkylarksBall");
        m_step = 360 / numberOfBalls;
        parent = transform.parent.gameObject;
        transform.parent = null;

        for(int count = 0; count <= numberOfBalls - 1; count++) {
            m_balls.Add(Instantiate(ballPfb, transform).GetComponent<SkylarkWraithScript>());
            Vector3 finalPos = parent.transform.position;
            finalPos.x += Mathf.Sin(Mathf.Deg2Rad * m_step * count);
            finalPos.z += Mathf.Cos(Mathf.Deg2Rad * m_step * count);
            m_balls[count].transform.position = finalPos;
            m_balls[count].Manager = this;
            m_balls[count].ttl += count * 2;
            m_balls[count].StartKillingSelf();
        }
    }

    // Update is called once per frame
    void Update() {
        if (m_balls.Count > 0) {
            transform.position = Vector3.Lerp(transform.position, parent.transform.position, Time.deltaTime * 10);

            m_yRot += (Time.deltaTime * speedOfRotation * Mathf.Exp(1.2f)) % 360;
            m_dist += (Time.deltaTime * speedOfExpension) / Mathf.Exp(0.5f);
            m_step = 360 / m_balls.Count;

            for (int count = 0; count <= m_balls.Count - 1; count++) {
                Vector3 finalPos = transform.position;
                finalPos.x += Mathf.Sin(Mathf.Deg2Rad * (m_step * count + m_yRot)) * m_dist;
                finalPos.z += Mathf.Cos(Mathf.Deg2Rad * (m_step * count + m_yRot)) * m_dist;
                m_balls[count].transform.position = finalPos;
            }
        }else {
            Destroy(gameObject);
        }
    }
}
