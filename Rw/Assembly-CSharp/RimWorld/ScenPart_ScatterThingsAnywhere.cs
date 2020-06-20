using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C28 RID: 3112
	public class ScenPart_ScatterThingsAnywhere : ScenPart_ScatterThings
	{
		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x06004A33 RID: 18995 RVA: 0x00010306 File Offset: 0x0000E506
		protected override bool NearPlayerStart
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004A34 RID: 18996 RVA: 0x001916A6 File Offset: 0x0018F8A6
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "MapScatteredWith", "ScenPart_MapScatteredWith".Translate());
		}

		// Token: 0x06004A35 RID: 18997 RVA: 0x001916C2 File Offset: 0x0018F8C2
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "MapScatteredWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x04002A21 RID: 10785
		public const string MapScatteredWithTag = "MapScatteredWith";
	}
}
