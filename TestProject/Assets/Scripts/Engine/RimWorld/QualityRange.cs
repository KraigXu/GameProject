using System;
using Verse;

namespace RimWorld
{
	
	public struct QualityRange : IEquatable<QualityRange>
	{
		
		// (get) Token: 0x06005423 RID: 21539 RVA: 0x001C16A5 File Offset: 0x001BF8A5
		public static QualityRange All
		{
			get
			{
				return new QualityRange(QualityCategory.Awful, QualityCategory.Legendary);
			}
		}

		
		public QualityRange(QualityCategory min, QualityCategory max)
		{
			this.min = min;
			this.max = max;
		}

		
		public bool Includes(QualityCategory p)
		{
			return p >= this.min && p <= this.max;
		}

		
		public static bool operator ==(QualityRange a, QualityRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		
		public static bool operator !=(QualityRange a, QualityRange b)
		{
			return !(a == b);
		}

		
		public static QualityRange FromString(string s)
		{
			string[] array = s.Split(new char[]
			{
				'~'
			});
			return new QualityRange(ParseHelper.FromString<QualityCategory>(array[0]), ParseHelper.FromString<QualityCategory>(array[1]));
		}

		
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<QualityCategory>(this.min.GetHashCode(), this.max);
		}

		
		public override bool Equals(object obj)
		{
			if (!(obj is QualityRange))
			{
				return false;
			}
			QualityRange qualityRange = (QualityRange)obj;
			return qualityRange.min == this.min && qualityRange.max == this.max;
		}

		
		public bool Equals(QualityRange other)
		{
			return other.min == this.min && other.max == this.max;
		}

		
		public QualityCategory min;

		
		public QualityCategory max;
	}
}
