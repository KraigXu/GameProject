using System;

namespace Verse
{
	
	public class NameSingle : Name
	{
		
		// (get) Token: 0x06001194 RID: 4500 RVA: 0x00063608 File Offset: 0x00061808
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		
		// (get) Token: 0x06001195 RID: 4501 RVA: 0x00063608 File Offset: 0x00061808
		public override string ToStringFull
		{
			get
			{
				return this.nameInt;
			}
		}

		
		// (get) Token: 0x06001196 RID: 4502 RVA: 0x00063608 File Offset: 0x00061808
		public override string ToStringShort
		{
			get
			{
				return this.nameInt;
			}
		}

		
		// (get) Token: 0x06001197 RID: 4503 RVA: 0x00063610 File Offset: 0x00061810
		public override bool IsValid
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		
		// (get) Token: 0x06001198 RID: 4504 RVA: 0x00063620 File Offset: 0x00061820
		public override bool Numerical
		{
			get
			{
				return this.numerical;
			}
		}

		
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

		
		public NameSingle()
		{
		}

		
		public NameSingle(string name, bool numerical = false)
		{
			this.nameInt = name;
			this.numerical = numerical;
		}

		
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.nameInt, "name", null, false);
			Scribe_Values.Look<bool>(ref this.numerical, "numerical", false, false);
		}

		
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

		
		public override string ToString()
		{
			return this.nameInt;
		}

		
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

		
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.nameInt.GetHashCode(), 1384661390);
		}

		
		private string nameInt;

		
		private bool numerical;
	}
}
