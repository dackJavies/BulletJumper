using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBullet : Bullet {

	public override void SelfDestruct() {
		transform.parent.GetComponent<BigCannon>().Fire();
		Object.Destroy(this.gameObject);
	}

	public override void Used() {
		return;
	}

	public override void Respawn() {
		return;
	}

}
