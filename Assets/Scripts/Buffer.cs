using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : MonoBehaviour {

	private const float LIFESPAN = 0.3f;
	private bool restartTimer;
	private bool timerGoing;
	public static int length;

	private BufferNode Head;

	void Start() {
		Head = new BufferEmpty();
		restartTimer = false;
		timerGoing = false;
		length = 0;
	}

	private IEnumerator ClearNext() {
		timerGoing = true;
		for(float i = 0; i < LIFESPAN; i += Time.deltaTime) {
			if (restartTimer) {
				restartTimer = false;
				StartCoroutine(ClearNext());
				yield break;
			}
			yield return null;
		}
		if (!Head.IsEmpty()) {
			Head = Head.GetNext();
			length -= 1;
			StartCoroutine(ClearNext());
		} else {
			timerGoing = false;
		}
	}

	public void AddCons(char val) {
		/*if (val == Head.Val) {
			return;
		}*/
		Head = new BufferCons(val, Head);
		length += 1;
		if (timerGoing) {
			restartTimer = true;
		} else {
			StartCoroutine(ClearNext());
		}
	}

	public char GetNext() {
		if (!Head.IsEmpty()) {
			return Head.Val;
		} else {
			return '\0';
		}
	}

	public BufferNode GetCurrentBuffer() {
		return Head;
	}

}
