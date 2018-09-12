using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	protected Rigidbody2D RB;
	protected CircleCollider2D CC;
	protected SpriteRenderer SR;

	public Vector2 direction;
	public static GameObject thePlayer;

	protected float BOUNCE_DISTANCE = 20.0f;
	protected float BOUNCE_ANGLE = 45f;
	protected float BOUND_WIDTH_FACTOR = 3;

	// Use this for initialization
	protected void Start () {
		if (thePlayer == null) {
			thePlayer = GameObject.Find("Player");
		}
		RB = GetComponent<Rigidbody2D>();
		CC = GetComponent<CircleCollider2D>();
		SR = GetComponent<SpriteRenderer>();

		RB.velocity = direction;
//		CC.enabled = false;
//		SR.enabled = false;
	}

	protected virtual void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerHurtBox" && tag != "UsedBullet") {
			other.gameObject.transform.parent.gameObject.GetComponent<Player>().Die();
		} else if (other.tag == "Wall" || other.tag == "Platform" || other.tag == "Ceiling" || other.tag == "Spikes") {
			SelfDestruct();
		} else if (other.tag == "Cannon" && other.gameObject != transform.parent.gameObject) {
			SelfDestruct();
		}
	}

	public virtual void SelfDestruct() {
		Object.Destroy(this.gameObject);
	}

	public virtual void Used() {
		SR.enabled = false;
		tag = "UsedBullet";
	}

	public virtual void Respawn() {
		SR.enabled = true;
		tag = "Bullet";
	}
	
}
