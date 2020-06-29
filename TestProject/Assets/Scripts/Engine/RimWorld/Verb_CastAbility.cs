using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Verb_CastAbility : Verb
	{
		
		
		public static Color RadiusHighlightColor
		{
			get
			{
				return new Color(0.3f, 0.8f, 1f);
			}
		}

		
		
		public override string ReportLabel
		{
			get
			{
				return this.ability.def.label;
			}
		}

		
		
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		
		
		public override ITargetingSource DestinationSelector
		{
			get
			{
				CompAbilityEffect_WithDest compAbilityEffect_WithDest = this.ability.CompOfType<CompAbilityEffect_WithDest>();
				if (compAbilityEffect_WithDest != null && compAbilityEffect_WithDest.Props.destination == AbilityEffectDestination.Selected)
				{
					return compAbilityEffect_WithDest;
				}
				return null;
			}
		}

		
		protected override bool TryCastShot()
		{
			return this.ability.Activate(this.currentTarget, this.currentDestination);
		}

		
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			CompAbilityEffect_WithDest compAbilityEffect_WithDest = this.ability.CompOfType<CompAbilityEffect_WithDest>();
			if (compAbilityEffect_WithDest != null && compAbilityEffect_WithDest.Props.destination == AbilityEffectDestination.Selected)
			{
				compAbilityEffect_WithDest.SetTarget(target);
				return;
			}
			this.ability.QueueCastingJob(target, null);
		}

		
		public virtual bool IsApplicableTo(LocalTargetInfo target, bool showMessages = false)
		{
			return true;
		}

		
		public override bool ValidateTarget(LocalTargetInfo target)
		{
			if (!this.CanHitTarget(target))
			{
				if (target.IsValid)
				{
					Messages.Message(this.ability.def.LabelCap + ": " + "AbilityCannotHitTarget".Translate(), MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			if (!this.IsApplicableTo(target, true))
			{
				return false;
			}
			for (int i = 0; i < this.ability.EffectComps.Count; i++)
			{
				if (!this.ability.EffectComps[i].Valid(target, true))
				{
					return false;
				}
			}
			return true;
		}

		
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.CanHitTarget(target) && this.IsApplicableTo(target, false))
			{
				base.OnGUI(target);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		
		public void DrawRadius()
		{
			GenDraw.DrawRadiusRing(this.ability.pawn.Position, this.verbProps.range);
		}

		
		public override void DrawHighlight(LocalTargetInfo target)
		{
			AbilityDef def = this.ability.def;
			this.DrawRadius();
			if (this.CanHitTarget(target) && this.IsApplicableTo(target, false))
			{
				if (def.HasAreaOfEffect)
				{
					if (target.IsValid)
					{
						GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
						GenDraw.DrawRadiusRing(target.Cell, def.EffectRadius, Verb_CastAbility.RadiusHighlightColor, null);
					}
				}
				else
				{
					GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
				}
			}
			if (target.IsValid)
			{
				this.ability.DrawEffectPreviews(target);
			}
		}

		
		
		public override Texture2D UIIcon
		{
			get
			{
				return this.ability.def.uiIcon;
			}
		}

		
		public Ability ability;
	}
}
