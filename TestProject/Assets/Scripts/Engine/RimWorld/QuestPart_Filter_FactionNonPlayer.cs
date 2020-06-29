using System;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_Filter_FactionNonPlayer : QuestPart_Filter
	{
		
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
