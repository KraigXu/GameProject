using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006EF RID: 1775
	public abstract class JoyGiver_InteractBuilding : JoyGiver
	{
		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06002F0E RID: 12046 RVA: 0x00010306 File Offset: 0x0000E506
		protected virtual bool CanDoDuringGathering
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x00108BEC File Offset: 0x00106DEC
		public override Job TryGiveJob(Pawn pawn)
		{
			Thing thing = this.FindBestGame(pawn, false, IntVec3.Invalid);
			if (thing != null)
			{
				return this.TryGivePlayJob(pawn, thing);
			}
			return null;
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x00108C14 File Offset: 0x00106E14
		public override Job TryGiveJobWhileInBed(Pawn pawn)
		{
			Thing thing = this.FindBestGame(pawn, true, IntVec3.Invalid);
			if (thing != null)
			{
				return this.TryGivePlayJobWhileInBed(pawn, thing);
			}
			return null;
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x00108C3C File Offset: 0x00106E3C
		public override Job TryGiveJobInGatheringArea(Pawn pawn, IntVec3 gatheringSpot)
		{
			if (!this.CanDoDuringGathering)
			{
				return null;
			}
			Thing thing = this.FindBestGame(pawn, false, gatheringSpot);
			if (thing != null)
			{
				return this.TryGivePlayJob(pawn, thing);
			}
			return null;
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x00108C6C File Offset: 0x00106E6C
		private Thing FindBestGame(Pawn pawn, bool inBed, IntVec3 gatheringSpot)
		{
			JoyGiver_InteractBuilding.tmpCandidates.Clear();
			this.GetSearchSet(pawn, JoyGiver_InteractBuilding.tmpCandidates);
			if (JoyGiver_InteractBuilding.tmpCandidates.Count == 0)
			{
				return null;
			}
			Predicate<Thing> predicate = (Thing t) => this.CanInteractWith(pawn, t, inBed);
			if (gatheringSpot.IsValid)
			{
				Predicate<Thing> oldValidator = predicate;
				predicate = ((Thing x) => GatheringsUtility.InGatheringArea(x.Position, gatheringSpot, pawn.Map) && oldValidator(x));
			}
			Thing result = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, JoyGiver_InteractBuilding.tmpCandidates, PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, predicate, null);
			JoyGiver_InteractBuilding.tmpCandidates.Clear();
			return result;
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x00108D40 File Offset: 0x00106F40
		protected virtual bool CanInteractWith(Pawn pawn, Thing t, bool inBed)
		{
			if (!pawn.CanReserve(t, this.def.jobDef.joyMaxParticipants, -1, null, false))
			{
				return false;
			}
			if (t.IsForbidden(pawn))
			{
				return false;
			}
			if (!t.IsSociallyProper(pawn))
			{
				return false;
			}
			if (!t.IsPoliticallyProper(pawn))
			{
				return false;
			}
			CompPowerTrader compPowerTrader = t.TryGetComp<CompPowerTrader>();
			return (compPowerTrader == null || compPowerTrader.PowerOn) && (!this.def.unroofedOnly || !t.Position.Roofed(t.Map));
		}

		// Token: 0x06002F14 RID: 12052
		protected abstract Job TryGivePlayJob(Pawn pawn, Thing bestGame);

		// Token: 0x06002F15 RID: 12053 RVA: 0x00108DC8 File Offset: 0x00106FC8
		protected virtual Job TryGivePlayJobWhileInBed(Pawn pawn, Thing bestGame)
		{
			Building_Bed t = pawn.CurrentBed();
			return JobMaker.MakeJob(this.def.jobDef, bestGame, pawn.Position, t);
		}

		// Token: 0x04001AA7 RID: 6823
		private static List<Thing> tmpCandidates = new List<Thing>();
	}
}
