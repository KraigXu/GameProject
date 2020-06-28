using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200000C RID: 12
	public struct FloatRange : IEquatable<FloatRange>
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600009D RID: 157 RVA: 0x0000460E File Offset: 0x0000280E
		public static FloatRange Zero
		{
			get
			{
				return new FloatRange(0f, 0f);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600009E RID: 158 RVA: 0x0000461F File Offset: 0x0000281F
		public static FloatRange One
		{
			get
			{
				return new FloatRange(1f, 1f);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00004630 File Offset: 0x00002830
		public static FloatRange ZeroToOne
		{
			get
			{
				return new FloatRange(0f, 1f);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00004641 File Offset: 0x00002841
		public float Average
		{
			get
			{
				return (this.min + this.max) / 2f;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00004656 File Offset: 0x00002856
		public float RandomInRange
		{
			get
			{
				return Rand.Range(this.min, this.max);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00004669 File Offset: 0x00002869
		public float TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000467C File Offset: 0x0000287C
		public float TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x0000468F File Offset: 0x0000288F
		public float Span
		{
			get
			{
				return this.TrueMax - this.TrueMin;
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000469E File Offset: 0x0000289E
		public FloatRange(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000046AE File Offset: 0x000028AE
		public float ClampToRange(float value)
		{
			return Mathf.Clamp(value, this.min, this.max);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000046C2 File Offset: 0x000028C2
		public float RandomInRangeSeeded(int seed)
		{
			return Rand.RangeSeeded(this.min, this.max, seed);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000046D6 File Offset: 0x000028D6
		public float LerpThroughRange(float lerpPct)
		{
			return Mathf.Lerp(this.min, this.max, lerpPct);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000046EA File Offset: 0x000028EA
		public float InverseLerpThroughRange(float f)
		{
			return Mathf.InverseLerp(this.min, this.max, f);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000046FE File Offset: 0x000028FE
		public bool Includes(float f)
		{
			return f >= this.min && f <= this.max;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004717 File Offset: 0x00002917
		public bool IncludesEpsilon(float f)
		{
			return f >= this.min - 1E-05f && f <= this.max + 1E-05f;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000473C File Offset: 0x0000293C
		public FloatRange ExpandedBy(float f)
		{
			return new FloatRange(this.min - f, this.max + f);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004753 File Offset: 0x00002953
		public static bool operator ==(FloatRange a, FloatRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004773 File Offset: 0x00002973
		public static bool operator !=(FloatRange a, FloatRange b)
		{
			return a.min != b.min || a.max != b.max;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004796 File Offset: 0x00002996
		public static FloatRange operator *(FloatRange r, float val)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000047AD File Offset: 0x000029AD
		public static FloatRange operator *(float val, FloatRange r)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000047C4 File Offset: 0x000029C4
		public static FloatRange FromString(string s)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			string[] array = s.Split(new char[]
			{
				'~'
			});
			if (array.Length == 1)
			{
				float num = Convert.ToSingle(array[0], invariantCulture);
				return new FloatRange(num, num);
			}
			return new FloatRange(Convert.ToSingle(array[0], invariantCulture), Convert.ToSingle(array[1], invariantCulture));
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004816 File Offset: 0x00002A16
		public override string ToString()
		{
			return this.min + "~" + this.max;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004838 File Offset: 0x00002A38
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<float>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004850 File Offset: 0x00002A50
		public override bool Equals(object obj)
		{
			return obj is FloatRange && this.Equals((FloatRange)obj);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004868 File Offset: 0x00002A68
		public bool Equals(FloatRange other)
		{
			return other.min == this.min && other.max == this.max;
		}

		// Token: 0x04000028 RID: 40
		public float min;

		// Token: 0x04000029 RID: 41
		public float max;
	}
}
