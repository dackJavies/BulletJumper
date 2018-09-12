using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobCannon : BigCannon {

	public override void Fire() {
		fired = true;
		current = Object.Instantiate(bullet, transform.position, transform.rotation);
		current.transform.parent = this.transform;
		current.GetComponent<Blob>().direction = direction;
	}

	protected override void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Player") {
			if (!fired) {
				Fire();
			} else {
				current.GetComponent<Blob>().SelfDestruct();
			}
		}
	}

}
