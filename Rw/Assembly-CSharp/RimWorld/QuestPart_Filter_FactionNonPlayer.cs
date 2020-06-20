using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000951 RID: 2385
	public class QuestPart_Filter_FactionNonPlayer : QuestPart_Filter
	{
		// Token: 0x06003878 RID: 14456 RVA: 0x0012E360 File Offset: 0x0012C560
		protected override bool Pass(SignalArgs args)
		{
			Faction faction;
			if (args.TryGetArg<Faction>("FACTION", out faction))
			{
				return faction != Faction.OfPlayer;
			}
			Thing thing;
			return args.TryGetArg<Thing>("SUBJECT", out thing) && thing.Faction != Faction.OfPlayer;
		}
	}
}
