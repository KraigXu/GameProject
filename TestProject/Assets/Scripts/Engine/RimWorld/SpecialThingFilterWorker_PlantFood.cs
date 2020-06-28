using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE3 RID: 4067
	public class SpecialThingFilterWorker_PlantFood : SpecialThingFilterWorker
	{
		// Token: 0x060061AD RID: 25005 RVA: 0x0021FC1F File Offset: 0x0021DE1F
		public override bool Matches(Thing t)
		{
			return this.AlwaysMatches(t.def);
		}

		// Token: 0x060061AE RID: 25006 RVA: 0x0021FC2D File Offset: 0x0021DE2D
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.ingestible != null && (def.ingestible.foodType & FoodTypeFlags.Plant) > FoodTypeFlags.None;
		}

		// Token: 0x060061AF RID: 25007 RVA: 0x0021FC4A File Offset: 0x0021DE4A
		public override bool CanEverMatch(ThingDef def)
		{
			return this.AlwaysMatches(def);
		}
	}
}
