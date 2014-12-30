using System;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public static class UnicodeConvert
	{
		public static string ToNonPrintableString(string input)
		{
			var chars = input.Select(GetBase3Bytes).SelectMany(b => b)
				.Select(ToNonPrintableUnicodeChar).ToArray();
			return new string(chars);
		}

		private static char ToNonPrintableUnicodeChar(byte b)
		{
			return (char) (b + '\u200b');
		}

		private static char FromNonPrintableUnicodeChar(char c)
		{
			return (char) (c - '\u200b');
		}

		private static IEnumerable<byte> GetBase3Bytes(char c)
		{
			if (c > 0xFF)
			{
				throw new ArgumentOutOfRangeException();
			}
			int num = (byte) c;
			for (int i = 0; i < 5; i++)
			{
				yield return (byte)(num%3);

				num /= 3;
			}
		}

		public static string FromNonPrintableString(string input)
		{
			return new String(ConvertCharacters(input).ToArray());
		}

		private static IEnumerable<char> ConvertCharacters(IEnumerable<char> input)
		{
			bool hasMore = true;
			var enumerator = input.GetEnumerator();
			do
			{
				int result = 0;
				int multiplier = 1;
				for (int i = 0; i < 5; i++)
				{
					hasMore = enumerator.MoveNext();
					if (!hasMore)
					{
						break;
					}
					var c = enumerator.Current;
					if (!IsNonPrintableUnicodeChar(c))
					{
						hasMore = false;
						break;
					}
					c = FromNonPrintableUnicodeChar(c);
					result += c*multiplier;
					multiplier *= 3;
				}
				if (result > 0)
				{
					yield return (char) result;
				}
			} while (hasMore);
		}

		private static bool IsNonPrintableUnicodeChar(char c)
		{
			return c >= 0x200b && c <= 0x200d;
		}
	}
}
