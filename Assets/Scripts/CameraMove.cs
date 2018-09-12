using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	private const float PLAYER_BOX_WIDTH = 2.0f;
	private const float PLAYER_BOX_HEIGHT = 1.0f;
	private const float CHECK_RATE = 0.1f;
	private const float SLOW_RATE = 2.0f;
	private const float SPEED_DAMPENER = 2.5f;
	private const float RUMBLE_DURATION = 0.1f;
	private const float RUMBLE_LOW = -0.15f;
	private const float RUMBLE_HIGH = 0.15f;

	private Vector3 preRumblePosition;

	private GameObject thePlayer;
	private Rigidbody2D RB;
	private Camera myCamera;

	void Start() {
		thePlayer = GameObject.Find("Player");
		RB = GetComponent<Rigidbody2D>();
		myCamera = GetComponent<Camera>();
		StartCoroutine(CheckCycle());
	}

	private IEnumerator CheckCycle() {
		for(float i = 0; i < CHECK_RATE; i += Time.deltaTime) {
			yield return null;
		}
		if (PlayerOutOfBox()) {
			FloatToPlayer();
		} else {
			RB.velocity /= SLOW_RATE;
		}
		StartCoroutine(CheckCycle());
	}

	private bool PlayerOutOfBox() {
		return (Mathf.Abs(transform.position.x - thePlayer.transform.position.x) > (PLAYER_BOX_WIDTH / 2))
			|| (Mathf.Abs(transform.position.y - thePlayer.transform.position.y) > (PLAYER_BOX_HEIGHT / 2));
	}

	private void FloatToPlayer() {
		Vector2 floatVector = thePlayer.transform.position - transform.position;
		RB.velocity = floatVector * myCamera.orthographicSize / SPEED_DAMPENER;
	}

	public IEnumerator Rumble() {
		preRumblePosition = transform.position;
		for(float i = 0; i < RUMBLE_DURATION; i += Time.deltaTime) {
			transform.position = GetNextRumblePosition();
			yield return null;
		}
		transform.position = preRumblePosition;
	}

	private Vector3 GetNextRumblePosition() {
		return new Vector3(
			transform.position.x + Random.Range(RUMBLE_LOW, RUMBLE_HIGH),
			transform.position.y + Random.Range(RUMBLE_LOW, RUMBLE_HIGH),
			transform.position.z
		);
	}

}
