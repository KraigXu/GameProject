using System;

namespace Verse.Grammar
{
	// Token: 0x020004CA RID: 1226
	public class Rule_Number : Rule
	{
		// Token: 0x06002423 RID: 9251 RVA: 0x000D852C File Offset: 0x000D672C
		public override Rule DeepCopy()
		{
			Rule_Number rule_Number = (Rule_Number)base.DeepCopy();
			rule_Number.range = this.range;
			rule_Number.selectionWeight = this.selectionWeight;
			return rule_Number;
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002424 RID: 9252 RVA: 0x000D8551 File Offset: 0x000D6751
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.selectionWeight;
			}
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x000D855C File Offset: 0x000D675C
		public override string Generate()
		{
			return this.range.RandomInRange.ToString();
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x000D857C File Offset: 0x000D677C
		public override string ToString()
		{
			return this.keyword + "->(number: " + this.range.ToString() + ")";
		}

		// Token: 0x040015D2 RID: 5586
		private IntRange range = IntRange.zero;

		// Token: 0x040015D3 RID: 5587
		public int selectionWeight = 1;
	}
}
