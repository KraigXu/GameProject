using System;

namespace Verse
{
	
	public struct LabelValue
	{
		
		// (get) Token: 0x06001D63 RID: 7523 RVA: 0x000B493A File Offset: 0x000B2B3A
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		
		// (get) Token: 0x06001D64 RID: 7524 RVA: 0x000B4942 File Offset: 0x000B2B42
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
