using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BufferNode : MonoBehaviour {

	public char Val;
	protected BufferNode Next;

	// Use this for initialization
	protected void Start () {
		Val = '\0';
		Next = null;
	}

	public abstract bool IsEmpty();

	public abstract BufferNode GetNext();

}
