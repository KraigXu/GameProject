using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class HediffComp_PsychicHarmonizer : HediffComp
	{
		
		// (get) Token: 0x06006426 RID: 25638 RVA: 0x0022B1B6 File Offset: 0x002293B6
		public HediffCompProperties_PsychicHarmonizer Props
		{
			get
			{
				return (HediffCompProperties_PsychicHarmonizer)this.props;
			}
		}

		
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			Pawn pawn = this.parent.pawn;
			if (pawn.IsHashIntervalTick(150))
			{
				return;
			}
			if (pawn.needs == null || pawn.needs.mood == null || pawn.Faction == null)
			{
				return;
			}
			if (pawn.Spawned)
			{
				List<Pawn> pawns = pawn.Map.mapPawns.PawnsInFaction(pawn.Faction);
				this.AffectPawns(pawn, pawns);
				return;
			}
			Caravan caravan = pawn.GetCaravan();
			if (caravan != null)
			{
				this.AffectPawns(pawn, caravan.pawns.InnerListForReading);
			}
		}

		
		private void AffectPawns(Pawn p, List<Pawn> pawns)
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (p != pawn && p.RaceProps.Humanlike && pawn.needs != null && pawn.needs.mood != null && pawn.needs.mood.thoughts != null && (!p.Spawned || !pawn.Spawned || pawn.Position.DistanceTo(p.Position) <= this.Props.range) && !pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicHarmonizer, false))
				{
					bool flag = false;
					foreach (Thought_Memory thought_Memory in pawn.needs.mood.thoughts.memories.Memories)
					{
						Thought_PsychicHarmonizer thought_PsychicHarmonizer = thought_Memory as Thought_PsychicHarmonizer;
						if (thought_PsychicHarmonizer != null && thought_PsychicHarmonizer.harmonizer == this.parent)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						Thought_PsychicHarmonizer thought_PsychicHarmonizer2 = (Thought_PsychicHarmonizer)ThoughtMaker.MakeThought(this.Props.thought);
						thought_PsychicHarmonizer2.harmonizer = this.parent;
						thought_PsychicHarmonizer2.otherPawn = this.parent.pawn;
						pawn.needs.mood.thoughts.memories.TryGainMemory(thought_PsychicHarmonizer2, null);
					}
				}
			}
		}
	}
}
