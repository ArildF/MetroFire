using System;

namespace Rogue.MetroFire.CampfireClient
{
	public class ConnectionState
	{
		public bool Connected { get; private set; }
		public Exception Exception { get; set; }
		public int RoomId { get; private set; }

		public ConnectionState(int roomId, bool connected, Exception exception = null)
		{
			RoomId = roomId;
			Connected = connected;
			Exception = exception;
		}
	}
}
