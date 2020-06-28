using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D7D RID: 3453
	public struct QualityRange : IEquatable<QualityRange>
	{
		// Token: 0x17000EF7 RID: 3831
		// (get) Token: 0x06005423 RID: 21539 RVA: 0x001C16A5 File Offset: 0x001BF8A5
		public static QualityRange All
		{
			get
			{
				return new QualityRange(QualityCategory.Awful, QualityCategory.Legendary);
			}
		}

		// Token: 0x06005424 RID: 21540 RVA: 0x001C16AE File Offset: 0x001BF8AE
		public QualityRange(QualityCategory min, QualityCategory max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x06005425 RID: 21541 RVA: 0x001C16BE File Offset: 0x001BF8BE
		public bool Includes(QualityCategory p)
		{
			return p >= this.min && p <= this.max;
		}

		// Token: 0x06005426 RID: 21542 RVA: 0x001C16D7 File Offset: 0x001BF8D7
		public static bool operator ==(QualityRange a, QualityRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x06005427 RID: 21543 RVA: 0x001C16F7 File Offset: 0x001BF8F7
		public static bool operator !=(QualityRange a, QualityRange b)
		{
			return !(a == b);
		}

		// Token: 0x06005428 RID: 21544 RVA: 0x001C1704 File Offset: 0x001BF904
		public static QualityRange FromString(string s)
		{
			string[] array = s.Split(new char[]
			{
				'~'
			});
			return new QualityRange(ParseHelper.FromString<QualityCategory>(array[0]), ParseHelper.FromString<QualityCategory>(array[1]));
		}

		// Token: 0x06005429 RID: 21545 RVA: 0x001C1738 File Offset: 0x001BF938
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x0600542A RID: 21546 RVA: 0x001C1766 File Offset: 0x001BF966
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<QualityCategory>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x0600542B RID: 21547 RVA: 0x001C1784 File Offset: 0x001BF984
		public override bool Equals(object obj)
		{
			if (!(obj is QualityRange))
			{
				return false;
			}
			QualityRange qualityRange = (QualityRange)obj;
			return qualityRange.min == this.min && qualityRange.max == this.max;
		}

		// Token: 0x0600542C RID: 21548 RVA: 0x001C17C0 File Offset: 0x001BF9C0
		public bool Equals(QualityRange other)
		{
			return other.min == this.min && other.max == this.max;
		}

		// Token: 0x04002E60 RID: 11872
		public QualityCategory min;

		// Token: 0x04002E61 RID: 11873
		public QualityCategory max;
	}
}
