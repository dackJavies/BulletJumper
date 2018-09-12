using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftBullet : BigBullet {

	protected override void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerHurtBox") {
			other.gameObject.transform.parent.gameObject.GetComponent<Player>().Die();
		} else if (other.tag == "Cannon" && other.gameObject != transform.parent.gameObject) {
			SelfDestruct();
		} else if (other.tag == "Bullet") {

		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
