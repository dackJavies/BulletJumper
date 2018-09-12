using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidBox : MonoBehaviour {

	private BoxCollider2D BC;

	// Use this for initialization
	void Start () {
		BC = GetComponent<BoxCollider2D>();
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			BC.isTrigger = false;
		}
	}
	
}
