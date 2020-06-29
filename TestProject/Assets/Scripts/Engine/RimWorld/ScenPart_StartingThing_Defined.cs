using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ScenPart_StartingThing_Defined : ScenPart_ThingCount
	{
		
		// (get) Token: 0x06004A3B RID: 19003 RVA: 0x001916F8 File Offset: 0x0018F8F8
		public static string PlayerStartWithIntro
		{
			get
			{
				return "ScenPart_StartWith".Translate();
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

		
		public override IEnumerable<Thing> PlayerStartingThings()
		{
			Thing thing = ThingMaker.MakeThing(this.thingDef, this.stuff);
			if (this.thingDef.Minifiable)
			{
				thing = thing.MakeMinified();
			}
			thing.stackCount = this.count;
			yield return thing;
			yield break;
		}

		
		public const string PlayerStartWithTag = "PlayerStartsWith";
	}
}
