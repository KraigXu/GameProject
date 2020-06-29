using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class Psycast : Ability
	{
		
		
		public override bool CanCast
		{
			get
			{
				if (!base.CanCast)
				{
					return false;
				}
				if (this.def.EntropyGain > 1.401298E-45f)
				{
					Hediff hediff = this.pawn.health.hediffSet.hediffs.FirstOrDefault((Hediff h) => h.def == HediffDefOf.PsychicAmplifier);
					return ((hediff != null && hediff.Severity >= (float)this.def.level) || this.def.level <= 0) && !this.pawn.psychicEntropy.WouldOverflowEntropy(this.def.EntropyGain);
				}
				return this.def.PsyfocusCost <= this.pawn.psychicEntropy.CurrentPsyfocus;
			}
		}

		
		public Psycast(Pawn pawn) : base(pawn)
		{
		}

		
		public Psycast(Pawn pawn, AbilityDef def) : base(pawn, def)
		{
		}

		
		public override IEnumerable<Command> GetGizmos()
		{
			if (this.gizmo == null)
			{
				this.gizmo = new Command_Psycast(this);
			}
			yield return this.gizmo;
			yield break;
		}

		
		public override bool Activate(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (this.def.EntropyGain > 1.401298E-45f && !this.pawn.psychicEntropy.TryAddEntropy(this.def.EntropyGain, null, true, false))
			{
				return false;
			}
			if (this.def.PsyfocusCost > 1.401298E-45f)
			{
				this.pawn.psychicEntropy.OffsetPsyfocusDirectly(-this.def.PsyfocusCost);
			}
			bool flag = base.EffectComps.Any((CompAbilityEffect c) => c.Props.psychic);
			if (flag)
			{
				if (this.def.HasAreaOfEffect)
				{
					MoteMaker.MakeStaticMote(target.Cell, this.pawn.Map, ThingDefOf.Mote_PsycastAreaEffect, this.def.EffectRadius);
					SoundDefOf.PsycastPsychicPulse.PlayOneShot(new TargetInfo(target.Cell, this.pawn.Map, false));
				}
				else
				{
					SoundDefOf.PsycastPsychicEffect.PlayOneShot(new TargetInfo(target.Cell, this.pawn.Map, false));
				}
			}
			else if (this.def.HasAreaOfEffect)
			{
				SoundDefOf.PsycastSkipPulse.PlayOneShot(new TargetInfo(target.Cell, this.pawn.Map, false));
			}
			else
			{
				SoundDefOf.PsycastSkipEffect.PlayOneShot(new TargetInfo(target.Cell, this.pawn.Map, false));
			}
			if (target.Thing != this.pawn)
			{
				MoteMaker.MakeConnectingLine(this.pawn.DrawPos, target.CenterVector3, flag ? ThingDefOf.Mote_PsycastPsychicLine : ThingDefOf.Mote_PsycastSkipLine, this.pawn.Map, 1f);
			}
			return base.Activate(target, dest);
		}

		
		protected override void ApplyEffects(IEnumerable<CompAbilityEffect> effects, LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (this.CanApplyPsycastTo(target))
			{
				IEnumerator<CompAbilityEffect> enumerator = effects.GetEnumerator();
				{
					while (enumerator.MoveNext())
					{
						CompAbilityEffect compAbilityEffect = enumerator.Current;
						compAbilityEffect.Apply(target, dest);
					}
					return;
				}
			}
			MoteMaker.ThrowText(target.CenterVector3, this.pawn.Map, "TextMote_Immune".Translate(), -1f);
		}

		
		public bool CanApplyPsycastTo(LocalTargetInfo target)
		{
			if (!base.EffectComps.Any((CompAbilityEffect e) => e.Props.psychic))
			{
				return true;
			}
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) < 1.401298E-45f)
				{
					return false;
				}
				if (pawn.Faction == Faction.OfMechanoids)
				{
					if (base.EffectComps.Any((CompAbilityEffect e) => !e.Props.applicableToMechs))
					{
						return false;
					}
				}
			}
			return true;
		}

		
		public override bool GizmoDisabled(out string reason)
		{
			if (this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) < 1.401298E-45f)
			{
				reason = "CommandPsycastZeroPsychicSensitivity".Translate();
				return true;
			}
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier, false);
			if ((firstHediffOfDef == null || firstHediffOfDef.Severity < (float)this.def.level) && this.def.level > 0)
			{
				reason = "CommandPsycastHigherLevelPsylinkRequired".Translate(this.def.level);
				return true;
			}
			if (this.def.level > this.pawn.psychicEntropy.MaxAbilityLevel)
			{
				reason = "CommandPsycastLowPsyfocus".Translate(Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[this.def.RequiredPsyfocusBand].ToStringPercent());
				return true;
			}
			if (this.def.PsyfocusCost > this.pawn.psychicEntropy.CurrentPsyfocus)
			{
				reason = "CommandPsycastNotEnoughPsyfocus".Translate(this.def.PsyfocusCost.ToStringPercent(), this.pawn.psychicEntropy.CurrentPsyfocus.ToStringPercent(), this.def.label.Named("PSYCASTNAME"), this.pawn.Named("CASTERNAME"));
				return true;
			}
			if (this.def.EntropyGain > 1.401298E-45f && this.pawn.psychicEntropy.WouldOverflowEntropy(this.def.EntropyGain + PsycastUtility.TotalEntropyFromQueuedPsycasts(this.pawn)))
			{
				reason = "CommandPsycastWouldExceedEntropy".Translate(this.def.label);
				return true;
			}
			return base.GizmoDisabled(out reason);
		}

		
		public override void QueueCastingJob(LocalTargetInfo target, LocalTargetInfo destination)
		{
			base.QueueCastingJob(target, destination);
			if (this.moteCast == null || this.moteCast.Destroyed)
			{
				this.moteCast = MoteMaker.MakeAttachedOverlay(this.pawn, ThingDefOf.Mote_CastPsycast, Psycast.MoteCastOffset, Psycast.MoteCastScale, base.verb.verbProps.warmupTime - Psycast.MoteCastFadeTime);
			}
		}

		
		public override void AbilityTick()
		{
			base.AbilityTick();
			if (this.moteCast != null && !this.moteCast.Destroyed && base.verb.WarmingUp)
			{
				this.moteCast.Maintain();
			}
			if (base.verb.WarmingUp)
			{
				if (this.soundCast == null || this.soundCast.Ended)
				{
					this.soundCast = SoundDefOf.PsycastCastLoop.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(this.pawn.Position, this.pawn.Map, false), MaintenanceType.PerTick));
					return;
				}
				this.soundCast.Maintain();
			}
		}

		
		private Mote moteCast;

		
		private Sustainer soundCast;

		
		private static float MoteCastFadeTime = 0.4f;

		
		private static float MoteCastScale = 1f;

		
		private static Vector3 MoteCastOffset = new Vector3(0f, 0f, 0.48f);
	}
}
