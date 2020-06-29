using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	
	public struct FloatRange : IEquatable<FloatRange>
	{
		
		// (get) Token: 0x0600009D RID: 157 RVA: 0x0000460E File Offset: 0x0000280E
		public static FloatRange Zero
		{
			get
			{
				return new FloatRange(0f, 0f);
			}
		}

		
		// (get) Token: 0x0600009E RID: 158 RVA: 0x0000461F File Offset: 0x0000281F
		public static FloatRange One
		{
			get
			{
				return new FloatRange(1f, 1f);
			}
		}

		
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00004630 File Offset: 0x00002830
		public static FloatRange ZeroToOne
		{
			get
			{
				return new FloatRange(0f, 1f);
			}
		}

		
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00004641 File Offset: 0x00002841
		public float Average
		{
			get
			{
				return (this.min + this.max) / 2f;
			}
		}

		
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00004656 File Offset: 0x00002856
		public float RandomInRange
		{
			get
			{
				return Rand.Range(this.min, this.max);
			}
		}

		
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00004669 File Offset: 0x00002869
		public float TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000467C File Offset: 0x0000287C
		public float TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x0000468F File Offset: 0x0000288F
		public float Span
		{
			get
			{
				return this.TrueMax - this.TrueMin;
			}
		}

		
		public FloatRange(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		
		public float ClampToRange(float value)
		{
			return Mathf.Clamp(value, this.min, this.max);
		}

		
		public float RandomInRangeSeeded(int seed)
		{
			return Rand.RangeSeeded(this.min, this.max, seed);
		}

		
		public float LerpThroughRange(float lerpPct)
		{
			return Mathf.Lerp(this.min, this.max, lerpPct);
		}

		
		public float InverseLerpThroughRange(float f)
		{
			return Mathf.InverseLerp(this.min, this.max, f);
		}

		
		public bool Includes(float f)
		{
			return f >= this.min && f <= this.max;
		}

		
		public bool IncludesEpsilon(float f)
		{
			return f >= this.min - 1E-05f && f <= this.max + 1E-05f;
		}

		
		public FloatRange ExpandedBy(float f)
		{
			return new FloatRange(this.min - f, this.max + f);
		}

		
		public static bool operator ==(FloatRange a, FloatRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		
		public static bool operator !=(FloatRange a, FloatRange b)
		{
			return a.min != b.min || a.max != b.max;
		}

		
		public static FloatRange operator *(FloatRange r, float val)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		
		public static FloatRange operator *(float val, FloatRange r)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		
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

		
		public override string ToString()
		{
			return this.min + "~" + this.max;
		}

		
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<float>(this.min.GetHashCode(), this.max);
		}

		
		public override bool Equals(object obj)
		{
			return obj is FloatRange && this.Equals((FloatRange)obj);
		}

		
		public bool Equals(FloatRange other)
		{
			return other.min == this.min && other.max == this.max;
		}

		
		public float min;

		
		public float max;
	}
}
