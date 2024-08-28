namespace Zoompy
{
	public interface IComponentInteractor
	{
		public void Configure(ComponentSystem cs);
		public void Interact();
	}
}