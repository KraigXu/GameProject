using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public abstract class JoyGiver
	{
		
		public virtual float GetChance(Pawn pawn)
		{
			return this.def.baseChance;
		}

		
		protected virtual void GetSearchSet(Pawn pawn, List<Thing> outCandidates)
		{
			outCandidates.Clear();
			if (this.def.thingDefs == null)
			{
				return;
			}
			if (this.def.thingDefs.Count == 1)
			{
				outCandidates.AddRange(pawn.Map.listerThings.ThingsOfDef(this.def.thingDefs[0]));
				return;
			}
			for (int i = 0; i < this.def.thingDefs.Count; i++)
			{
				outCandidates.AddRange(pawn.Map.listerThings.ThingsOfDef(this.def.thingDefs[i]));
			}
		}

		
		public abstract Job TryGiveJob(Pawn pawn);

		
		public virtual Job TryGiveJobWhileInBed(Pawn pawn)
		{
			return null;
		}

		
		public virtual Job TryGiveJobInGatheringArea(Pawn pawn, IntVec3 gatherSpot)
		{
			return null;
		}

		
		public virtual bool CanBeGivenTo(Pawn pawn)
		{
			return this.MissingRequiredCapacity(pawn) == null && this.def.joyKind.PawnCanDo(pawn);
		}

		
		public PawnCapacityDef MissingRequiredCapacity(Pawn pawn)
		{
			for (int i = 0; i < this.def.requiredCapacities.Count; i++)
			{
				if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[i]))
				{
					return this.def.requiredCapacities[i];
				}
			}
			return null;
		}

		
		public JoyGiverDef def;
	}
}
