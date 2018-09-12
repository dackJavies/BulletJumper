using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public GameObject[] cannons;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			other.gameObject.GetComponent<Player>().checkPoint = transform.position;
			RespawnBullets();
		}
	}

	private void RespawnBullets() {
		Cannon cur;
		foreach(GameObject cannonGO in cannons) {
			cur = cannonGO.GetComponent<Cannon>();
			cur.RespawnBullets();
		}
	}

}
