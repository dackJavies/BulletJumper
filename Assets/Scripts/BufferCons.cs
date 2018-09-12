using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferCons : BufferNode {

	public BufferCons(char val, BufferNode next) {
		Val = val;
		Next = next;
	}

    public override BufferNode GetNext()
    {
		return Next;
    }

    public override bool IsEmpty()
    {
		return false;
    }

}
