using Zoompy.Generator;

namespace Zoompy
{
	public interface ISignalHook
	{
		public void ApplyConfiguration(ComponentSystem cs, GenerationSettings genSettings){}
		public void OnAnyInputChange();
		public void OnInputChange(int index, byte data);
		void SetComponenSystem(ComponentSystem parent);
	}
}