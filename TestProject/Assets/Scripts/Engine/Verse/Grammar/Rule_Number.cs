using System;

namespace Verse.Grammar
{
	
	public class Rule_Number : Rule
	{
		
		public override Rule DeepCopy()
		{
			Rule_Number rule_Number = (Rule_Number)base.DeepCopy();
			rule_Number.range = this.range;
			rule_Number.selectionWeight = this.selectionWeight;
			return rule_Number;
		}

		
		// (get) Token: 0x06002424 RID: 9252 RVA: 0x000D8551 File Offset: 0x000D6751
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.selectionWeight;
			}
		}

		
		public override string Generate()
		{
			return this.range.RandomInRange.ToString();
		}

		
		public override string ToString()
		{
			return this.keyword + "->(number: " + this.range.ToString() + ")";
		}

		
		private IntRange range = IntRange.zero;

		
		public int selectionWeight = 1;
	}
}
