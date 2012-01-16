namespace Rogue.MetroFire.CampfireClient
{
	public class ConnectivityState
	{
		public bool WithProxy { get; private set; }
		public bool WithoutProxy { get; private set; }

		public ConnectivityState(bool withProxy, bool withoutProxy)
		{
			WithProxy = withProxy;
			WithoutProxy = withoutProxy;
		}
	}
}