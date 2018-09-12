using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

	public Vector2 CannonballDirection;
	public GameObject bullet;
	public float[] rhythm;
	private bool cannonActive;
	private bool turnOffSignal;
	private bool onCamera;
	private const float TURN_OFF_TIMER = 10.0f;

	void Start() {
		cannonActive = false;
		turnOffSignal = false;
		onCamera = false;
		if (CannonballDirection == null || CannonballDirection == Vector2.zero) {
			Debug.Log("Direction of cannon not set properly.");
		}
		if (rhythm.Length == 0) {
			Debug.Log("Cannon must have a rhythm.");
		}
		StartCoroutine(Fire());
	}

	private IEnumerator Fire() {
		GameObject nextBullet;
		int i = 0;
		while(true) {
			if (turnOffSignal) {
				turnOffSignal = false;
				yield break;
			}
			if (i >= rhythm.Length) {
				i = 0;
			}
			for(float j = 0; j < rhythm[i]; j += Time.deltaTime) {
				yield return null;
			}
			nextBullet = Object.Instantiate(bullet, transform.position, transform.rotation);
			nextBullet.transform.parent = this.transform;
			nextBullet.GetComponent<Bullet>().direction = this.CannonballDirection;
			i++;
		}
	}

	private IEnumerator TurnOff() {
		for(float i = 0; i < TURN_OFF_TIMER; i += Time.deltaTime) {
			if (onCamera) {
				yield break;
			}
			yield return null;
		}
		turnOffSignal = true;
	}

/*	void OnBecameVisible() {
		if (!cannonActive) {
			StartCoroutine(Fire());
		}
		cannonActive = true;
		onCamera = true;
	}

	void OnBecameInvisible() {
		onCamera = false;
		if (cannonActive) {
			StartCoroutine(TurnOff());		
		}
	}*/

	public void RespawnBullets() {
		Bullet cur;
		GameObject curGO;
		for(int i = 0; i < transform.childCount; i++) {
			curGO = transform.GetChild(i).gameObject;
			cur = curGO.GetComponent<Bullet>();
			if (cur != null) {
				cur.Respawn();
			}
		}
	}

}
