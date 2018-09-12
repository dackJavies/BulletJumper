using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : Bullet {

	private const float SHRINK_AMOUNT = 0.05f;
	private const float NORMAL_BULLET_GROWTH_PERCENTAGE = 0.1f;
	private const float BIG_BULLET_GROWTH_PERCENTAGE = 0.5f;
	private const float GROWTH_TIME = 0.2f;

	public override void SelfDestruct() {
		transform.parent.GetComponent<BlobCannon>().Fire();
		Object.Destroy(this.gameObject);
	}

	public override void Used() {
		return;
	}

	public override void Respawn() {
		return;
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerHurtBox" && tag != "UsedBullet") {
			other.gameObject.transform.parent.gameObject.GetComponent<Player>().Die();
		} else if (other.tag == "Wall" || other.tag == "Platform" || other.tag == "Ceiling" || other.tag == "Spikes") {
			SelfDestruct();
		} else if (other.tag == "Cannon" && other.gameObject != transform.parent.gameObject) {
			SelfDestruct();
		} else if (other.tag == "Bullet" || other.tag == "BigBullet") {
			Absorb(other.transform.localScale.x / 3);
			if (other.tag == "Bullet") {
				other.GetComponent<Bullet>().SelfDestruct();
			} else {
				other.GetComponent<BigBullet>().SelfDestruct();
			}
		}
	}

	public void Shrink() {
		StartCoroutine(ShowGrowth(-1 * SHRINK_AMOUNT));
	}

	public void Absorb(float size) {
		StartCoroutine(ShowGrowth(size));
	}

	private IEnumerator ShowGrowth(float amount) {
		Vector3 tempScale; 
		float delta;
		float goal = transform.localScale.x + amount;
		for(float i = 0; i < GROWTH_TIME; i += Time.deltaTime) {
			tempScale = transform.localScale;
			delta = (Time.deltaTime / GROWTH_TIME) * amount;
			tempScale.x += delta;
			tempScale.y += delta;
			transform.localScale = tempScale;
			yield return null;
		}
	}
}
