using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShot : MonoBehaviour {
    public float speed = 12;
    public float timeToLive = 10;
    public GameObject waterDie;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(gameObject.KillSelf(timeToLive));
    }

    // Update is called once per frame
    void Update() {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == Layers.Enemy) {
			//deal damage
			other.GetComponent<Enemy>().TakeDamage(5);
			//slow enemy
			other.GetComponent<Enemy>().DebuffEnemy(5, Enemy.Effects.Slow);
			//then go make some puddle on ground
			RaycastHit hitInfo;
			if (Physics.Raycast(other.transform.position, -Vector3.up, out hitInfo, 1 << Layers.Terrain)) {
				GameObject puddle = Instantiate(Resources.Load<GameObject>("Elements/Water/Puddle"), hitInfo.point, Quaternion.identity);
			}

			//then die
			GameObject instance = Instantiate(waterDie, transform.position, transform.rotation);
			Destroy(instance, 1);
			Destroy(gameObject);
		} else if (other.CompareTag("Fissure")) {
			Instantiate(Resources.Load<GameObject>("Elements/Mud/MudPit"), other.transform.position, other.transform.rotation);
			Destroy(other.gameObject);
			Destroy(gameObject);

		} else if(other.gameObject.layer == Layers.Terrain) {
            GameObject instance = Instantiate(waterDie, transform.position, transform.rotation);
            Destroy(instance, 1);
            Destroy(gameObject);
        }
    }
}
