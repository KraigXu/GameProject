using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C2A RID: 3114
	public class ScenPart_StartingThing_Defined : ScenPart_ThingCount
	{
		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x06004A3B RID: 19003 RVA: 0x001916F8 File Offset: 0x0018F8F8
		public static string PlayerStartWithIntro
		{
			get
			{
				return "ScenPart_StartWith".Translate();
			}
		}

		// Token: 0x06004A3C RID: 19004 RVA: 0x00190E9C File Offset: 0x0018F09C
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x06004A3D RID: 19005 RVA: 0x00191709 File Offset: 0x0018F909
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PlayerStartsWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x06004A3E RID: 19006 RVA: 0x00191720 File Offset: 0x0018F920
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

		// Token: 0x04002A22 RID: 10786
		public const string PlayerStartWithTag = "PlayerStartsWith";
	}
}
