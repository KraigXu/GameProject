using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000023 RID: 35
	public static class GenString
	{
		// Token: 0x0600024A RID: 586 RVA: 0x0000B33C File Offset: 0x0000953C
		static GenString()
		{
			for (int i = 0; i < 10000; i++)
			{
				GenString.numberStrings[i] = (i - 5000).ToString();
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000B37E File Offset: 0x0000957E
		public static string ToStringCached(this int num)
		{
			if (num < -4999)
			{
				return num.ToString();
			}
			if (num > 4999)
			{
				return num.ToString();
			}
			return GenString.numberStrings[num + 5000];
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000B3AD File Offset: 0x000095AD
		public static IEnumerable<string> SplitBy(this string str, int chunkLength)
		{
			if (str.NullOrEmpty())
			{
				yield break;
			}
			if (chunkLength < 1)
			{
				throw new ArgumentException();
			}
			for (int i = 0; i < str.Length; i += chunkLength)
			{
				if (chunkLength > str.Length - i)
				{
					chunkLength = str.Length - i;
				}
				yield return str.Substring(i, chunkLength);
			}
			yield break;
		}

		// Token: 0x04000058 RID: 88
		private static string[] numberStrings = new string[10000];
	}
}
