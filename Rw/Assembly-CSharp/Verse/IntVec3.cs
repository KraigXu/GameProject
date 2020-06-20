using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200000F RID: 15
	public struct IntVec3 : IEquatable<IntVec3>
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00004D43 File Offset: 0x00002F43
		public IntVec2 ToIntVec2
		{
			get
			{
				return new IntVec2(this.x, this.z);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00004D56 File Offset: 0x00002F56
		public bool IsValid
		{
			get
			{
				return this.y >= 0;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00004D64 File Offset: 0x00002F64
		public int LengthHorizontalSquared
		{
			get
			{
				return this.x * this.x + this.z * this.z;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00004D81 File Offset: 0x00002F81
		public float LengthHorizontal
		{
			get
			{
				return GenMath.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00004DA4 File Offset: 0x00002FA4
		public int LengthManhattan
		{
			get
			{
				return ((this.x >= 0) ? this.x : (-this.x)) + ((this.z >= 0) ? this.z : (-this.z));
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00004DD8 File Offset: 0x00002FD8
		public float AngleFlat
		{
			get
			{
				if (this.x == 0 && this.z == 0)
				{
					return 0f;
				}
				return Quaternion.LookRotation(this.ToVector3()).eulerAngles.y;
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004E13 File Offset: 0x00003013
		public IntVec3(int newX, int newY, int newZ)
		{
			this.x = newX;
			this.y = newY;
			this.z = newZ;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004E2A File Offset: 0x0000302A
		public IntVec3(Vector3 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.z;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004E4D File Offset: 0x0000304D
		public IntVec3(Vector2 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.y;
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00004E70 File Offset: 0x00003070
		public static IntVec3 Zero
		{
			get
			{
				return new IntVec3(0, 0, 0);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00004E7A File Offset: 0x0000307A
		public static IntVec3 North
		{
			get
			{
				return new IntVec3(0, 0, 1);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00004E84 File Offset: 0x00003084
		public static IntVec3 East
		{
			get
			{
				return new IntVec3(1, 0, 0);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00004E8E File Offset: 0x0000308E
		public static IntVec3 South
		{
			get
			{
				return new IntVec3(0, 0, -1);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00004E98 File Offset: 0x00003098
		public static IntVec3 West
		{
			get
			{
				return new IntVec3(-1, 0, 0);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00004EA2 File Offset: 0x000030A2
		public static IntVec3 NorthWest
		{
			get
			{
				return new IntVec3(-1, 0, 1);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00004EAC File Offset: 0x000030AC
		public static IntVec3 NorthEast
		{
			get
			{
				return new IntVec3(1, 0, 1);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00004EB6 File Offset: 0x000030B6
		public static IntVec3 SouthWest
		{
			get
			{
				return new IntVec3(-1, 0, -1);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00004EC0 File Offset: 0x000030C0
		public static IntVec3 SouthEast
		{
			get
			{
				return new IntVec3(1, 0, -1);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00004ECA File Offset: 0x000030CA
		public static IntVec3 Invalid
		{
			get
			{
				return new IntVec3(-1000, -1000, -1000);
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00004EE0 File Offset: 0x000030E0
		public static IntVec3 FromString(string str)
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
			IntVec3 result;
			try
			{
				CultureInfo invariantCulture = CultureInfo.InvariantCulture;
				int newX = Convert.ToInt32(array[0], invariantCulture);
				int newY = Convert.ToInt32(array[1], invariantCulture);
				int newZ = Convert.ToInt32(array[2], invariantCulture);
				result = new IntVec3(newX, newY, newZ);
			}
			catch (Exception arg)
			{
				Log.Warning(str + " is not a valid IntVec3 format. Exception: " + arg, false);
				result = IntVec3.Invalid;
			}
			return result;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004F88 File Offset: 0x00003188
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, (float)this.y, (float)this.z);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00004FA4 File Offset: 0x000031A4
		public Vector3 ToVector3Shifted()
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y, (float)this.z + 0.5f);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00004FCC File Offset: 0x000031CC
		public Vector3 ToVector3ShiftedWithAltitude(AltitudeLayer AltLayer)
		{
			return this.ToVector3ShiftedWithAltitude(AltLayer.AltitudeFor());
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004FDA File Offset: 0x000031DA
		public Vector3 ToVector3ShiftedWithAltitude(float AddedAltitude)
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y + AddedAltitude, (float)this.z + 0.5f);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005004 File Offset: 0x00003204
		public bool InHorDistOf(IntVec3 otherLoc, float maxDist)
		{
			float num = (float)(this.x - otherLoc.x);
			float num2 = (float)(this.z - otherLoc.z);
			return num * num + num2 * num2 <= maxDist * maxDist;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000503C File Offset: 0x0000323C
		public static IntVec3 FromVector3(Vector3 v)
		{
			return IntVec3.FromVector3(v, 0);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005045 File Offset: 0x00003245
		public static IntVec3 FromVector3(Vector3 v, int newY)
		{
			return new IntVec3((int)v.x, newY, (int)v.z);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000505B File Offset: 0x0000325B
		public Vector2 ToUIPosition()
		{
			return this.ToVector3Shifted().MapToUIPosition();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005068 File Offset: 0x00003268
		public bool AdjacentToCardinal(IntVec3 other)
		{
			return this.IsValid && ((other.z == this.z && (other.x == this.x + 1 || other.x == this.x - 1)) || (other.x == this.x && (other.z == this.z + 1 || other.z == this.z - 1)));
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000050E0 File Offset: 0x000032E0
		public bool AdjacentToDiagonal(IntVec3 other)
		{
			return this.IsValid && Mathf.Abs(this.x - other.x) == 1 && Mathf.Abs(this.z - other.z) == 1;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005118 File Offset: 0x00003318
		public bool AdjacentToCardinal(Room room)
		{
			if (!this.IsValid)
			{
				return false;
			}
			Map map = room.Map;
			if (this.InBounds(map) && this.GetRoom(map, RegionType.Set_All) == room)
			{
				return true;
			}
			IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
			for (int i = 0; i < cardinalDirections.Length; i++)
			{
				IntVec3 intVec = this + cardinalDirections[i];
				if (intVec.InBounds(map) && intVec.GetRoom(map, RegionType.Set_All) == room)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005194 File Offset: 0x00003394
		public IntVec3 ClampInsideMap(Map map)
		{
			return this.ClampInsideRect(CellRect.WholeMap(map));
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000051A4 File Offset: 0x000033A4
		public IntVec3 ClampInsideRect(CellRect rect)
		{
			this.x = Mathf.Clamp(this.x, rect.minX, rect.maxX);
			this.y = 0;
			this.z = Mathf.Clamp(this.z, rect.minZ, rect.maxZ);
			return this;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000051F8 File Offset: 0x000033F8
		public static IntVec3 operator +(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005226 File Offset: 0x00003426
		public static IntVec3 operator -(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005254 File Offset: 0x00003454
		public static IntVec3 operator *(IntVec3 a, int i)
		{
			return new IntVec3(a.x * i, a.y * i, a.z * i);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005273 File Offset: 0x00003473
		public static bool operator ==(IntVec3 a, IntVec3 b)
		{
			return a.x == b.x && a.z == b.z && a.y == b.y;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000052A2 File Offset: 0x000034A2
		public static bool operator !=(IntVec3 a, IntVec3 b)
		{
			return a.x != b.x || a.z != b.z || a.y != b.y;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000052D1 File Offset: 0x000034D1
		public override bool Equals(object obj)
		{
			return obj is IntVec3 && this.Equals((IntVec3)obj);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000052E9 File Offset: 0x000034E9
		public bool Equals(IntVec3 other)
		{
			return this.x == other.x && this.z == other.z && this.y == other.y;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005317 File Offset: 0x00003517
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(0, this.x), this.y), this.z);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000533B File Offset: 0x0000353B
		public ulong UniqueHashCode()
		{
			return (ulong)(0L + (long)this.x + 4096L * (long)this.z + 16777216L * (long)this.y);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005368 File Offset: 0x00003568
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(),
				", ",
				this.y.ToString(),
				", ",
				this.z.ToString(),
				")"
			});
		}

		// Token: 0x0400002E RID: 46
		public int x;

		// Token: 0x0400002F RID: 47
		public int y;

		// Token: 0x04000030 RID: 48
		public int z;
	}
}
