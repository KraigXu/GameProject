using System;

namespace Verse
{
	
	public struct TaggedString
	{
		
		// (get) Token: 0x06001BF6 RID: 7158 RVA: 0x000AA983 File Offset: 0x000A8B83
		public string RawText
		{
			get
			{
				return this.rawText;
			}
		}

		
		public char this[int i]
		{
			get
			{
				return this.RawText[i];
			}
		}

		
		// (get) Token: 0x06001BF8 RID: 7160 RVA: 0x000AA999 File Offset: 0x000A8B99
		public int Length
		{
			get
			{
				return this.RawText.Length;
			}
		}

		
		// (get) Token: 0x06001BF9 RID: 7161 RVA: 0x000AA9A6 File Offset: 0x000A8BA6
		public int StrippedLength
		{
			get
			{
				return this.RawText.StripTags().Length;
			}
		}

		
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

		
		public TaggedString(string dat)
		{
			this.rawText = dat;
		}

		
		public string Resolve()
		{
			return ColoredText.Resolve(this);
		}

		
		public TaggedString CapitalizeFirst()
		{
			return this.RawText.CapitalizeFirst();
		}

		
		public bool NullOrEmpty()
		{
			return this.RawText.NullOrEmpty();
		}

		
		public TaggedString AdjustedFor(Pawn p, string pawnSymbol = "PAWN", bool addRelationInfoSymbol = true)
		{
			return this.RawText.AdjustedFor(p, pawnSymbol, addRelationInfoSymbol);
		}

		
		public float GetWidthCached()
		{
			return this.RawText.StripTags().GetWidthCached();
		}

		
		public TaggedString Trim()
		{
			return new TaggedString(this.RawText.Trim());
		}

		
		public TaggedString Shorten()
		{
			this.rawText = this.rawText.Shorten();
			return this;
		}

		
		public TaggedString ToLower()
		{
			return new TaggedString(this.RawText.ToLower());
		}

		
		public TaggedString Replace(string oldValue, string newValue)
		{
			return new TaggedString(this.RawText.Replace(oldValue, newValue));
		}

		
		public static implicit operator string(TaggedString taggedString)
		{
			return taggedString.RawText.StripTags();
		}

		
		public static implicit operator TaggedString(string str)
		{
			return new TaggedString(str);
		}

		
		public static TaggedString operator +(TaggedString t1, TaggedString t2)
		{
			return new TaggedString(t1.RawText + t2.RawText);
		}

		
		public static TaggedString operator +(string t1, TaggedString t2)
		{
			return new TaggedString(t1 + t2.RawText);
		}

		
		public static TaggedString operator +(TaggedString t1, string t2)
		{
			return new TaggedString(t1.RawText + t2);
		}

		
		public override string ToString()
		{
			return this.RawText;
		}

		
		private string rawText;

		
		private static TaggedString empty;
	}
}
