using System;

namespace Verse
{
	
	public struct TaggedString
	{
		private string rawText;
		private static TaggedString empty;


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

		
		
		public int Length
		{
			get
			{
				return this.RawText.Length;
			}
		}

		
		
		public int StrippedLength
		{
			get
			{
				return this.RawText.StripTags().Length;
			}
		}

		
		
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
	}
}
