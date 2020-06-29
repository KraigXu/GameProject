using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	
	public struct IntVec3 : IEquatable<IntVec3>
	{
		
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00004D43 File Offset: 0x00002F43
		public IntVec2 ToIntVec2
		{
			get
			{
				return new IntVec2(this.x, this.z);
			}
		}

		
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00004D56 File Offset: 0x00002F56
		public bool IsValid
		{
			get
			{
				return this.y >= 0;
			}
		}

		
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00004D64 File Offset: 0x00002F64
		public int LengthHorizontalSquared
		{
			get
			{
				return this.x * this.x + this.z * this.z;
			}
		}

		
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00004D81 File Offset: 0x00002F81
		public float LengthHorizontal
		{
			get
			{
				return GenMath.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00004DA4 File Offset: 0x00002FA4
		public int LengthManhattan
		{
			get
			{
				return ((this.x >= 0) ? this.x : (-this.x)) + ((this.z >= 0) ? this.z : (-this.z));
			}
		}

		
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

		
		public IntVec3(int newX, int newY, int newZ)
		{
			this.x = newX;
			this.y = newY;
			this.z = newZ;
		}

		
		public IntVec3(Vector3 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.z;
		}

		
		public IntVec3(Vector2 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.y;
		}

		
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00004E70 File Offset: 0x00003070
		public static IntVec3 Zero
		{
			get
			{
				return new IntVec3(0, 0, 0);
			}
		}

		
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00004E7A File Offset: 0x0000307A
		public static IntVec3 North
		{
			get
			{
				return new IntVec3(0, 0, 1);
			}
		}

		
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00004E84 File Offset: 0x00003084
		public static IntVec3 East
		{
			get
			{
				return new IntVec3(1, 0, 0);
			}
		}

		
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00004E8E File Offset: 0x0000308E
		public static IntVec3 South
		{
			get
			{
				return new IntVec3(0, 0, -1);
			}
		}

		
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00004E98 File Offset: 0x00003098
		public static IntVec3 West
		{
			get
			{
				return new IntVec3(-1, 0, 0);
			}
		}

		
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00004EA2 File Offset: 0x000030A2
		public static IntVec3 NorthWest
		{
			get
			{
				return new IntVec3(-1, 0, 1);
			}
		}

		
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00004EAC File Offset: 0x000030AC
		public static IntVec3 NorthEast
		{
			get
			{
				return new IntVec3(1, 0, 1);
			}
		}

		
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00004EB6 File Offset: 0x000030B6
		public static IntVec3 SouthWest
		{
			get
			{
				return new IntVec3(-1, 0, -1);
			}
		}

		
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00004EC0 File Offset: 0x000030C0
		public static IntVec3 SouthEast
		{
			get
			{
				return new IntVec3(1, 0, -1);
			}
		}

		
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00004ECA File Offset: 0x000030CA
		public static IntVec3 Invalid
		{
			get
			{
				return new IntVec3(-1000, -1000, -1000);
			}
		}

		
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

		
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, (float)this.y, (float)this.z);
		}

		
		public Vector3 ToVector3Shifted()
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y, (float)this.z + 0.5f);
		}

		
		public Vector3 ToVector3ShiftedWithAltitude(AltitudeLayer AltLayer)
		{
			return this.ToVector3ShiftedWithAltitude(AltLayer.AltitudeFor());
		}

		
		public Vector3 ToVector3ShiftedWithAltitude(float AddedAltitude)
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y + AddedAltitude, (float)this.z + 0.5f);
		}

		
		public bool InHorDistOf(IntVec3 otherLoc, float maxDist)
		{
			float num = (float)(this.x - otherLoc.x);
			float num2 = (float)(this.z - otherLoc.z);
			return num * num + num2 * num2 <= maxDist * maxDist;
		}

		
		public static IntVec3 FromVector3(Vector3 v)
		{
			return IntVec3.FromVector3(v, 0);
		}

		
		public static IntVec3 FromVector3(Vector3 v, int newY)
		{
			return new IntVec3((int)v.x, newY, (int)v.z);
		}

		
		public Vector2 ToUIPosition()
		{
			return this.ToVector3Shifted().MapToUIPosition();
		}

		
		public bool AdjacentToCardinal(IntVec3 other)
		{
			return this.IsValid && ((other.z == this.z && (other.x == this.x + 1 || other.x == this.x - 1)) || (other.x == this.x && (other.z == this.z + 1 || other.z == this.z - 1)));
		}

		
		public bool AdjacentToDiagonal(IntVec3 other)
		{
			return this.IsValid && Mathf.Abs(this.x - other.x) == 1 && Mathf.Abs(this.z - other.z) == 1;
		}

		
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

		
		public IntVec3 ClampInsideMap(Map map)
		{
			return this.ClampInsideRect(CellRect.WholeMap(map));
		}

		
		public IntVec3 ClampInsideRect(CellRect rect)
		{
			this.x = Mathf.Clamp(this.x, rect.minX, rect.maxX);
			this.y = 0;
			this.z = Mathf.Clamp(this.z, rect.minZ, rect.maxZ);
			return this;
		}

		
		public static IntVec3 operator +(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		
		public static IntVec3 operator -(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		
		public static IntVec3 operator *(IntVec3 a, int i)
		{
			return new IntVec3(a.x * i, a.y * i, a.z * i);
		}

		
		public static bool operator ==(IntVec3 a, IntVec3 b)
		{
			return a.x == b.x && a.z == b.z && a.y == b.y;
		}

		
		public static bool operator !=(IntVec3 a, IntVec3 b)
		{
			return a.x != b.x || a.z != b.z || a.y != b.y;
		}

		
		public override bool Equals(object obj)
		{
			return obj is IntVec3 && this.Equals((IntVec3)obj);
		}

		
		public bool Equals(IntVec3 other)
		{
			return this.x == other.x && this.z == other.z && this.y == other.y;
		}

		
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(0, this.x), this.y), this.z);
		}

		
		public ulong UniqueHashCode()
		{
			return (ulong)(0L + (long)this.x + 4096L * (long)this.z + 16777216L * (long)this.y);
		}

		
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

		
		public int x;

		
		public int y;

		
		public int z;
	}
}
