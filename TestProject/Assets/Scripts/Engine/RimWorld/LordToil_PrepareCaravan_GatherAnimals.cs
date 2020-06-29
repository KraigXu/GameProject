﻿using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordToil_PrepareCaravan_GatherAnimals : LordToil
	{
		
		
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		
		
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		
		public LordToil_PrepareCaravan_GatherAnimals(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.IsColonist || pawn.RaceProps.Animal)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_GatherPawns, this.meetingPoint, -1f);
					pawn.mindState.duty.pawnsToGather = PawnsToGather.Animals;
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait);
				}
			}
		}

		
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				GatherAnimalsAndSlavesForCaravanUtility.CheckArrived(this.lord, this.lord.ownedPawns, this.meetingPoint, "AllAnimalsGathered", (Pawn x) => x.RaceProps.Animal, (Pawn x) => GatherAnimalsAndSlavesForCaravanUtility.IsFollowingAnyone(x));
			}
		}

		
		private IntVec3 meetingPoint;
	}
}
