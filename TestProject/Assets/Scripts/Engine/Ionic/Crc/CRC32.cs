using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ionic.Crc
{
	// Token: 0x020012C4 RID: 4804
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000C")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class CRC32
	{
		// Token: 0x17001353 RID: 4947
		// (get) Token: 0x060071E7 RID: 29159 RVA: 0x0027DB94 File Offset: 0x0027BD94
		public long TotalBytesRead
		{
			get
			{
				return this._TotalBytesRead;
			}
		}

		// Token: 0x17001354 RID: 4948
		// (get) Token: 0x060071E8 RID: 29160 RVA: 0x0027DB9C File Offset: 0x0027BD9C
		public int Crc32Result
		{
			get
			{
				return (int)(~(int)this._register);
			}
		}

		// Token: 0x060071E9 RID: 29161 RVA: 0x0027DBA5 File Offset: 0x0027BDA5
		public int GetCrc32(Stream input)
		{
			return this.GetCrc32AndCopy(input, null);
		}

		// Token: 0x060071EA RID: 29162 RVA: 0x0027DBB0 File Offset: 0x0027BDB0
		public int GetCrc32AndCopy(Stream input, Stream output)
		{
			if (input == null)
			{
				throw new Exception("The input stream must not be null.");
			}
			byte[] array = new byte[8192];
			int count = 8192;
			this._TotalBytesRead = 0L;
			int i = input.Read(array, 0, count);
			if (output != null)
			{
				output.Write(array, 0, i);
			}
			this._TotalBytesRead += (long)i;
			while (i > 0)
			{
				this.SlurpBlock(array, 0, i);
				i = input.Read(array, 0, count);
				if (output != null)
				{
					output.Write(array, 0, i);
				}
				this._TotalBytesRead += (long)i;
			}
			return (int)(~(int)this._register);
		}

		// Token: 0x060071EB RID: 29163 RVA: 0x0027DC44 File Offset: 0x0027BE44
		public int ComputeCrc32(int W, byte B)
		{
			return this._InternalComputeCrc32((uint)W, B);
		}

		// Token: 0x060071EC RID: 29164 RVA: 0x0027DC4E File Offset: 0x0027BE4E
		internal int _InternalComputeCrc32(uint W, byte B)
		{
			return (int)(this.crc32Table[(int)((W ^ (uint)B) & 255u)] ^ W >> 8);
		}

		// Token: 0x060071ED RID: 29165 RVA: 0x0027DC64 File Offset: 0x0027BE64
		public void SlurpBlock(byte[] block, int offset, int count)
		{
			if (block == null)
			{
				throw new Exception("The data buffer must not be null.");
			}
			for (int i = 0; i < count; i++)
			{
				int num = offset + i;
				byte b = block[num];
				if (this.reverseBits)
				{
					uint num2 = this._register >> 24 ^ (uint)b;
					this._register = (this._register << 8 ^ this.crc32Table[(int)num2]);
				}
				else
				{
					uint num3 = (this._register & 255u) ^ (uint)b;
					this._register = (this._register >> 8 ^ this.crc32Table[(int)num3]);
				}
			}
			this._TotalBytesRead += (long)count;
		}

		// Token: 0x060071EE RID: 29166 RVA: 0x0027DCF8 File Offset: 0x0027BEF8
		public void UpdateCRC(byte b)
		{
			if (this.reverseBits)
			{
				uint num = this._register >> 24 ^ (uint)b;
				this._register = (this._register << 8 ^ this.crc32Table[(int)num]);
				return;
			}
			uint num2 = (this._register & 255u) ^ (uint)b;
			this._register = (this._register >> 8 ^ this.crc32Table[(int)num2]);
		}

		// Token: 0x060071EF RID: 29167 RVA: 0x0027DD58 File Offset: 0x0027BF58
		public void UpdateCRC(byte b, int n)
		{
			while (n-- > 0)
			{
				if (this.reverseBits)
				{
					uint num = this._register >> 24 ^ (uint)b;
					this._register = (this._register << 8 ^ this.crc32Table[(int)((num >= 0u) ? num : (num + 256u))]);
				}
				else
				{
					uint num2 = (this._register & 255u) ^ (uint)b;
					this._register = (this._register >> 8 ^ this.crc32Table[(int)((num2 >= 0u) ? num2 : (num2 + 256u))]);
				}
			}
		}

		// Token: 0x060071F0 RID: 29168 RVA: 0x0027DDE0 File Offset: 0x0027BFE0
		private static uint ReverseBits(uint data)
		{
			uint num = (data & 1431655765u) << 1 | (data >> 1 & 1431655765u);
			num = ((num & 858993459u) << 2 | (num >> 2 & 858993459u));
			num = ((num & 252645135u) << 4 | (num >> 4 & 252645135u));
			return num << 24 | (num & 65280u) << 8 | (num >> 8 & 65280u) | num >> 24;
		}

		// Token: 0x060071F1 RID: 29169 RVA: 0x0027DE4C File Offset: 0x0027C04C
		private static byte ReverseBits(byte data)
		{
			int num = (int)data * 131586;
			uint num2 = 17055760u;
			uint num3 = (uint)(num & (int)num2);
			uint num4 = (uint)(num << 2 & (int)((int)num2 << 1));
			return (byte)(16781313u * (num3 + num4) >> 24);
		}

		// Token: 0x060071F2 RID: 29170 RVA: 0x0027DE80 File Offset: 0x0027C080
		private void GenerateLookupTable()
		{
			this.crc32Table = new uint[256];
			byte b = 0;
			do
			{
				uint num = (uint)b;
				for (byte b2 = 8; b2 > 0; b2 -= 1)
				{
					if ((num & 1u) == 1u)
					{
						num = (num >> 1 ^ this.dwPolynomial);
					}
					else
					{
						num >>= 1;
					}
				}
				if (this.reverseBits)
				{
					this.crc32Table[(int)CRC32.ReverseBits(b)] = CRC32.ReverseBits(num);
				}
				else
				{
					this.crc32Table[(int)b] = num;
				}
				b += 1;
			}
			while (b != 0);
		}

		// Token: 0x060071F3 RID: 29171 RVA: 0x0027DEF4 File Offset: 0x0027C0F4
		private uint gf2_matrix_times(uint[] matrix, uint vec)
		{
			uint num = 0u;
			int num2 = 0;
			while (vec != 0u)
			{
				if ((vec & 1u) == 1u)
				{
					num ^= matrix[num2];
				}
				vec >>= 1;
				num2++;
			}
			return num;
		}

		// Token: 0x060071F4 RID: 29172 RVA: 0x0027DF20 File Offset: 0x0027C120
		private void gf2_matrix_square(uint[] square, uint[] mat)
		{
			for (int i = 0; i < 32; i++)
			{
				square[i] = this.gf2_matrix_times(mat, mat[i]);
			}
		}

		// Token: 0x060071F5 RID: 29173 RVA: 0x0027DF48 File Offset: 0x0027C148
		public void Combine(int crc, int length)
		{
			uint[] array = new uint[32];
			uint[] array2 = new uint[32];
			if (length == 0)
			{
				return;
			}
			uint num = ~this._register;
			array2[0] = this.dwPolynomial;
			uint num2 = 1u;
			for (int i = 1; i < 32; i++)
			{
				array2[i] = num2;
				num2 <<= 1;
			}
			this.gf2_matrix_square(array, array2);
			this.gf2_matrix_square(array2, array);
			uint num3 = (uint)length;
			do
			{
				this.gf2_matrix_square(array, array2);
				if ((num3 & 1u) == 1u)
				{
					num = this.gf2_matrix_times(array, num);
				}
				num3 >>= 1;
				if (num3 == 0u)
				{
					break;
				}
				this.gf2_matrix_square(array2, array);
				if ((num3 & 1u) == 1u)
				{
					num = this.gf2_matrix_times(array2, num);
				}
				num3 >>= 1;
			}
			while (num3 != 0u);
			num ^= (uint)crc;
			this._register = ~num;
		}

		// Token: 0x060071F6 RID: 29174 RVA: 0x0027DFFF File Offset: 0x0027C1FF
		public CRC32() : this(false)
		{
		}

		// Token: 0x060071F7 RID: 29175 RVA: 0x0027E008 File Offset: 0x0027C208
		public CRC32(bool reverseBits) : this(-306674912, reverseBits)
		{
		}

		// Token: 0x060071F8 RID: 29176 RVA: 0x0027E016 File Offset: 0x0027C216
		public CRC32(int polynomial, bool reverseBits)
		{
			this.reverseBits = reverseBits;
			this.dwPolynomial = (uint)polynomial;
			this.GenerateLookupTable();
		}

		// Token: 0x060071F9 RID: 29177 RVA: 0x0027E039 File Offset: 0x0027C239
		public void Reset()
		{
			this._register = uint.MaxValue;
		}

		// Token: 0x0400467F RID: 18047
		private uint dwPolynomial;

		// Token: 0x04004680 RID: 18048
		private long _TotalBytesRead;

		// Token: 0x04004681 RID: 18049
		private bool reverseBits;

		// Token: 0x04004682 RID: 18050
		private uint[] crc32Table;

		// Token: 0x04004683 RID: 18051
		private const int BUFFER_SIZE = 8192;

		// Token: 0x04004684 RID: 18052
		private uint _register = uint.MaxValue;
	}
}
