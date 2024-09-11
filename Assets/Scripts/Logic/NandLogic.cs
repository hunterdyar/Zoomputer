using System;
using System.IO;
using UnityEngine;
using Zoompy;

[Logic(Path="Gate/Nand")]
public class NandLogic : Logic
{
    public override void OnInputChange(ZConnection c, byte d)
    {
	    var a = _hub.Get(_system.Inputs[0]);
	    var b = _hub.Get(_system.Inputs[1]);
	    _hub.SetConnection(_system.Outputs[0], a > 0 && b > 0 ? (byte)0 : (byte)1);
    }
}
