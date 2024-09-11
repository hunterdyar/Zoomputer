using UnityEngine;
using Zoompy;


[Logic(Path = "Arithmetic/2 Bit Adder")]
public class AdderLogic : Logic
{
	private ZSystem _componentSystem;
	// public void OnAnyInputChange()
	// {
	// 	//3 inputs, bit A, B, and Carry.
	// 	//two outputs: bit and carry.
	// 	//0+0 = carry
	// 	//0+1, 1+0 = 1+carry
	// 	int total = _componentSystem.Inputs[0].GetSignalByte() + _componentSystem.Inputs[1].GetSignalByte() + _componentSystem.Inputs[2].GetSignalByte();
	// 	switch (total)
	// 	{
	// 		case 0:
	// 			_componentSystem.Outputs[0].SetSignalByte(0);
	// 			_componentSystem.Outputs[1].SetSignalByte(0);
	// 			break;
	// 		case 1:
	// 			_componentSystem.Outputs[0].SetSignalByte(1);
	// 			_componentSystem.Outputs[1].SetSignalByte(0);
	// 			break;
	// 		case 2:
	// 			_componentSystem.Outputs[0].SetSignalByte(0);
	// 			_componentSystem.Outputs[1].SetSignalByte(1);
	// 			break;
	// 		case 3:
	// 			_componentSystem.Outputs[0].SetSignalByte(1);
	// 			_componentSystem.Outputs[1].SetSignalByte(1);
	// 			break;
	// 		default:
	// 			Debug.Log("Error in Adder Input");
	// 			_componentSystem.Outputs[0].SetSignalByte(1);
	// 			_componentSystem.Outputs[1].SetSignalByte(1);
	// 			break;
	// 	}
	// }


	public override void OnInputChange(ZConnection c, byte d)
	{
		throw new System.NotImplementedException();
	}
}
