using System;
using System.Linq;

namespace Verse
{
	// Token: 0x02000292 RID: 658
	public class NameTriple : Name
	{
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x060011A3 RID: 4515 RVA: 0x00063817 File Offset: 0x00061A17
		public string First
		{
			get
			{
				return this.firstInt;
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x060011A4 RID: 4516 RVA: 0x0006381F File Offset: 0x00061A1F
		public string Nick
		{
			get
			{
				return this.nickInt;
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x060011A5 RID: 4517 RVA: 0x00063827 File Offset: 0x00061A27
		public string Last
		{
			get
			{
				return this.lastInt;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x060011A6 RID: 4518 RVA: 0x00063830 File Offset: 0x00061A30
		public override string ToStringFull
		{
			get
			{
				if (this.First == this.Nick || this.Last == this.Nick)
				{
					return this.First + " " + this.Last;
				}
				return string.Concat(new string[]
				{
					this.First,
					" '",
					this.Nick,
					"' ",
					this.Last
				});
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x060011A7 RID: 4519 RVA: 0x0006381F File Offset: 0x00061A1F
		public override string ToStringShort
		{
			get
			{
				return this.nickInt;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060011A8 RID: 4520 RVA: 0x000638B0 File Offset: 0x00061AB0
		public override bool IsValid
		{
			get
			{
				return !this.First.NullOrEmpty() && !this.Last.NullOrEmpty();
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060011A9 RID: 4521 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool Numerical
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060011AA RID: 4522 RVA: 0x000638CF File Offset: 0x00061ACF
		public static NameTriple Invalid
		{
			get
			{
				return NameTriple.invalidInt;
			}
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x00063739 File Offset: 0x00061939
		public NameTriple()
		{
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x000638D6 File Offset: 0x00061AD6
		public NameTriple(string first, string nick, string last)
		{
			this.firstInt = first.Trim();
			this.nickInt = nick.Trim();
			this.lastInt = last.Trim();
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x00063902 File Offset: 0x00061B02
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.firstInt, "first", null, false);
			Scribe_Values.Look<string>(ref this.nickInt, "nick", null, false);
			Scribe_Values.Look<string>(ref this.lastInt, "last", null, false);
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x0006393C File Offset: 0x00061B3C
		public void PostLoad()
		{
			if (this.firstInt != null)
			{
				this.firstInt = this.firstInt.Trim();
			}
			if (this.nickInt != null)
			{
				this.nickInt = this.nickInt.Trim();
			}
			if (this.lastInt != null)
			{
				this.lastInt = this.lastInt.Trim();
			}
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x00063994 File Offset: 0x00061B94
		public void ResolveMissingPieces(string overrideLastName = null)
		{
			if (this.First.NullOrEmpty() && this.Nick.NullOrEmpty() && this.Last.NullOrEmpty())
			{
				Log.Error("Cannot resolve misssing pieces in PawnName: No name data.", false);
				this.firstInt = (this.nickInt = (this.lastInt = "Empty"));
				return;
			}
			if (this.First == null)
			{
				this.firstInt = "";
			}
			if (this.Last == null)
			{
				this.lastInt = "";
			}
			if (overrideLastName != null)
			{
				this.lastInt = overrideLastName;
			}
			if (this.Nick.NullOrEmpty())
			{
				if (this.Last == "")
				{
					this.nickInt = this.First;
					return;
				}
				if (Rand.ValueSeeded(Gen.HashCombine<string>(this.First.GetHashCode(), this.Last)) < 0.5f)
				{
					this.nickInt = this.First;
				}
				else
				{
					this.nickInt = this.Last;
				}
				this.CapitalizeNick();
			}
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x00063A90 File Offset: 0x00061C90
		public override bool ConfusinglySimilarTo(Name other)
		{
			NameTriple nameTriple = other as NameTriple;
			if (nameTriple != null)
			{
				if (this.Nick != null && this.Nick == nameTriple.Nick)
				{
					return true;
				}
				if (this.First == nameTriple.First && this.Last == nameTriple.Last)
				{
					return true;
				}
			}
			NameSingle nameSingle = other as NameSingle;
			return nameSingle != null && nameSingle.Name == this.Nick;
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00063B0C File Offset: 0x00061D0C
		public static NameTriple FromString(string rawName)
		{
			if (rawName.Trim().Length == 0)
			{
				Log.Error("Tried to parse PawnName from empty or whitespace string.", false);
				return NameTriple.Invalid;
			}
			NameTriple nameTriple = new NameTriple();
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < rawName.Length - 1; i++)
			{
				if (rawName[i] == ' ' && rawName[i + 1] == '\'' && num == -1)
				{
					num = i;
				}
				if (rawName[i] == '\'' && rawName[i + 1] == ' ')
				{
					num2 = i;
				}
			}
			if (num == -1 || num2 == -1)
			{
				if (!rawName.Contains(' '))
				{
					nameTriple.nickInt = rawName.Trim();
				}
				else
				{
					string[] array = rawName.Split(new char[]
					{
						' '
					});
					if (array.Length == 1)
					{
						nameTriple.nickInt = array[0].Trim();
					}
					else if (array.Length == 2)
					{
						nameTriple.firstInt = array[0].Trim();
						nameTriple.lastInt = array[1].Trim();
					}
					else
					{
						nameTriple.firstInt = array[0].Trim();
						nameTriple.lastInt = "";
						for (int j = 1; j < array.Length; j++)
						{
							NameTriple nameTriple2 = nameTriple;
							nameTriple2.lastInt += array[j];
							if (j < array.Length - 1)
							{
								NameTriple nameTriple3 = nameTriple;
								nameTriple3.lastInt += " ";
							}
						}
					}
				}
			}
			else
			{
				nameTriple.firstInt = rawName.Substring(0, num).Trim();
				nameTriple.nickInt = rawName.Substring(num + 2, num2 - num - 2).Trim();
				nameTriple.lastInt = ((num2 < rawName.Length - 2) ? rawName.Substring(num2 + 2).Trim() : "");
			}
			return nameTriple;
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00063CC4 File Offset: 0x00061EC4
		public void CapitalizeNick()
		{
			if (!this.nickInt.NullOrEmpty())
			{
				this.nickInt = char.ToUpper(this.Nick[0]).ToString() + this.Nick.Substring(1);
			}
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x00063D0E File Offset: 0x00061F0E
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.First,
				" '",
				this.Nick,
				"' ",
				this.Last
			});
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x00063D48 File Offset: 0x00061F48
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is NameTriple))
			{
				return false;
			}
			NameTriple nameTriple = (NameTriple)obj;
			return this.First == nameTriple.First && this.Last == nameTriple.Last && this.Nick == nameTriple.Nick;
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00063DA4 File Offset: 0x00061FA4
		public override int GetHashCode()
		{
			return Gen.HashCombine<string>(Gen.HashCombine<string>(Gen.HashCombine<string>(0, this.First), this.Last), this.Nick);
		}

		// Token: 0x04000C7D RID: 3197
		[LoadAlias("first")]
		private string firstInt;

		// Token: 0x04000C7E RID: 3198
		[LoadAlias("nick")]
		private string nickInt;

		// Token: 0x04000C7F RID: 3199
		[LoadAlias("last")]
		private string lastInt;

		// Token: 0x04000C80 RID: 3200
		private static NameTriple invalidInt = new NameTriple("Invalid", "Invalid", "Invalid");
	}
}
