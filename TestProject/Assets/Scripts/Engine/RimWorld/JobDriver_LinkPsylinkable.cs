using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	
	public class JobDriver_LinkPsylinkable : JobDriver
	{
		
		// (get) Token: 0x06002CA9 RID: 11433 RVA: 0x000FE0F0 File Offset: 0x000FC2F0
		private Thing PsylinkableThing
		{
			get
			{
				return base.TargetA.Thing;
			}
		}

		
		// (get) Token: 0x06002CAA RID: 11434 RVA: 0x000FE10B File Offset: 0x000FC30B
		private CompPsylinkable Psylinkable
		{
			get
			{
				return this.PsylinkableThing.TryGetComp<CompPsylinkable>();
			}
		}

		
		// (get) Token: 0x06002CAB RID: 11435 RVA: 0x000DF673 File Offset: 0x000DD873
		private LocalTargetInfo LinkSpot
		{
			get
			{
				return this.job.targetB;
			}
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.PsylinkableThing, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.LinkSpot, this.job, 1, -1, null, errorOnFailed);
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Psylinkables are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 5464564, false);
				yield break;
			}
			base.AddFailCondition(() => !this.Psylinkable.CanPsylink(this.pawn, new LocalTargetInfo?(this.LinkSpot)).Accepted);
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil toil = Toils_General.Wait(15000, TargetIndex.None);
			toil.tickAction = delegate
			{
				this.pawn.rotationTracker.FaceTarget(this.PsylinkableThing);
				if (Find.TickManager.TicksGame % 720 == 0)
				{
					Vector3 vector = this.pawn.Position.ToVector3();
					vector += (this.PsylinkableThing.Position.ToVector3() - vector) * Rand.Value;
					MoteMaker.MakeStaticMote(vector, this.pawn.Map, ThingDefOf.Mote_PsycastAreaEffect, 0.5f);
					this.Psylinkable.Props.linkSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(this.PsylinkableThing), MaintenanceType.None));
				}
			};
			toil.handlingFacing = false;
			toil.socialMode = RandomSocialMode.Off;
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				this.Psylinkable.FinishLinkingRitual(this.pawn);
			});
			yield break;
		}

		
		public const int LinkTimeTicks = 15000;

		
		public const int EffectsTickInterval = 720;

		
		protected const TargetIndex PsylinkableInd = TargetIndex.A;

		
		protected const TargetIndex LinkSpotInd = TargetIndex.B;
	}
}
