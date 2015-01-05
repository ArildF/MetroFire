using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Rogue.MetroFire.UI.Infrastructure;

namespace MetroFire.Specs
{
	class UnicodeConvertTests
	{
		[Test]
		public void Converting_produces_only_nonprintable_characters()
		{
			const string str = "!!L=";
			var converted = UnicodeConvert.ToNonPrintableString(str);
			var formatted = String.Join("", 
				converted.Select(c => "\\u" + ((int)c).ToString("X4")));
			converted.All(c => c >= '\u200b' && c <= '\u200d').Should().BeTrue();
		}

		[Test]
		public void Conversion_roundtrips()
		{
			const string str = "abcdf";
			var converted = UnicodeConvert.ToNonPrintableString(str);
			var deconverted = UnicodeConvert.FromNonPrintableString(converted);

			deconverted.Should().Be(str);
		}


		[Test]
		public void Converting_stops_when_encountering_normal_characters()
		{
			const string str = "abcdf";
			var converted = UnicodeConvert.ToNonPrintableString(str);
			var actualString = converted + "ia ia oh";

			var deconverted = UnicodeConvert.FromNonPrintableString(actualString);
			deconverted.Should().Be(str);
		}
	}
}
