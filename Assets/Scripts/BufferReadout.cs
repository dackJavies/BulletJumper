using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferReadout : MonoBehaviour {

	private static TextMesh TM;

	void Start () {
		TM = GetComponent<TextMesh>();
	}

	public void ShowCurrentBuffer(BufferNode head) {
		BufferNode current = head;
		string readout = "";
//		while(!current.IsEmpty()) {
		Debug.Log(Buffer.length);
		for(int i = 0; i < Buffer.length; i++) {
			if (readout != "") {
				readout += ", ";
			}
			readout += current.Val;
			current = head.GetNext();
		}
		TM.text = readout;
	}
	
}
