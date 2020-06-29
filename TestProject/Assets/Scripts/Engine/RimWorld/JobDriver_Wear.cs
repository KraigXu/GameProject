using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_Wear : JobDriver
	{
		
		// (get) Token: 0x06002DDD RID: 11741 RVA: 0x00102268 File Offset: 0x00100468
		private Apparel Apparel
		{
			get
			{
				return (Apparel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<int>(ref this.unequipBuffer, "unequipBuffer", 0, false);
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Apparel, this.job, 1, -1, null, errorOnFailed);
		}

		
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.duration = (int)(this.Apparel.GetStatValue(StatDefOf.EquipDelay, true) * 60f);
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
			for (int i = wornApparel.Count - 1; i >= 0; i--)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.pawn.RaceProps.body))
				{
					this.duration += (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true) * 60f);
				}
			}
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil toil = new Toil();
			toil.tickAction = delegate
			{
				this.unequipBuffer++;
				this.TryUnequipSomething();
			};
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.duration;
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				Apparel apparel = this.Apparel;
				this.pawn.apparel.Wear(apparel, true, false);
				if (this.pawn.outfits != null && this.job.playerForced)
				{
					this.pawn.outfits.forcedHandler.SetForced(apparel, true);
				}
			});
			yield break;
		}

		
		private void TryUnequipSomething()
		{
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
			int i = wornApparel.Count - 1;
			while (i >= 0)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.pawn.RaceProps.body))
				{
					int num = (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true) * 60f);
					if (this.unequipBuffer < num)
					{
						break;
					}
					bool forbid = this.pawn.Faction != null && this.pawn.Faction.HostileTo(Faction.OfPlayer);
					Apparel apparel2;
					if (!this.pawn.apparel.TryDrop(wornApparel[i], out apparel2, this.pawn.PositionHeld, forbid))
					{
						Log.Error(this.pawn + " could not drop " + wornApparel[i].ToStringSafe<Apparel>(), false);
						base.EndJobWith(JobCondition.Errored);
						return;
					}
					break;
				}
				else
				{
					i--;
				}
			}
		}

		
		private int duration;

		
		private int unequipBuffer;

		
		private const TargetIndex ApparelInd = TargetIndex.A;
	}
}
