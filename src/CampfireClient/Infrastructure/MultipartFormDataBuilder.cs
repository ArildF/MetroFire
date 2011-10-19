using System;
using System.IO;
using System.Text;

namespace Rogue.MetroFire.CampfireClient.Infrastructure
{
	public class MultipartFormDataBuilder
	{
		private Stream _requestStream;
		private readonly string _boundary;

		public MultipartFormDataBuilder()
		{
			_boundary = new String('-', 42) + DateTime.Now.Ticks.ToString("XXX");
		}

		public void SetRequestStream(Stream requestStream)
		{
			_requestStream = requestStream;
		}

		public string Boundary
		{
			get { return _boundary; }
		}

		public void WriteStream(Stream inputStream, string name, string fileName, string contentType)
		{
			WriteLine("--" + Boundary);
			WriteLine(String.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", name, fileName));
			WriteLine("Content-Type: " + contentType);
			WriteLine("");

			var buf = new byte[1024];

			int read;
			while ((read = inputStream.Read(buf, 0, buf.Length)) > 0)
			{
				_requestStream.Write(buf, 0, read);
			}
			WriteLine("");

			WriteLine("--" + Boundary);

			_requestStream.Flush();
		}

		private void WriteLine(string format)
		{
			var bytes = Encoding.UTF8.GetBytes(format + Environment.NewLine);
			_requestStream.Write(bytes, 0, bytes.Length);
		}
	}
}
