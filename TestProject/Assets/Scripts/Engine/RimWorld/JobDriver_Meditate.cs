﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	
	public class JobDriver_Meditate : JobDriver
	{
		
		// (get) Token: 0x06002DA3 RID: 11683 RVA: 0x00100E98 File Offset: 0x000FF098
		public LocalTargetInfo Focus
		{
			get
			{
				return this.job.GetTarget(TargetIndex.C);
			}
		}

		
		// (get) Token: 0x06002DA4 RID: 11684 RVA: 0x00100EA8 File Offset: 0x000FF0A8
		private bool FromBed
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).IsValid;
			}
		}

		
		protected string PsyfocusPerDayReport()
		{
			if (!this.pawn.HasPsylink)
			{
				return "";
			}
			Thing thing = this.Focus.Thing;
			float f = MeditationUtility.PsyfocusGainPerTick(this.pawn, thing) * 60000f;
			return "\n" + "PsyfocusPerDayOfMeditation".Translate(f.ToStringPercent()).CapitalizeFirst();
		}

		
		public override string GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return base.GetReport();
			}
			Thing thing = this.Focus.Thing;
			if (thing != null && !thing.Destroyed)
			{
				return "MeditatingAt".Translate() + ": " + thing.LabelShort.CapitalizeFirst() + "." + this.PsyfocusPerDayReport();
			}
			return base.GetReport() + this.PsyfocusPerDayReport();
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil meditate = new Toil();
			meditate.socialMode = RandomSocialMode.Off;
			if (this.FromBed)
			{
				this.KeepLyingDown(TargetIndex.B);
				meditate = Toils_LayDown.LayDown(TargetIndex.B, this.job.GetTarget(TargetIndex.B).Thing is Building_Bed, false, false, true);
			}
			else
			{
				yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
				meditate.initAction = delegate
				{
					LocalTargetInfo target = this.job.GetTarget(TargetIndex.C);
					if (target.IsValid)
					{
						this.faceDir = target.Cell - this.pawn.Position;
						return;
					}
					this.faceDir = (this.job.def.faceDir.IsValid ? this.job.def.faceDir : Rot4.Random).FacingCell;
				};
				if (this.Focus != null)
				{
					meditate.FailOnDespawnedNullOrForbidden(TargetIndex.C);
				}
				meditate.handlingFacing = true;
			}
			meditate.defaultCompleteMode = ToilCompleteMode.Delay;
			meditate.defaultDuration = this.job.def.joyDuration;
			meditate.FailOn(() => !MeditationUtility.CanMeditateNow(this.pawn) || !MeditationUtility.SafeEnvironmentalConditions(this.pawn, base.TargetLocA, base.Map));
			meditate.AddPreTickAction(delegate
			{
				if (this.faceDir.IsValid && !this.FromBed)
				{
					this.pawn.rotationTracker.FaceCell(this.pawn.Position + this.faceDir);
				}
				this.pawn.GainComfortFromCellIfPossible(false);
				this.MeditationTick();
				if (ModLister.RoyaltyInstalled && MeditationFocusDefOf.Natural.CanPawnUse(this.pawn))
				{
					int num = GenRadial.NumCellsInRadius(MeditationUtility.FocusObjectSearchRadius);
					for (int i = 0; i < num; i++)
					{
						IntVec3 c = this.pawn.Position + GenRadial.RadialPattern[i];
						if (c.InBounds(this.pawn.Map))
						{
							Plant plant = c.GetPlant(this.pawn.Map);
							if (plant != null && plant.def == ThingDefOf.Plant_TreeAnima)
							{
								CompSpawnSubplant compSpawnSubplant = plant.TryGetComp<CompSpawnSubplant>();
								if (compSpawnSubplant != null)
								{
									compSpawnSubplant.AddProgress(JobDriver_Meditate.AnimaTreeSubplantProgressPerTick);
								}
							}
						}
					}
				}
			});
			meditate.AddFinishAction(delegate
			{
				if (this.sustainer != null)
				{
					this.sustainer.End();
				}
			});
			yield return meditate;
			yield break;
		}

		
		protected void MeditationTick()
		{
			this.pawn.skills.Learn(SkillDefOf.Intellectual, 0.0180000011f, false);
			if (this.pawn.needs.joy != null)
			{
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.None, 1f, null);
			}
			if (this.pawn.IsHashIntervalTick(100))
			{
				MoteMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, ThingDefOf.Mote_Meditating);
			}
			if (ModsConfig.RoyaltyActive)
			{
				this.pawn.psychicEntropy.Notify_Meditated();
				if (this.pawn.HasPsylink && this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) > 1.401298E-45f)
				{
					if (this.psyfocusMote == null || this.psyfocusMote.Destroyed)
					{
						this.psyfocusMote = MoteMaker.MakeAttachedOverlay(this.pawn, ThingDefOf.Mote_PsyfocusPulse, Vector3.zero, 1f, -1f);
					}
					this.psyfocusMote.Maintain();
					if (this.sustainer == null || this.sustainer.Ended)
					{
						this.sustainer = SoundDefOf.MeditationGainPsyfocus.TrySpawnSustainer(SoundInfo.InMap(this.pawn, MaintenanceType.None));
					}
					else
					{
						this.sustainer.Maintain();
					}
					this.pawn.psychicEntropy.GainPsyfocus(this.Focus.Thing);
				}
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.faceDir, "faceDir", default(IntVec3), false);
		}

		
		protected IntVec3 faceDir;

		
		private Mote psyfocusMote;

		
		private Sustainer sustainer;

		
		protected const TargetIndex SpotInd = TargetIndex.A;

		
		protected const TargetIndex BedInd = TargetIndex.B;

		
		protected const TargetIndex FocusInd = TargetIndex.C;

		
		public static float AnimaTreeSubplantProgressPerTick = 6.666667E-05f;

		
		private const int TicksBetweenMotesBase = 100;
	}
}
