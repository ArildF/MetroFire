using System;
using System.Net;
using System.Runtime.Serialization;

namespace Rogue.MetroFire.CampfireClient
{
	[Serializable]
	public class StreamingDisconnectedException : WebException
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public StreamingDisconnectedException()
		{
		}

		public StreamingDisconnectedException(string message) : base(message)
		{
		}

		public StreamingDisconnectedException(string message, Exception inner) : base(message, inner)
		{
		}

		protected StreamingDisconnectedException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}
