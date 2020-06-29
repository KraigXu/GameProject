using System;
using Verse;

namespace RimWorld
{
	
	public class CompGiveThoughtToAllMapPawnsOnDestroy : ThingComp
	{
		
		
		private CompProperties_GiveThoughtToAllMapPawnsOnDestroy Props
		{
			get
			{
				return (CompProperties_GiveThoughtToAllMapPawnsOnDestroy)this.props;
			}
		}

		
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (previousMap != null)
			{
				if (!this.Props.message.NullOrEmpty())
				{
					Messages.Message(this.Props.message, new TargetInfo(this.parent.Position, previousMap, false), MessageTypeDefOf.NegativeEvent, true);
				}
				foreach (Pawn pawn in previousMap.mapPawns.AllPawnsSpawned)
				{
					Pawn_NeedsTracker needs = pawn.needs;
					if (needs != null)
					{
						Need_Mood mood = needs.mood;
						if (mood != null)
						{
							mood.thoughts.memories.TryGainMemory(this.Props.thought, null);
						}
					}
				}
			}
		}
	}
}
