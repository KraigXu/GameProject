using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B9 RID: 2233
	public class ChemicalDef : Def
	{
		// Token: 0x060035D7 RID: 13783 RVA: 0x00124B7B File Offset: 0x00122D7B
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.addictionHediff == null)
			{
				yield return "addictionHediff is null";
			}
			yield break;
			yield break;
		}

		// Token: 0x04001DA2 RID: 7586
		public HediffDef addictionHediff;

		// Token: 0x04001DA3 RID: 7587
		public HediffDef toleranceHediff;

		// Token: 0x04001DA4 RID: 7588
		public bool canBinge = true;

		// Token: 0x04001DA5 RID: 7589
		public float onGeneratedAddictedToleranceChance;

		// Token: 0x04001DA6 RID: 7590
		public List<HediffGiver_Event> onGeneratedAddictedEvents;
	}
}
