using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CF1 RID: 3313
	public class CompBiocodableApparel : CompBiocodable
	{
		// Token: 0x06005098 RID: 20632 RVA: 0x001B17F4 File Offset: 0x001AF9F4
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.biocoded)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_Biocoded_Name".Translate(), this.codedPawnLabel, "Stat_Thing_Biocoded_Desc".Translate(), 2753, null, null, false);
			}
			yield break;
		}
	}
}
