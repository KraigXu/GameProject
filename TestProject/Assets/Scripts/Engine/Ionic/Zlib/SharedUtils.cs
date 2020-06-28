using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x020012BB RID: 4795
	internal class SharedUtils
	{
		// Token: 0x06007192 RID: 29074 RVA: 0x0027C3FA File Offset: 0x0027A5FA
		public static int URShift(int number, int bits)
		{
			return (int)((uint)number >> bits);
		}

		// Token: 0x06007193 RID: 29075 RVA: 0x0027C404 File Offset: 0x0027A604
		public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			char[] array = new char[target.Length];
			int num = sourceTextReader.Read(array, start, count);
			if (num == 0)
			{
				return -1;
			}
			for (int i = start; i < start + num; i++)
			{
				target[i] = (byte)array[i];
			}
			return num;
		}

		// Token: 0x06007194 RID: 29076 RVA: 0x0027C445 File Offset: 0x0027A645
		internal static byte[] ToByteArray(string sourceString)
		{
			return Encoding.UTF8.GetBytes(sourceString);
		}

		// Token: 0x06007195 RID: 29077 RVA: 0x0027C452 File Offset: 0x0027A652
		internal static char[] ToCharArray(byte[] byteArray)
		{
			return Encoding.UTF8.GetChars(byteArray);
		}
	}
}
