using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000008 RID: 8
	public struct CellRect : IEquatable<CellRect>, IEnumerable<IntVec3>, IEnumerable
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002FC0 File Offset: 0x000011C0
		public static CellRect Empty
		{
			get
			{
				return new CellRect(0, 0, 0, 0);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002FCB File Offset: 0x000011CB
		public bool IsEmpty
		{
			get
			{
				return this.Width <= 0 || this.Height <= 0;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002FE4 File Offset: 0x000011E4
		public int Area
		{
			get
			{
				return this.Width * this.Height;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002FF3 File Offset: 0x000011F3
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00003014 File Offset: 0x00001214
		public int Width
		{
			get
			{
				if (this.minX > this.maxX)
				{
					return 0;
				}
				return this.maxX - this.minX + 1;
			}
			set
			{
				this.maxX = this.minX + Mathf.Max(value, 0) - 1;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000049 RID: 73 RVA: 0x0000302C File Offset: 0x0000122C
		// (set) Token: 0x0600004A RID: 74 RVA: 0x0000304D File Offset: 0x0000124D
		public int Height
		{
			get
			{
				if (this.minZ > this.maxZ)
				{
					return 0;
				}
				return this.maxZ - this.minZ + 1;
			}
			set
			{
				this.maxZ = this.minZ + Mathf.Max(value, 0) - 1;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00003065 File Offset: 0x00001265
		public IEnumerable<IntVec3> Corners
		{
			get
			{
				if (this.IsEmpty)
				{
					yield break;
				}
				yield return new IntVec3(this.minX, 0, this.minZ);
				if (this.Width > 1)
				{
					yield return new IntVec3(this.maxX, 0, this.minZ);
				}
				if (this.Height > 1)
				{
					yield return new IntVec3(this.minX, 0, this.maxZ);
					if (this.Width > 1)
					{
						yield return new IntVec3(this.maxX, 0, this.maxZ);
					}
				}
				yield break;
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000307A File Offset: 0x0000127A
		[Obsolete("Use foreach on the cellrect instead")]
		public CellRect.CellRectIterator GetIterator()
		{
			return new CellRect.CellRectIterator(this);
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00003087 File Offset: 0x00001287
		public IntVec3 BottomLeft
		{
			get
			{
				return new IntVec3(this.minX, 0, this.minZ);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600004E RID: 78 RVA: 0x0000309B File Offset: 0x0000129B
		public IntVec3 TopRight
		{
			get
			{
				return new IntVec3(this.maxX, 0, this.maxZ);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600004F RID: 79 RVA: 0x000030AF File Offset: 0x000012AF
		public IntVec3 RandomCell
		{
			get
			{
				return new IntVec3(Rand.RangeInclusive(this.minX, this.maxX), 0, Rand.RangeInclusive(this.minZ, this.maxZ));
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000050 RID: 80 RVA: 0x000030D9 File Offset: 0x000012D9
		public IntVec3 CenterCell
		{
			get
			{
				return new IntVec3(this.minX + this.Width / 2, 0, this.minZ + this.Height / 2);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000030FF File Offset: 0x000012FF
		public Vector3 CenterVector3
		{
			get
			{
				return new Vector3((float)this.minX + (float)this.Width / 2f, 0f, (float)this.minZ + (float)this.Height / 2f);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00003135 File Offset: 0x00001335
		public Vector3 RandomVector3
		{
			get
			{
				return new Vector3(Rand.Range((float)this.minX, (float)this.maxX + 1f), 0f, Rand.Range((float)this.minZ, (float)this.maxZ + 1f));
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00003173 File Offset: 0x00001373
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int z = this.minZ; z <= this.maxZ; z = num + 1)
				{
					for (int x = this.minX; x <= this.maxX; x = num + 1)
					{
						yield return new IntVec3(x, 0, z);
						num = x;
					}
					num = z;
				}
				yield break;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00003188 File Offset: 0x00001388
		public IEnumerable<IntVec2> Cells2D
		{
			get
			{
				int num;
				for (int z = this.minZ; z <= this.maxZ; z = num + 1)
				{
					for (int x = this.minX; x <= this.maxX; x = num + 1)
					{
						yield return new IntVec2(x, z);
						num = x;
					}
					num = z;
				}
				yield break;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000055 RID: 85 RVA: 0x0000319D File Offset: 0x0000139D
		public IEnumerable<IntVec3> EdgeCells
		{
			get
			{
				if (this.IsEmpty)
				{
					yield break;
				}
				int x = this.minX;
				int z = this.minZ;
				int num;
				while (x <= this.maxX)
				{
					yield return new IntVec3(x, 0, z);
					num = x;
					x = num + 1;
				}
				num = x;
				x = num - 1;
				num = z;
				for (z = num + 1; z <= this.maxZ; z = num + 1)
				{
					yield return new IntVec3(x, 0, z);
					num = z;
				}
				num = z;
				z = num - 1;
				num = x;
				for (x = num - 1; x >= this.minX; x = num - 1)
				{
					yield return new IntVec3(x, 0, z);
					num = x;
				}
				num = x;
				x = num + 1;
				num = z;
				for (z = num - 1; z > this.minZ; z = num - 1)
				{
					yield return new IntVec3(x, 0, z);
					num = z;
				}
				yield break;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000031B2 File Offset: 0x000013B2
		public int EdgeCellsCount
		{
			get
			{
				if (this.Area == 0)
				{
					return 0;
				}
				if (this.Area == 1)
				{
					return 1;
				}
				return this.Width * 2 + (this.Height - 2) * 2;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000031DC File Offset: 0x000013DC
		public IEnumerable<IntVec3> AdjacentCellsCardinal
		{
			get
			{
				if (this.IsEmpty)
				{
					yield break;
				}
				int num;
				for (int x = this.minX; x <= this.maxX; x = num + 1)
				{
					yield return new IntVec3(x, 0, this.minZ - 1);
					yield return new IntVec3(x, 0, this.maxZ + 1);
					num = x;
				}
				for (int x = this.minZ; x <= this.maxZ; x = num + 1)
				{
					yield return new IntVec3(this.minX - 1, 0, x);
					yield return new IntVec3(this.maxX + 1, 0, x);
					num = x;
				}
				yield break;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000031F1 File Offset: 0x000013F1
		public IEnumerable<IntVec3> AdjacentCells
		{
			get
			{
				if (this.IsEmpty)
				{
					yield break;
				}
				foreach (IntVec3 intVec in this.AdjacentCellsCardinal)
				{
					yield return intVec;
				}
				IEnumerator<IntVec3> enumerator = null;
				yield return new IntVec3(this.minX - 1, 0, this.minZ - 1);
				yield return new IntVec3(this.maxX + 1, 0, this.minZ - 1);
				yield return new IntVec3(this.minX - 1, 0, this.maxZ + 1);
				yield return new IntVec3(this.maxX + 1, 0, this.maxZ + 1);
				yield break;
				yield break;
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003206 File Offset: 0x00001406
		public static bool operator ==(CellRect lhs, CellRect rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003210 File Offset: 0x00001410
		public static bool operator !=(CellRect lhs, CellRect rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000321C File Offset: 0x0000141C
		public CellRect(int minX, int minZ, int width, int height)
		{
			this.minX = minX;
			this.minZ = minZ;
			this.maxX = minX + width - 1;
			this.maxZ = minZ + height - 1;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003243 File Offset: 0x00001443
		public static CellRect WholeMap(Map map)
		{
			return new CellRect(0, 0, map.Size.x, map.Size.z);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003264 File Offset: 0x00001464
		public static CellRect FromLimits(int minX, int minZ, int maxX, int maxZ)
		{
			return new CellRect
			{
				minX = Mathf.Min(minX, maxX),
				minZ = Mathf.Min(minZ, maxZ),
				maxX = Mathf.Max(maxX, minX),
				maxZ = Mathf.Max(maxZ, minZ)
			};
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000032B4 File Offset: 0x000014B4
		public static CellRect FromLimits(IntVec3 first, IntVec3 second)
		{
			return new CellRect
			{
				minX = Mathf.Min(first.x, second.x),
				minZ = Mathf.Min(first.z, second.z),
				maxX = Mathf.Max(first.x, second.x),
				maxZ = Mathf.Max(first.z, second.z)
			};
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000332C File Offset: 0x0000152C
		public static CellRect CenteredOn(IntVec3 center, int radius)
		{
			return new CellRect
			{
				minX = center.x - radius,
				maxX = center.x + radius,
				minZ = center.z - radius,
				maxZ = center.z + radius
			};
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003380 File Offset: 0x00001580
		public static CellRect CenteredOn(IntVec3 center, int width, int height)
		{
			CellRect cellRect = default(CellRect);
			cellRect.minX = center.x - width / 2;
			cellRect.minZ = center.z - height / 2;
			cellRect.maxX = cellRect.minX + width - 1;
			cellRect.maxZ = cellRect.minZ + height - 1;
			return cellRect;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000033DA File Offset: 0x000015DA
		public static CellRect ViewRect(Map map)
		{
			if (Current.ProgramState != ProgramState.Playing || Find.CurrentMap != map || WorldRendererUtility.WorldRenderedNow)
			{
				return CellRect.Empty;
			}
			return Find.CameraDriver.CurrentViewRect;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003403 File Offset: 0x00001603
		public static CellRect SingleCell(IntVec3 c)
		{
			return new CellRect(c.x, c.z, 1, 1);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003418 File Offset: 0x00001618
		public bool InBounds(Map map)
		{
			return this.minX >= 0 && this.minZ >= 0 && this.maxX < map.Size.x && this.maxZ < map.Size.z;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003454 File Offset: 0x00001654
		public bool FullyContainedWithin(CellRect within)
		{
			CellRect rhs = this;
			rhs.ClipInsideRect(within);
			return this == rhs;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003480 File Offset: 0x00001680
		public bool Overlaps(CellRect other)
		{
			return !this.IsEmpty && !other.IsEmpty && (this.minX <= other.maxX && this.maxX >= other.minX && this.maxZ >= other.minZ) && this.minZ <= other.maxZ;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000034E0 File Offset: 0x000016E0
		public bool IsOnEdge(IntVec3 c)
		{
			return (c.x == this.minX && c.z >= this.minZ && c.z <= this.maxZ) || (c.x == this.maxX && c.z >= this.minZ && c.z <= this.maxZ) || (c.z == this.minZ && c.x >= this.minX && c.x <= this.maxX) || (c.z == this.maxZ && c.x >= this.minX && c.x <= this.maxX);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000035A0 File Offset: 0x000017A0
		public bool IsOnEdge(IntVec3 c, Rot4 rot)
		{
			if (rot == Rot4.West)
			{
				return c.x == this.minX && c.z >= this.minZ && c.z <= this.maxZ;
			}
			if (rot == Rot4.East)
			{
				return c.x == this.maxX && c.z >= this.minZ && c.z <= this.maxZ;
			}
			if (rot == Rot4.South)
			{
				return c.z == this.minZ && c.x >= this.minX && c.x <= this.maxX;
			}
			return c.z == this.maxZ && c.x >= this.minX && c.x <= this.maxX;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003694 File Offset: 0x00001894
		public bool IsOnEdge(IntVec3 c, int edgeWidth)
		{
			return this.Contains(c) && (c.x < this.minX + edgeWidth || c.z < this.minZ + edgeWidth || c.x >= this.maxX + 1 - edgeWidth || c.z >= this.maxZ + 1 - edgeWidth);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000036F8 File Offset: 0x000018F8
		public bool IsCorner(IntVec3 c)
		{
			return (c.x == this.minX && c.z == this.minZ) || (c.x == this.maxX && c.z == this.minZ) || (c.x == this.minX && c.z == this.maxZ) || (c.x == this.maxX && c.z == this.maxZ);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000377C File Offset: 0x0000197C
		public Rot4 GetClosestEdge(IntVec3 c)
		{
			int num = Mathf.Abs(c.x - this.minX);
			int num2 = Mathf.Abs(c.x - this.maxX);
			int num3 = Mathf.Abs(c.z - this.maxZ);
			int num4 = Mathf.Abs(c.z - this.minZ);
			return GenMath.MinBy<Rot4>(Rot4.West, (float)num, Rot4.East, (float)num2, Rot4.North, (float)num3, Rot4.South, (float)num4);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000037F8 File Offset: 0x000019F8
		public CellRect ClipInsideMap(Map map)
		{
			if (this.minX < 0)
			{
				this.minX = 0;
			}
			if (this.minZ < 0)
			{
				this.minZ = 0;
			}
			if (this.maxX > map.Size.x - 1)
			{
				this.maxX = map.Size.x - 1;
			}
			if (this.maxZ > map.Size.z - 1)
			{
				this.maxZ = map.Size.z - 1;
			}
			return this;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000387C File Offset: 0x00001A7C
		public CellRect ClipInsideRect(CellRect otherRect)
		{
			if (this.minX < otherRect.minX)
			{
				this.minX = otherRect.minX;
			}
			if (this.maxX > otherRect.maxX)
			{
				this.maxX = otherRect.maxX;
			}
			if (this.minZ < otherRect.minZ)
			{
				this.minZ = otherRect.minZ;
			}
			if (this.maxZ > otherRect.maxZ)
			{
				this.maxZ = otherRect.maxZ;
			}
			return this;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000038F7 File Offset: 0x00001AF7
		public bool Contains(IntVec3 c)
		{
			return c.x >= this.minX && c.x <= this.maxX && c.z >= this.minZ && c.z <= this.maxZ;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003938 File Offset: 0x00001B38
		public float ClosestDistSquaredTo(IntVec3 c)
		{
			if (this.Contains(c))
			{
				return 0f;
			}
			if (c.x < this.minX)
			{
				if (c.z < this.minZ)
				{
					return (float)(c - new IntVec3(this.minX, 0, this.minZ)).LengthHorizontalSquared;
				}
				if (c.z > this.maxZ)
				{
					return (float)(c - new IntVec3(this.minX, 0, this.maxZ)).LengthHorizontalSquared;
				}
				return (float)((this.minX - c.x) * (this.minX - c.x));
			}
			else if (c.x > this.maxX)
			{
				if (c.z < this.minZ)
				{
					return (float)(c - new IntVec3(this.maxX, 0, this.minZ)).LengthHorizontalSquared;
				}
				if (c.z > this.maxZ)
				{
					return (float)(c - new IntVec3(this.maxX, 0, this.maxZ)).LengthHorizontalSquared;
				}
				return (float)((c.x - this.maxX) * (c.x - this.maxX));
			}
			else
			{
				if (c.z < this.minZ)
				{
					return (float)((this.minZ - c.z) * (this.minZ - c.z));
				}
				return (float)((c.z - this.maxZ) * (c.z - this.maxZ));
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003AB4 File Offset: 0x00001CB4
		public IntVec3 ClosestCellTo(IntVec3 c)
		{
			if (this.Contains(c))
			{
				return c;
			}
			if (c.x < this.minX)
			{
				if (c.z < this.minZ)
				{
					return new IntVec3(this.minX, 0, this.minZ);
				}
				if (c.z > this.maxZ)
				{
					return new IntVec3(this.minX, 0, this.maxZ);
				}
				return new IntVec3(this.minX, 0, c.z);
			}
			else if (c.x > this.maxX)
			{
				if (c.z < this.minZ)
				{
					return new IntVec3(this.maxX, 0, this.minZ);
				}
				if (c.z > this.maxZ)
				{
					return new IntVec3(this.maxX, 0, this.maxZ);
				}
				return new IntVec3(this.maxX, 0, c.z);
			}
			else
			{
				if (c.z < this.minZ)
				{
					return new IntVec3(c.x, 0, this.minZ);
				}
				return new IntVec3(c.x, 0, this.maxZ);
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003BC8 File Offset: 0x00001DC8
		public bool InNoBuildEdgeArea(Map map)
		{
			return !this.IsEmpty && (this.minX < 10 || this.minZ < 10 || this.maxX >= map.Size.x - 10 || this.maxZ >= map.Size.z - 10);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003C24 File Offset: 0x00001E24
		public IEnumerable<IntVec3> GetEdgeCells(Rot4 dir)
		{
			if (dir == Rot4.North)
			{
				int num;
				for (int x = this.minX; x <= this.maxX; x = num + 1)
				{
					yield return new IntVec3(x, 0, this.maxZ);
					num = x;
				}
			}
			else if (dir == Rot4.South)
			{
				int num;
				for (int x = this.minX; x <= this.maxX; x = num + 1)
				{
					yield return new IntVec3(x, 0, this.minZ);
					num = x;
				}
			}
			else if (dir == Rot4.West)
			{
				int num;
				for (int x = this.minZ; x <= this.maxZ; x = num + 1)
				{
					yield return new IntVec3(this.minX, 0, x);
					num = x;
				}
			}
			else if (dir == Rot4.East)
			{
				int num;
				for (int x = this.minZ; x <= this.maxZ; x = num + 1)
				{
					yield return new IntVec3(this.maxX, 0, x);
					num = x;
				}
			}
			yield break;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003C40 File Offset: 0x00001E40
		public bool TryFindRandomInnerRectTouchingEdge(IntVec2 size, out CellRect rect, Predicate<CellRect> predicate = null)
		{
			if (this.Width < size.x || this.Height < size.z)
			{
				rect = CellRect.Empty;
				return false;
			}
			if (size.x <= 0 || size.z <= 0 || this.IsEmpty)
			{
				rect = CellRect.Empty;
				return false;
			}
			CellRect cellRect = this;
			cellRect.maxX -= size.x - 1;
			cellRect.maxZ -= size.z - 1;
			IntVec3 intVec;
			if (cellRect.EdgeCells.Where(delegate(IntVec3 x)
			{
				if (predicate == null)
				{
					return true;
				}
				CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
				return predicate(obj);
			}).TryRandomElement(out intVec))
			{
				rect = new CellRect(intVec.x, intVec.z, size.x, size.z);
				return true;
			}
			rect = CellRect.Empty;
			return false;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003D58 File Offset: 0x00001F58
		public bool TryFindRandomInnerRect(IntVec2 size, out CellRect rect, Predicate<CellRect> predicate = null)
		{
			if (this.Width < size.x || this.Height < size.z)
			{
				rect = CellRect.Empty;
				return false;
			}
			if (size.x <= 0 || size.z <= 0 || this.IsEmpty)
			{
				rect = CellRect.Empty;
				return false;
			}
			CellRect cellRect = this;
			cellRect.maxX -= size.x - 1;
			cellRect.maxZ -= size.z - 1;
			IntVec3 intVec;
			if (cellRect.Cells.Where(delegate(IntVec3 x)
			{
				if (predicate == null)
				{
					return true;
				}
				CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
				return predicate(obj);
			}).TryRandomElement(out intVec))
			{
				rect = new CellRect(intVec.x, intVec.z, size.x, size.z);
				return true;
			}
			rect = CellRect.Empty;
			return false;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003E70 File Offset: 0x00002070
		public CellRect ExpandedBy(int dist)
		{
			CellRect result = this;
			result.minX -= dist;
			result.minZ -= dist;
			result.maxX += dist;
			result.maxZ += dist;
			return result;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003EB5 File Offset: 0x000020B5
		public CellRect ContractedBy(int dist)
		{
			return this.ExpandedBy(-dist);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003EBF File Offset: 0x000020BF
		public CellRect MovedBy(IntVec2 offset)
		{
			return this.MovedBy(offset.ToIntVec3);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003ED0 File Offset: 0x000020D0
		public CellRect MovedBy(IntVec3 offset)
		{
			CellRect result = this;
			result.minX += offset.x;
			result.minZ += offset.z;
			result.maxX += offset.x;
			result.maxZ += offset.z;
			return result;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003F29 File Offset: 0x00002129
		public int IndexOf(IntVec3 location)
		{
			return location.x - this.minX + (location.z - this.minZ) * this.Width;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003F50 File Offset: 0x00002150
		public void DebugDraw()
		{
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Vector3 vector = new Vector3((float)this.minX, y, (float)this.minZ);
			Vector3 vector2 = new Vector3((float)this.minX, y, (float)(this.maxZ + 1));
			Vector3 vector3 = new Vector3((float)(this.maxX + 1), y, (float)(this.maxZ + 1));
			Vector3 vector4 = new Vector3((float)(this.maxX + 1), y, (float)this.minZ);
			GenDraw.DrawLineBetween(vector, vector2);
			GenDraw.DrawLineBetween(vector2, vector3);
			GenDraw.DrawLineBetween(vector3, vector4);
			GenDraw.DrawLineBetween(vector4, vector);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003FE3 File Offset: 0x000021E3
		public CellRect.Enumerator GetEnumerator()
		{
			return new CellRect.Enumerator(this);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003FF0 File Offset: 0x000021F0
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003FF0 File Offset: 0x000021F0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004000 File Offset: 0x00002200
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.minX,
				",",
				this.minZ,
				",",
				this.maxX,
				",",
				this.maxZ,
				")"
			});
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000407C File Offset: 0x0000227C
		public static CellRect FromString(string str)
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
			int num = Convert.ToInt32(array[0], invariantCulture);
			int num2 = Convert.ToInt32(array[1], invariantCulture);
			int num3 = Convert.ToInt32(array[2], invariantCulture);
			int num4 = Convert.ToInt32(array[3], invariantCulture);
			return new CellRect(num, num2, num3 - num + 1, num4 - num2 + 1);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004100 File Offset: 0x00002300
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(0, this.minX), this.maxX), this.minZ), this.maxZ);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000412F File Offset: 0x0000232F
		public override bool Equals(object obj)
		{
			return obj is CellRect && this.Equals((CellRect)obj);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004147 File Offset: 0x00002347
		public bool Equals(CellRect other)
		{
			return this.minX == other.minX && this.maxX == other.maxX && this.minZ == other.minZ && this.maxZ == other.maxZ;
		}

		// Token: 0x0400001F RID: 31
		public int minX;

		// Token: 0x04000020 RID: 32
		public int maxX;

		// Token: 0x04000021 RID: 33
		public int minZ;

		// Token: 0x04000022 RID: 34
		public int maxZ;

		// Token: 0x020012C9 RID: 4809
		public struct Enumerator : IEnumerator<IntVec3>, IEnumerator, IDisposable
		{
			// Token: 0x1700135F RID: 4959
			// (get) Token: 0x0600721A RID: 29210 RVA: 0x0027E2C4 File Offset: 0x0027C4C4
			public IntVec3 Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			// Token: 0x17001360 RID: 4960
			// (get) Token: 0x0600721B RID: 29211 RVA: 0x0027E2D8 File Offset: 0x0027C4D8
			object IEnumerator.Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			// Token: 0x0600721C RID: 29212 RVA: 0x0027E2F1 File Offset: 0x0027C4F1
			public Enumerator(CellRect ir)
			{
				this.ir = ir;
				this.x = ir.minX - 1;
				this.z = ir.minZ;
			}

			// Token: 0x0600721D RID: 29213 RVA: 0x0027E314 File Offset: 0x0027C514
			public bool MoveNext()
			{
				this.x++;
				if (this.x > this.ir.maxX)
				{
					this.x = this.ir.minX;
					this.z++;
				}
				return this.z <= this.ir.maxZ;
			}

			// Token: 0x0600721E RID: 29214 RVA: 0x0027E377 File Offset: 0x0027C577
			public void Reset()
			{
				this.x = this.ir.minX - 1;
				this.z = this.ir.minZ;
			}

			// Token: 0x0600721F RID: 29215 RVA: 0x00002681 File Offset: 0x00000881
			void IDisposable.Dispose()
			{
			}

			// Token: 0x040046B3 RID: 18099
			private CellRect ir;

			// Token: 0x040046B4 RID: 18100
			private int x;

			// Token: 0x040046B5 RID: 18101
			private int z;
		}

		// Token: 0x020012CA RID: 4810
		[Obsolete("Do not use this anymore, CellRect has a struct-enumerator as substitute")]
		public struct CellRectIterator
		{
			// Token: 0x17001361 RID: 4961
			// (get) Token: 0x06007220 RID: 29216 RVA: 0x0027E39D File Offset: 0x0027C59D
			public IntVec3 Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			// Token: 0x06007221 RID: 29217 RVA: 0x0027E3B1 File Offset: 0x0027C5B1
			public CellRectIterator(CellRect cr)
			{
				this.minX = cr.minX;
				this.maxX = cr.maxX;
				this.maxZ = cr.maxZ;
				this.x = cr.minX;
				this.z = cr.minZ;
			}

			// Token: 0x06007222 RID: 29218 RVA: 0x0027E3EF File Offset: 0x0027C5EF
			public void MoveNext()
			{
				this.x++;
				if (this.x > this.maxX)
				{
					this.x = this.minX;
					this.z++;
				}
			}

			// Token: 0x06007223 RID: 29219 RVA: 0x0027E427 File Offset: 0x0027C627
			public bool Done()
			{
				return this.z > this.maxZ;
			}

			// Token: 0x040046B6 RID: 18102
			private int maxX;

			// Token: 0x040046B7 RID: 18103
			private int minX;

			// Token: 0x040046B8 RID: 18104
			private int maxZ;

			// Token: 0x040046B9 RID: 18105
			private int x;

			// Token: 0x040046BA RID: 18106
			private int z;
		}
	}
}
