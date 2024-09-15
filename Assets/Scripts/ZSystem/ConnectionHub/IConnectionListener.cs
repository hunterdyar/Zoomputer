namespace Zoompy
{
	public interface IConnectionListener
	{
		//connection gets passed along despite being required for listener to register, since a listener (e.g. node) may have multiple connections.
		public void OnConnectionDidChange(ZConnection connection, byte data);
	}
}