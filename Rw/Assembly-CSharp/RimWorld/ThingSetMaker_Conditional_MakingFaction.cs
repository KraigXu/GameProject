using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CC7 RID: 3271
	public class ThingSetMaker_Conditional_MakingFaction : ThingSetMaker_Conditional
	{
		// Token: 0x06004F51 RID: 20305 RVA: 0x001AB688 File Offset: 0x001A9888
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return (!this.requireNonNull || parms.makingFaction != null) && (this.makingFaction == null || (parms.makingFaction != null && parms.makingFaction.def == this.makingFaction)) && (this.makingFactionCategories.NullOrEmpty<string>() || (parms.makingFaction != null && this.makingFactionCategories.Contains(parms.makingFaction.def.categoryTag)));
		}

		// Token: 0x04002C83 RID: 11395
		public FactionDef makingFaction;

		// Token: 0x04002C84 RID: 11396
		public List<string> makingFactionCategories;

		// Token: 0x04002C85 RID: 11397
		public bool requireNonNull;
	}
}
