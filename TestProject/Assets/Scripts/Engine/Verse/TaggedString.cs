using System;

namespace Verse
{
	// Token: 0x020003B5 RID: 949
	public struct TaggedString
	{
		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001BF6 RID: 7158 RVA: 0x000AA983 File Offset: 0x000A8B83
		public string RawText
		{
			get
			{
				return this.rawText;
			}
		}

		// Token: 0x1700056B RID: 1387
		public char this[int i]
		{
			get
			{
				return this.RawText[i];
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001BF8 RID: 7160 RVA: 0x000AA999 File Offset: 0x000A8B99
		public int Length
		{
			get
			{
				return this.RawText.Length;
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001BF9 RID: 7161 RVA: 0x000AA9A6 File Offset: 0x000A8BA6
		public int StrippedLength
		{
			get
			{
				return this.RawText.StripTags().Length;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001BFA RID: 7162 RVA: 0x000AA9B8 File Offset: 0x000A8BB8
		public static TaggedString Empty
		{
			get
			{
				if (TaggedString.empty == null)
				{
					TaggedString.empty = new TaggedString("");
				}
				return TaggedString.empty;
			}
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x000AA9DA File Offset: 0x000A8BDA
		public TaggedString(string dat)
		{
			this.rawText = dat;
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x000AA9E3 File Offset: 0x000A8BE3
		public string Resolve()
		{
			return ColoredText.Resolve(this);
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x000AA9F0 File Offset: 0x000A8BF0
		public TaggedString CapitalizeFirst()
		{
			return this.RawText.CapitalizeFirst();
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x000AAA02 File Offset: 0x000A8C02
		public bool NullOrEmpty()
		{
			return this.RawText.NullOrEmpty();
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x000AAA0F File Offset: 0x000A8C0F
		public TaggedString AdjustedFor(Pawn p, string pawnSymbol = "PAWN", bool addRelationInfoSymbol = true)
		{
			return this.RawText.AdjustedFor(p, pawnSymbol, addRelationInfoSymbol);
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x000AAA24 File Offset: 0x000A8C24
		public float GetWidthCached()
		{
			return this.RawText.StripTags().GetWidthCached();
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x000AAA36 File Offset: 0x000A8C36
		public TaggedString Trim()
		{
			return new TaggedString(this.RawText.Trim());
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x000AAA48 File Offset: 0x000A8C48
		public TaggedString Shorten()
		{
			this.rawText = this.rawText.Shorten();
			return this;
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x000AAA61 File Offset: 0x000A8C61
		public TaggedString ToLower()
		{
			return new TaggedString(this.RawText.ToLower());
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x000AAA73 File Offset: 0x000A8C73
		public TaggedString Replace(string oldValue, string newValue)
		{
			return new TaggedString(this.RawText.Replace(oldValue, newValue));
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x000AAA87 File Offset: 0x000A8C87
		public static implicit operator string(TaggedString taggedString)
		{
			return taggedString.RawText.StripTags();
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x000AAA95 File Offset: 0x000A8C95
		public static implicit operator TaggedString(string str)
		{
			return new TaggedString(str);
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x000AAA9D File Offset: 0x000A8C9D
		public static TaggedString operator +(TaggedString t1, TaggedString t2)
		{
			return new TaggedString(t1.RawText + t2.RawText);
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x000AAAB7 File Offset: 0x000A8CB7
		public static TaggedString operator +(string t1, TaggedString t2)
		{
			return new TaggedString(t1 + t2.RawText);
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x000AAACB File Offset: 0x000A8CCB
		public static TaggedString operator +(TaggedString t1, string t2)
		{
			return new TaggedString(t1.RawText + t2);
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x000AAADF File Offset: 0x000A8CDF
		public override string ToString()
		{
			return this.RawText;
		}

		// Token: 0x0400107E RID: 4222
		private string rawText;

		// Token: 0x0400107F RID: 4223
		private static TaggedString empty;
	}
}
