using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCannon : MonoBehaviour {

	public GameObject bullet;
	public Vector2 direction;
	protected GameObject current;
	protected bool fired;

	protected void Start() {
		fired = false;
	}

	public virtual void Fire() {
		fired = true;
		current = Object.Instantiate(bullet, transform.position, transform.rotation);
		current.transform.parent = this.transform;
		current.GetComponent<BigBullet>().direction = direction;
	}

	protected virtual void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Player") {
			if (!fired) {
				Fire();
			} else {
				current.GetComponent<BigBullet>().SelfDestruct();
			}
		}
	}

	
}
