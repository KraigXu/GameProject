using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CF3 RID: 3315
	public class CompBiocodableWeapon : CompBiocodable
	{
		// Token: 0x0600509B RID: 20635 RVA: 0x001B1824 File Offset: 0x001AFA24
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.biocoded)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Stat_Thing_Biocoded_Name".Translate(), this.codedPawnLabel, "Stat_Thing_Biocoded_Desc".Translate(), 5404, null, null, false);
			}
			yield break;
		}
	}
}
