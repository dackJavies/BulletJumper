using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

	private BoxCollider2D SolidBox;
	private bool playerOnPlatform;
	private Player player;

	// Use this for initialization
	void Start () {
		playerOnPlatform = false;
		SetSolidBoxesEnabled(true);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Player") {
			player = collider.gameObject.GetComponent<Player>();
			float yVelocity = player.GetComponent<Rigidbody2D>().velocity.y;
			SetSolidBoxesEnabled(yVelocity < 0 || !PlayerInside(player.transform));
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Player") {
			if (player.GetComponent<Rigidbody2D>().velocity.y < 0 && PlayerInside(other.transform)) {
				SetSolidBoxesEnabled(false);
			}
			playerOnPlatform = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D collider) {
		if (collider.tag == "Player") {
			playerOnPlatform = false;
			SetSolidBoxesEnabled(true);
		}
	}

	private void SetSolidBoxesEnabled(bool setting) {
		GameObject current;
		foreach(Transform t in transform) {
			current = t.gameObject;
			current.GetComponent<BoxCollider2D>().enabled = setting;
		}
	}

	private void SetSolidBoxesTrigger(bool setting) {
		GameObject current;
		foreach(Transform t in transform) {
			current = t.gameObject;
			current.GetComponent<BoxCollider2D>().isTrigger = setting;
		}
	}

	private bool PlayerInside(Transform player) {
		return (player.position.y - (player.localScale.y / 2) - 0.1f 
			< transform.position.y + (transform.localScale.y / 2))
			&& ((player.position.x + (player.localScale.x / 2) + 0.1f > 
				transform.position.x - (transform.localScale.x / 2))
				||
			    (player.position.x - (player.localScale.x / 2) - 0.1f < 
				transform.position.x + (transform.localScale.x / 2)));
	}
	
}
