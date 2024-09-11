using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zoompy;

[Logic(Path = "Gate/Not")]
public class NotLogic : Logic
{
	public override void OnInputChange(ZConnection c, byte d)
	{
		_hub.SetConnection(_system.Outputs[0], d == 1 ? (byte)0 : (byte)1);
	}
}

