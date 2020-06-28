using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200066A RID: 1642
	public class JobDriver_Mine : JobDriver
	{
		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06002CC8 RID: 11464 RVA: 0x000FE7A4 File Offset: 0x000FC9A4
		private Thing MineTarget
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x000FE7C5 File Offset: 0x000FC9C5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.MineTarget, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002CCA RID: 11466 RVA: 0x000FE7E7 File Offset: 0x000FC9E7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnCellMissingDesignation(TargetIndex.A, DesignationDefOf.Mine);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil mine = new Toil();
			mine.tickAction = delegate
			{
				Pawn actor = mine.actor;
				Thing mineTarget = this.MineTarget;
				if (this.ticksToPickHit < -100)
				{
					this.ResetTicksToPickHit();
				}
				if (actor.skills != null && (mineTarget.Faction != actor.Faction || actor.Faction == null))
				{
					actor.skills.Learn(SkillDefOf.Mining, 0.07f, false);
				}
				this.ticksToPickHit--;
				if (this.ticksToPickHit <= 0)
				{
					IntVec3 position = mineTarget.Position;
					if (this.effecter == null)
					{
						this.effecter = EffecterDefOf.Mine.Spawn();
					}
					this.effecter.Trigger(actor, mineTarget);
					int num = mineTarget.def.building.isNaturalRock ? 80 : 40;
					Mineable mineable = mineTarget as Mineable;
					if (mineable == null || mineTarget.HitPoints > num)
					{
						DamageInfo dinfo = new DamageInfo(DamageDefOf.Mining, (float)num, 0f, -1f, mine.actor, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
						mineTarget.TakeDamage(dinfo);
					}
					else
					{
						mineable.Notify_TookMiningDamage(mineTarget.HitPoints, mine.actor);
						mineable.HitPoints = 0;
						mineable.DestroyMined(actor);
					}
					if (mineTarget.Destroyed)
					{
						actor.Map.mineStrikeManager.CheckStruckOre(position, mineTarget.def, actor);
						actor.records.Increment(RecordDefOf.CellsMined);
						if (this.pawn.Faction != Faction.OfPlayer)
						{
							List<Thing> thingList = position.GetThingList(this.Map);
							for (int i = 0; i < thingList.Count; i++)
							{
								thingList[i].SetForbidden(true, false);
							}
						}
						if (this.pawn.Faction == Faction.OfPlayer && MineStrikeManager.MineableIsVeryValuable(mineTarget.def))
						{
							TaleRecorder.RecordTale(TaleDefOf.MinedValuable, new object[]
							{
								this.pawn,
								mineTarget.def.building.mineableThing
							});
						}
						if (this.pawn.Faction == Faction.OfPlayer && MineStrikeManager.MineableIsValuable(mineTarget.def) && !this.pawn.Map.IsPlayerHome)
						{
							TaleRecorder.RecordTale(TaleDefOf.CaravanRemoteMining, new object[]
							{
								this.pawn,
								mineTarget.def.building.mineableThing
							});
						}
						this.ReadyForNextToil();
						return;
					}
					this.ResetTicksToPickHit();
				}
			};
			mine.defaultCompleteMode = ToilCompleteMode.Never;
			mine.WithProgressBar(TargetIndex.A, () => 1f - (float)this.MineTarget.HitPoints / (float)this.MineTarget.MaxHitPoints, false, -0.5f);
			mine.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			mine.activeSkill = (() => SkillDefOf.Mining);
			yield return mine;
			yield break;
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x000FE7F8 File Offset: 0x000FC9F8
		private void ResetTicksToPickHit()
		{
			float num = this.pawn.GetStatValue(StatDefOf.MiningSpeed, true);
			if (num < 0.6f && this.pawn.Faction != Faction.OfPlayer)
			{
				num = 0.6f;
			}
			this.ticksToPickHit = (int)Math.Round((double)(100f / num));
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x000FE84B File Offset: 0x000FCA4B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToPickHit, "ticksToPickHit", 0, false);
		}

		// Token: 0x040019F7 RID: 6647
		private int ticksToPickHit = -1000;

		// Token: 0x040019F8 RID: 6648
		private Effecter effecter;

		// Token: 0x040019F9 RID: 6649
		public const int BaseTicksBetweenPickHits = 100;

		// Token: 0x040019FA RID: 6650
		private const int BaseDamagePerPickHit_NaturalRock = 80;

		// Token: 0x040019FB RID: 6651
		private const int BaseDamagePerPickHit_NotNaturalRock = 40;

		// Token: 0x040019FC RID: 6652
		private const float MinMiningSpeedFactorForNPCs = 0.6f;
	}
}
