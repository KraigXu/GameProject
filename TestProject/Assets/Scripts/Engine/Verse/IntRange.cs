﻿using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	
	public struct IntRange : IEquatable<IntRange>
	{
		
		
		public static IntRange zero
		{
			get
			{
				return new IntRange(0, 0);
			}
		}

		
		
		public static IntRange one
		{
			get
			{
				return new IntRange(1, 1);
			}
		}

		
		
		public int TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		
		
		public int TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		
		
		public float Average
		{
			get
			{
				return ((float)this.min + (float)this.max) / 2f;
			}
		}

		
		
		public int RandomInRange
		{
			get
			{
				return Rand.RangeInclusive(this.min, this.max);
			}
		}

		
		public IntRange(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		
		public int Lerped(float lerpFactor)
		{
			return this.min + Mathf.RoundToInt(lerpFactor * (float)(this.max - this.min));
		}

		
		public static IntRange FromString(string s)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			string[] array = s.Split(new char[]
			{
				'~'
			});
			if (array.Length == 1)
			{
				int num = Convert.ToInt32(array[0], invariantCulture);
				return new IntRange(num, num);
			}
			return new IntRange(Convert.ToInt32(array[0], invariantCulture), Convert.ToInt32(array[1], invariantCulture));
		}

		
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.min, this.max);
		}

		
		public override bool Equals(object obj)
		{
			return obj is IntRange && this.Equals((IntRange)obj);
		}

		
		public bool Equals(IntRange other)
		{
			return this.min == other.min && this.max == other.max;
		}

		
		public static bool operator ==(IntRange lhs, IntRange rhs)
		{
			return lhs.Equals(rhs);
		}

		
		public static bool operator !=(IntRange lhs, IntRange rhs)
		{
			return !(lhs == rhs);
		}

		
		internal bool Includes(int val)
		{
			return val >= this.min && val <= this.max;
		}

		
		public int min;

		
		public int max;
	}
}
