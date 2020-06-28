using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE6 RID: 4070
	public class SpecialThingFilterWorker_NonSmeltableWeapons : SpecialThingFilterWorker
	{
		// Token: 0x060061B9 RID: 25017 RVA: 0x0021FC88 File Offset: 0x0021DE88
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.Smeltable;
		}

		// Token: 0x060061BA RID: 25018 RVA: 0x0021FCCC File Offset: 0x0021DECC
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

		// Token: 0x060061BB RID: 25019 RVA: 0x0021FD2A File Offset: 0x0021DF2A
		public override bool AlwaysMatches(ThingDef def)
		{
			return this.CanEverMatch(def) && !def.smeltable && !def.MadeFromStuff;
		}
	}
}
