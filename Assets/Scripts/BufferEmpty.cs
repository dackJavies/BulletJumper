using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferEmpty : BufferNode
{
    public override BufferNode GetNext()
    {
		return null;
    }

    public override bool IsEmpty()
    {
  		return true;
    }
}
