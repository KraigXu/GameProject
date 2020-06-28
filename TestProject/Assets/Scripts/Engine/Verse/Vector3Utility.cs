using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000011 RID: 17
	public static class Vector3Utility
	{
		// Token: 0x06000118 RID: 280 RVA: 0x00005512 File Offset: 0x00003712
		public static Vector3 HorizontalVectorFromAngle(float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000552C File Offset: 0x0000372C
		public static float AngleFlat(this Vector3 v)
		{
			if (v.x == 0f && v.z == 0f)
			{
				return 0f;
			}
			return Quaternion.LookRotation(v).eulerAngles.y;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000556C File Offset: 0x0000376C
		public static Vector3 RandomHorizontalOffset(float maxDist)
		{
			float d = Rand.Range(0f, maxDist);
			float y = (float)Rand.Range(0, 360);
			return Quaternion.Euler(new Vector3(0f, y, 0f)) * Vector3.forward * d;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000055B7 File Offset: 0x000037B7
		public static Vector3 Yto0(this Vector3 v3)
		{
			return new Vector3(v3.x, 0f, v3.z);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000055CF File Offset: 0x000037CF
		public static Vector3 RotatedBy(this Vector3 v3, float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * v3;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000055E4 File Offset: 0x000037E4
		public static Vector3 RotatedBy(this Vector3 orig, Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return orig;
			case 1:
				return new Vector3(orig.z, orig.y, -orig.x);
			case 2:
				return new Vector3(-orig.x, orig.y, -orig.z);
			case 3:
				return new Vector3(-orig.z, orig.y, orig.x);
			default:
				return orig;
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005660 File Offset: 0x00003860
		public static float AngleToFlat(this Vector3 a, Vector3 b)
		{
			return new Vector2(a.x, a.z).AngleTo(new Vector2(b.x, b.z));
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000568C File Offset: 0x0000388C
		public static Vector3 FromAngleFlat(float angle)
		{
			Vector2 vector = Vector2Utility.FromAngle(angle);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000056B6 File Offset: 0x000038B6
		public static float ToAngleFlat(this Vector3 v)
		{
			return new Vector2(v.x, v.z).ToAngle();
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000056CE File Offset: 0x000038CE
		public static Vector3 Abs(this Vector3 v)
		{
			return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}
	}
}
