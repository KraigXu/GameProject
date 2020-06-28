using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE9 RID: 4073
	public class SpecialThingFilterWorker_NonBurnableWeapons : SpecialThingFilterWorker
	{
		// Token: 0x060061C5 RID: 25029 RVA: 0x0021FD7D File Offset: 0x0021DF7D
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.BurnableByRecipe;
		}

		// Token: 0x060061C6 RID: 25030 RVA: 0x0021FDC0 File Offset: 0x0021DFC0
		public override bool CanEverMatch(ThingDef def)
		{
			if (!def.IsWeapon)
			{
				return false;
			}
			if (!def.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				for (int i = 0; i < def.thingCategories.Count; i++)
				{
					for (ThingCategoryDef thingCategoryDef = def.thingCategories[i]; thingCategoryDef != null; thingCategoryDef = thingCategoryDef.parent)
					{
						if (thingCategoryDef == ThingCategoryDefOf.Weapons)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060061C7 RID: 25031 RVA: 0x0021FE1E File Offset: 0x0021E01E
		public override bool AlwaysMatches(ThingDef def)
		{
			return this.CanEverMatch(def) && !def.burnableByRecipe && !def.MadeFromStuff;
		}
	}
}
