namespace Zoompy
{
	public interface ISignalHook
	{
		public void OnAnyInputChange();
		public void OnInputChange(int index, byte data);
		void SetComponenSystem(ComponentSystem parent);
	}
}