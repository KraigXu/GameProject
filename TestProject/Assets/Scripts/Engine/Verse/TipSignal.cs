using System;

namespace Verse
{
	
	public struct TipSignal
	{
		
		public TipSignal(string text, int uniqueId)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		
		public TipSignal(string text, int uniqueId, TooltipPriority priority)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = priority;
			this.delay = 0.45f;
		}

		
		public TipSignal(string text)
		{
			if (text == null)
			{
				text = "";
			}
			this.text = text;
			this.textGetter = null;
			this.uniqueId = text.GetHashCode();
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		
		public TipSignal(string text, float delay)
		{
			if (text == null)
			{
				text = "";
			}
			this.text = text;
			this.textGetter = null;
			this.uniqueId = text.GetHashCode();
			this.priority = TooltipPriority.Default;
			this.delay = delay;
		}

		
		public TipSignal(TaggedString text)
		{
			if (text == null)
			{
				text = "";
			}
			this.text = text.Resolve();
			this.textGetter = null;
			this.uniqueId = text.GetHashCode();
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		
		public TipSignal(Func<string> textGetter, int uniqueId)
		{
			this.text = "";
			this.textGetter = textGetter;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		
		public TipSignal(Func<string> textGetter, int uniqueId, TooltipPriority priority)
		{
			this.text = "";
			this.textGetter = textGetter;
			this.uniqueId = uniqueId;
			this.priority = priority;
			this.delay = 0.45f;
		}

		
		public TipSignal(TipSignal cloneSource)
		{
			this.text = cloneSource.text;
			this.textGetter = null;
			this.priority = cloneSource.priority;
			this.uniqueId = cloneSource.uniqueId;
			this.delay = 0.45f;
		}

		
		public static implicit operator TipSignal(string str)
		{
			return new TipSignal(str);
		}

		
		public static implicit operator TipSignal(TaggedString str)
		{
			return new TipSignal(str);
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Tip(",
				this.text,
				", ",
				this.uniqueId,
				")"
			});
		}

		
		public const float DefaultDelay = 0.45f;

		
		public string text;

		
		public Func<string> textGetter;

		
		public int uniqueId;

		
		public TooltipPriority priority;

		
		public float delay;
	}
}
