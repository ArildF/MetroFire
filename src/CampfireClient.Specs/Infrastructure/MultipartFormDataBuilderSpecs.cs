using System;
using System.IO;
using System.Text;
using Machine.Specifications;
using Rogue.MetroFire.CampfireClient.Infrastructure;

namespace Rogue.MetroFire.CampfireClient.Specs.Infrastructure
{
	[Subject(typeof(MultipartFormDataBuilder))]
	public class When_building_request_with_stream
	{
		Establish context = () =>
			{
				_requestStream = new MemoryStream();
				_builder = new MultipartFormDataBuilder();
				_builder.SetRequestStream(_requestStream);

				_inputStream = new MemoryStream(Encoding.UTF8.GetBytes("Hello world"));
			};

		Because of = () => _builder.WriteStream(_inputStream, "upload", "HelloWorld.txt", "text/plain");

		It starts_with_two_dashes_and_the_boundary = () => GetRequestLines()[0].ShouldEqual("--" + _builder.Boundary);

		It should_have_a_content_disposition_header =
			() => GetRequestLines()[1].ShouldEqual("Content-Disposition: form-data; name=\"upload\"; filename=\"HelloWorld.txt\"");

		It should_have_a_content_type = () => GetRequestLines()[2].ShouldEqual("Content-Type: text/plain");

		It should_contain_the_file_contents = () => GetRequestLines()[4].ShouldEqual("Hello world");

		It should_end_with_the_boundary = () => GetRequestLines()[5].ShouldEqual("--" + _builder.Boundary);



		It should_be_a_valid_multipart_form_data_request = () => GetRequest().ShouldNotEqual("");



		private static string[] GetRequestLines()
		{
			return GetRequest().Split(new[] {Environment.NewLine}, StringSplitOptions.None);
		}

		private static string GetRequest()
		{
			_requestStream.Seek(0, SeekOrigin.Begin);
			return Encoding.UTF8.GetString(_requestStream.ToArray());
		}

		private static MemoryStream _requestStream;
		private static MultipartFormDataBuilder _builder;
		private static MemoryStream _inputStream;
	}
}
