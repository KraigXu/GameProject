using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	
	public struct IntVec2 : IEquatable<IntVec2>
	{
		
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004A06 File Offset: 0x00002C06
		public bool IsInvalid
		{
			get
			{
				return this.x < -500;
			}
		}

		
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00004A15 File Offset: 0x00002C15
		public bool IsValid
		{
			get
			{
				return this.x >= -500;
			}
		}

		
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004A27 File Offset: 0x00002C27
		public static IntVec2 Zero
		{
			get
			{
				return new IntVec2(0, 0);
			}
		}

		
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00004A30 File Offset: 0x00002C30
		public static IntVec2 One
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00004A39 File Offset: 0x00002C39
		public static IntVec2 Two
		{
			get
			{
				return new IntVec2(2, 2);
			}
		}

		
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00004A42 File Offset: 0x00002C42
		public static IntVec2 North
		{
			get
			{
				return new IntVec2(0, 1);
			}
		}

		
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00004A4B File Offset: 0x00002C4B
		public static IntVec2 East
		{
			get
			{
				return new IntVec2(1, 0);
			}
		}

		
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00004A54 File Offset: 0x00002C54
		public static IntVec2 South
		{
			get
			{
				return new IntVec2(0, -1);
			}
		}

		
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00004A5D File Offset: 0x00002C5D
		public static IntVec2 West
		{
			get
			{
				return new IntVec2(-1, 0);
			}
		}

		
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00004A66 File Offset: 0x00002C66
		public float Magnitude
		{
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00004A89 File Offset: 0x00002C89
		public int MagnitudeManhattan
		{
			get
			{
				return Mathf.Abs(this.x) + Mathf.Abs(this.z);
			}
		}

		
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00004AA2 File Offset: 0x00002CA2
		public int Area
		{
			get
			{
				return Mathf.Abs(this.x) * Mathf.Abs(this.z);
			}
		}

		
		public IntVec2(int newX, int newZ)
		{
			this.x = newX;
			this.z = newZ;
		}

		
		public IntVec2(Vector2 v2)
		{
			this.x = (int)v2.x;
			this.z = (int)v2.y;
		}

		
		public Vector2 ToVector2()
		{
			return new Vector2((float)this.x, (float)this.z);
		}

		
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, 0f, (float)this.z);
		}

		
		public IntVec2 Rotated()
		{
			return new IntVec2(this.z, this.x);
		}

		
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

		
		public string ToStringCross()
		{
			return this.x.ToString() + " x " + this.z.ToString();
		}

		
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

		
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00004C00 File Offset: 0x00002E00
		public static IntVec2 Invalid
		{
			get
			{
				return new IntVec2(-1000, -1000);
			}
		}

		
		public Vector2 ToVector2Shifted()
		{
			return new Vector2((float)this.x + 0.5f, (float)this.z + 0.5f);
		}

		
		public static IntVec2 operator +(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x + b.x, a.z + b.z);
		}

		
		public static IntVec2 operator -(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x - b.x, a.z - b.z);
		}

		
		public static IntVec2 operator *(IntVec2 a, int b)
		{
			return new IntVec2(a.x * b, a.z * b);
		}

		
		public static IntVec2 operator /(IntVec2 a, int b)
		{
			return new IntVec2(a.x / b, a.z / b);
		}

		
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00004CA2 File Offset: 0x00002EA2
		public IntVec3 ToIntVec3
		{
			get
			{
				return new IntVec3(this.x, 0, this.z);
			}
		}

		
		public static bool operator ==(IntVec2 a, IntVec2 b)
		{
			return a.x == b.x && a.z == b.z;
		}

		
		public static bool operator !=(IntVec2 a, IntVec2 b)
		{
			return a.x != b.x || a.z != b.z;
		}

		
		public override bool Equals(object obj)
		{
			return obj is IntVec2 && this.Equals((IntVec2)obj);
		}

		
		public bool Equals(IntVec2 other)
		{
			return this.x == other.x && this.z == other.z;
		}

		
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.x, this.z);
		}

		
		public int x;

		
		public int z;
	}
}
