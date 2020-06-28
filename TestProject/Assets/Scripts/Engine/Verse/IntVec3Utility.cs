using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000010 RID: 16
	public static class IntVec3Utility
	{
		// Token: 0x06000111 RID: 273 RVA: 0x000053CA File Offset: 0x000035CA
		public static IntVec3 ToIntVec3(this Vector3 vect)
		{
			return new IntVec3(vect);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000053D4 File Offset: 0x000035D4
		public static float DistanceTo(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontal;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000053F0 File Offset: 0x000035F0
		public static int DistanceToSquared(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontalSquared;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000540C File Offset: 0x0000360C
		public static IntVec3 RotatedBy(this IntVec3 orig, Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return orig;
			case 1:
				return new IntVec3(orig.z, orig.y, -orig.x);
			case 2:
				return new IntVec3(-orig.x, orig.y, -orig.z);
			case 3:
				return new IntVec3(-orig.z, orig.y, orig.x);
			default:
				return orig;
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005488 File Offset: 0x00003688
		public static int ManhattanDistanceFlat(IntVec3 a, IntVec3 b)
		{
			return Math.Abs(a.x - b.x) + Math.Abs(a.z - b.z);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000054AF File Offset: 0x000036AF
		public static IntVec3 RandomHorizontalOffset(float maxDist)
		{
			return Vector3Utility.RandomHorizontalOffset(maxDist).ToIntVec3();
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000054BC File Offset: 0x000036BC
		public static int DistanceToEdge(this IntVec3 v, Map map)
		{
			return Mathf.Max(Mathf.Min(Mathf.Min(Mathf.Min(v.x, v.z), map.Size.x - v.x - 1), map.Size.z - v.z - 1), 0);
		}
	}
}
