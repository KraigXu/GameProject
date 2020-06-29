using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ScenPart_ScatterThingsAnywhere : ScenPart_ScatterThings
	{
		
		// (get) Token: 0x06004A33 RID: 18995 RVA: 0x00010306 File Offset: 0x0000E506
		protected override bool NearPlayerStart
		{
			get
			{
				return false;
			}
		}

		
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "MapScatteredWith", "ScenPart_MapScatteredWith".Translate());
		}

		
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "MapScatteredWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}

		
		public const string MapScatteredWithTag = "MapScatteredWith";
	}
}
