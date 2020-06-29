using System;

namespace Verse
{
	
	public struct LabelValue
	{
		
		
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		
		
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		
		public LabelValue(string label, string value)
		{
			this = default(LabelValue);
			this.label = label;
			this.value = value;
		}

		
		public override string ToString()
		{
			return this.label;
		}

		
		private string label;

		
		private string value;
	}
}
