using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ScenPart_ScatterThingsNearPlayerStart : ScenPart_ScatterThings
	{
		
		// (get) Token: 0x06004A37 RID: 18999 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool NearPlayerStart
		{
			get
			{
				return true;
			}
		}

		
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PlayerStartsWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}
	}
}
