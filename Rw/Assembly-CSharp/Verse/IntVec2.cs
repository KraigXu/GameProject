using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200000E RID: 14
	public struct IntVec2 : IEquatable<IntVec2>
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004A06 File Offset: 0x00002C06
		public bool IsInvalid
		{
			get
			{
				return this.x < -500;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00004A15 File Offset: 0x00002C15
		public bool IsValid
		{
			get
			{
				return this.x >= -500;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004A27 File Offset: 0x00002C27
		public static IntVec2 Zero
		{
			get
			{
				return new IntVec2(0, 0);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00004A30 File Offset: 0x00002C30
		public static IntVec2 One
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00004A39 File Offset: 0x00002C39
		public static IntVec2 Two
		{
			get
			{
				return new IntVec2(2, 2);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00004A42 File Offset: 0x00002C42
		public static IntVec2 North
		{
			get
			{
				return new IntVec2(0, 1);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00004A4B File Offset: 0x00002C4B
		public static IntVec2 East
		{
			get
			{
				return new IntVec2(1, 0);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00004A54 File Offset: 0x00002C54
		public static IntVec2 South
		{
			get
			{
				return new IntVec2(0, -1);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00004A5D File Offset: 0x00002C5D
		public static IntVec2 West
		{
			get
			{
				return new IntVec2(-1, 0);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00004A66 File Offset: 0x00002C66
		public float Magnitude
		{
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00004A89 File Offset: 0x00002C89
		public int MagnitudeManhattan
		{
			get
			{
				return Mathf.Abs(this.x) + Mathf.Abs(this.z);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00004AA2 File Offset: 0x00002CA2
		public int Area
		{
			get
			{
				return Mathf.Abs(this.x) * Mathf.Abs(this.z);
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004ABB File Offset: 0x00002CBB
		public IntVec2(int newX, int newZ)
		{
			this.x = newX;
			this.z = newZ;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004ACB File Offset: 0x00002CCB
		public IntVec2(Vector2 v2)
		{
			this.x = (int)v2.x;
			this.z = (int)v2.y;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004AE7 File Offset: 0x00002CE7
		public Vector2 ToVector2()
		{
			return new Vector2((float)this.x, (float)this.z);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00004AFC File Offset: 0x00002CFC
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, 0f, (float)this.z);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004B16 File Offset: 0x00002D16
		public IntVec2 Rotated()
		{
			return new IntVec2(this.z, this.x);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004B2C File Offset: 0x00002D2C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(),
				", ",
				this.z.ToString(),
				")"
			});
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004B78 File Offset: 0x00002D78
		public string ToStringCross()
		{
			return this.x.ToString() + " x " + this.z.ToString();
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004B9C File Offset: 0x00002D9C
		public static IntVec2 FromString(string str)
		{
			str = str.TrimStart(new char[]
			{
				'('
			});
			str = str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = str.Split(new char[]
			{
				','
			});
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			int newX = Convert.ToInt32(array[0], invariantCulture);
			int newZ = Convert.ToInt32(array[1], invariantCulture);
			return new IntVec2(newX, newZ);
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00004C00 File Offset: 0x00002E00
		public static IntVec2 Invalid
		{
			get
			{
				return new IntVec2(-1000, -1000);
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004C11 File Offset: 0x00002E11
		public Vector2 ToVector2Shifted()
		{
			return new Vector2((float)this.x + 0.5f, (float)this.z + 0.5f);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004C32 File Offset: 0x00002E32
		public static IntVec2 operator +(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x + b.x, a.z + b.z);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004C53 File Offset: 0x00002E53
		public static IntVec2 operator -(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x - b.x, a.z - b.z);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004C74 File Offset: 0x00002E74
		public static IntVec2 operator *(IntVec2 a, int b)
		{
			return new IntVec2(a.x * b, a.z * b);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004C8B File Offset: 0x00002E8B
		public static IntVec2 operator /(IntVec2 a, int b)
		{
			return new IntVec2(a.x / b, a.z / b);
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00004CA2 File Offset: 0x00002EA2
		public IntVec3 ToIntVec3
		{
			get
			{
				return new IntVec3(this.x, 0, this.z);
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004CB6 File Offset: 0x00002EB6
		public static bool operator ==(IntVec2 a, IntVec2 b)
		{
			return a.x == b.x && a.z == b.z;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004CD7 File Offset: 0x00002ED7
		public static bool operator !=(IntVec2 a, IntVec2 b)
		{
			return a.x != b.x || a.z != b.z;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004CF8 File Offset: 0x00002EF8
		public override bool Equals(object obj)
		{
			return obj is IntVec2 && this.Equals((IntVec2)obj);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004D10 File Offset: 0x00002F10
		public bool Equals(IntVec2 other)
		{
			return this.x == other.x && this.z == other.z;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004D30 File Offset: 0x00002F30
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.x, this.z);
		}

		// Token: 0x0400002C RID: 44
		public int x;

		// Token: 0x0400002D RID: 45
		public int z;
	}
}
