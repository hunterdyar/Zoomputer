using UnityEngine;
using Zoompy.LogicImplementations;

namespace Zoompy.ComponentGenerator
{
	[CreateAssetMenu(fileName = "Gneratore", menuName = "Component/Generator", order = 0)]
	public class ComponentGenerator : ScriptableObject
	{
		[Min(0)]
		public int numberInputs;
		[Min(0)]
		public int numberOutputs;
		
		[HideInInspector]
		public string baseLogicClassName;
	}
}