using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000012 RID: 18
	public static class Vector2Utility
	{
		// Token: 0x06000122 RID: 290 RVA: 0x000056F6 File Offset: 0x000038F6
		public static Vector2 Rotated(this Vector2 v)
		{
			return new Vector2(v.y, v.x);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005709 File Offset: 0x00003909
		public static Vector2 RotatedBy(this Vector2 v, Rot4 rot)
		{
			return v.RotatedBy(rot.AsAngle);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005718 File Offset: 0x00003918
		public static Vector2 RotatedBy(this Vector2 v, float degrees)
		{
			float num = Mathf.Sin(degrees * 0.0174532924f);
			float num2 = Mathf.Cos(degrees * 0.0174532924f);
			return new Vector2(num2 * v.x - num * v.y, num * v.x + num2 * v.y);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005766 File Offset: 0x00003966
		public static float AngleTo(this Vector2 a, Vector2 b)
		{
			return Mathf.Atan2(-(b.y - a.y), b.x - a.x) * 57.29578f;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000578E File Offset: 0x0000398E
		public static Vector2 Moved(this Vector2 v, float angle, float distance)
		{
			return new Vector2(v.x + Mathf.Cos(angle * 0.0174532924f) * distance, v.y - Mathf.Sin(angle * 0.0174532924f) * distance);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000057BF File Offset: 0x000039BF
		public static Vector2 FromAngle(float angle)
		{
			return new Vector2(Mathf.Cos(angle * 0.0174532924f), -Mathf.Sin(angle * 0.0174532924f));
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000057DF File Offset: 0x000039DF
		public static float ToAngle(this Vector2 v)
		{
			return Mathf.Atan2(-v.y, v.x) * 57.29578f;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000057F9 File Offset: 0x000039F9
		public static float Cross(this Vector2 u, Vector2 v)
		{
			return u.x * v.y - u.y * v.x;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00005818 File Offset: 0x00003A18
		public static float DistanceToRect(this Vector2 u, Rect rect)
		{
			if (rect.Contains(u))
			{
				return 0f;
			}
			if (u.x < rect.xMin && u.y < rect.yMin)
			{
				return Vector2.Distance(u, new Vector2(rect.xMin, rect.yMin));
			}
			if (u.x > rect.xMax && u.y < rect.yMin)
			{
				return Vector2.Distance(u, new Vector2(rect.xMax, rect.yMin));
			}
			if (u.x < rect.xMin && u.y > rect.yMax)
			{
				return Vector2.Distance(u, new Vector2(rect.xMin, rect.yMax));
			}
			if (u.x > rect.xMax && u.y > rect.yMax)
			{
				return Vector2.Distance(u, new Vector2(rect.xMax, rect.yMax));
			}
			if (u.x < rect.xMin)
			{
				return rect.xMin - u.x;
			}
			if (u.x > rect.xMax)
			{
				return u.x - rect.xMax;
			}
			if (u.y < rect.yMin)
			{
				return rect.yMin - u.y;
			}
			return u.y - rect.yMax;
		}
	}
}
