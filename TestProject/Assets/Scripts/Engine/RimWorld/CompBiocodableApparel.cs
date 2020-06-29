using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class CompBiocodableApparel : CompBiocodable
	{
		
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
