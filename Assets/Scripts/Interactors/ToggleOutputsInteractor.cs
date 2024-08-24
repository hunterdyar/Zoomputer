
	using UnityEngine;
	using Zoompy;
	using Zoompy.Interactors;

	[Interactor(Path = "Toggle Outputs")]
	public class ToggleOutputsInteractor : MonoBehaviour, IComponentInteractor
	{
		private ComponentSystem _cs;
		public void Configure(ComponentSystem cs)
		{
			_cs = cs;
		}

		public void Interact()
		{
			foreach (var port in _cs.Outputs)
			{
				port.SetSignal(!port.GetSignal());
			}
		}
	}
