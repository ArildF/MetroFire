using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Subjects;
using System.Text;
using System.Linq;

namespace Rogue.MetroFire.CampfireClient.Infrastructure
{
	public class MultipartFormDataBuilder
	{
		private readonly string _boundary;
		private readonly List<StreamItem> _streamItems = new List<StreamItem>();

		public MultipartFormDataBuilder()
		{
			_boundary = new String('-', 42) + DateTime.Now.Ticks.ToString("XXX");
		}


		public string Boundary
		{
			get { return _boundary; }
		}

		public long ContentLength
		{
			get { return _streamItems.Sum(si => si.Size); }
		}

		public void AddStream(Stream inputStream, string name, string fileName, string contentType)
		{
			_streamItems.Add(new StreamItem(inputStream, name, fileName, contentType, Boundary));
		}

		public void Write(Stream outputStream, IObserver<ProgressState> progressObserver)
		{
			foreach (var streamItem in _streamItems)
			{
				streamItem.Write(outputStream, progressObserver);
			}
		}

		private class StreamItem
		{
			private readonly Stream _inputStream;
			private readonly byte[] _header;
			private readonly byte[] _footer;

			public StreamItem(Stream inputStream, string name, string fileName, string contentType, string boundary)
			{
				_inputStream = inputStream;

				_header = CreateHeader(name, fileName, contentType, boundary);

				_footer = CreateFooter(boundary);
			}

			public long Size
			{
				get{ return _header.Length + _inputStream.Length + _footer.Length;}
			}

			public void Write(Stream outputStream, IObserver<ProgressState> progressObserver)
			{
				var buf = new byte[4096];
				long total = Size;

				int current = 0;

				progressObserver.OnNext(new ProgressState(total, current));

				outputStream.Write(_header, 0, _header.Length);
				current += _header.Length;

				progressObserver.OnNext(new ProgressState(total, current));

				int read;
				while ((read = _inputStream.Read(buf, 0, buf.Length)) > 0)
				{
					outputStream.Write(buf, 0, read);
					current += read;
					progressObserver.OnNext(new ProgressState(total, current));

				}

				outputStream.Write(_footer, 0, _footer.Length);

				progressObserver.OnCompleted();

				outputStream.Flush();
			}



			private byte[] CreateFooter(string boundary)
			{
				var str = Environment.NewLine + "--" + boundary + Environment.NewLine;
				return Encoding.UTF8.GetBytes(str);
			}

			private byte[] CreateHeader(string name, string fileName, string contentType, string boundary)
			{
				var memoryStream = new MemoryStream();

				WriteLine(memoryStream, "--" + boundary);
				WriteLine(memoryStream, String.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", name, fileName));
				WriteLine(memoryStream, "Content-Type: " + contentType);
				WriteLine(memoryStream, "");

				return memoryStream.ToArray();
			}

			private void WriteLine(Stream stream, string format)
			{
				var bytes = Encoding.UTF8.GetBytes(format + Environment.NewLine);
				stream.Write(bytes, 0, bytes.Length);
			}
		}
	}
}
