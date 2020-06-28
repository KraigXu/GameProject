using System;

namespace Verse
{
	// Token: 0x02000291 RID: 657
	public class NameSingle : Name
	{
		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06001194 RID: 4500 RVA: 0x00063608 File Offset: 0x00061808
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06001195 RID: 4501 RVA: 0x00063608 File Offset: 0x00061808
		public override string ToStringFull
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06001196 RID: 4502 RVA: 0x00063608 File Offset: 0x00061808
		public override string ToStringShort
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06001197 RID: 4503 RVA: 0x00063610 File Offset: 0x00061810
		public override bool IsValid
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06001198 RID: 4504 RVA: 0x00063620 File Offset: 0x00061820
		public override bool Numerical
		{
			get
			{
				return this.numerical;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06001199 RID: 4505 RVA: 0x00063628 File Offset: 0x00061828
		private int FirstDigitPosition
		{
			get
			{
				if (!this.numerical)
				{
					return -1;
				}
				if (this.nameInt.NullOrEmpty() || !char.IsDigit(this.nameInt[this.nameInt.Length - 1]))
				{
					return -1;
				}
				for (int i = this.nameInt.Length - 2; i >= 0; i--)
				{
					if (!char.IsDigit(this.nameInt[i]))
					{
						return i + 1;
					}
				}
				return 0;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x0600119A RID: 4506 RVA: 0x000636A0 File Offset: 0x000618A0
		public string NameWithoutNumber
		{
			get
			{
				if (!this.numerical)
				{
					return this.nameInt;
				}
				int firstDigitPosition = this.FirstDigitPosition;
				if (firstDigitPosition < 0)
				{
					return this.nameInt;
				}
				int num = firstDigitPosition;
				if (num - 1 >= 0 && this.nameInt[num - 1] == ' ')
				{
					num--;
				}
				if (num <= 0)
				{
					return "";
				}
				return this.nameInt.Substring(0, num);
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x0600119B RID: 4507 RVA: 0x00063704 File Offset: 0x00061904
		public int Number
		{
			get
			{
				if (!this.numerical)
				{
					return 0;
				}
				int firstDigitPosition = this.FirstDigitPosition;
				if (firstDigitPosition < 0)
				{
					return 0;
				}
				return int.Parse(this.nameInt.Substring(firstDigitPosition));
			}
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00063739 File Offset: 0x00061939
		public NameSingle()
		{
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x00063741 File Offset: 0x00061941
		public NameSingle(string name, bool numerical = false)
		{
			this.nameInt = name;
			this.numerical = numerical;
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x00063757 File Offset: 0x00061957
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.nameInt, "name", null, false);
			Scribe_Values.Look<bool>(ref this.numerical, "numerical", false, false);
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x00063780 File Offset: 0x00061980
		public override bool ConfusinglySimilarTo(Name other)
		{
			NameSingle nameSingle = other as NameSingle;
			if (nameSingle != null && nameSingle.nameInt == this.nameInt)
			{
				return true;
			}
			NameTriple nameTriple = other as NameTriple;
			return nameTriple != null && nameTriple.Nick == this.nameInt;
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00063608 File Offset: 0x00061808
		public override string ToString()
		{
			return this.nameInt;
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x000637CC File Offset: 0x000619CC
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is NameSingle))
			{
				return false;
			}
			NameSingle nameSingle = (NameSingle)obj;
			return this.nameInt == nameSingle.nameInt;
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x00063800 File Offset: 0x00061A00
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.nameInt.GetHashCode(), 1384661390);
		}

		// Token: 0x04000C7B RID: 3195
		private string nameInt;

		// Token: 0x04000C7C RID: 3196
		private bool numerical;
	}
}
