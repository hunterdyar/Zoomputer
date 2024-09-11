using System.IO;
using UnityEngine;
using Zoompy;

[Logic(Path="Gate/Xor")]
public class XorLogic : Logic
{


		public override void OnInputChange(ZConnection c, byte d)
		{
			var a = _hub.Get(_system.Inputs[0]) > 0;
			var b = _hub.Get(_system.Inputs[1]) > 0;
			_hub.SetConnection(_system.Outputs[0], a ^ b, true);
		}
}
