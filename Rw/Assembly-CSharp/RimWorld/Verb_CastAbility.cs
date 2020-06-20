using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AE6 RID: 2790
	public class Verb_CastAbility : Verb
	{
		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x060041EA RID: 16874 RVA: 0x00160509 File Offset: 0x0015E709
		public static Color RadiusHighlightColor
		{
			get
			{
				return new Color(0.3f, 0.8f, 1f);
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x060041EB RID: 16875 RVA: 0x0016051F File Offset: 0x0015E71F
		public override string ReportLabel
		{
			get
			{
				return this.ability.def.label;
			}
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x060041EC RID: 16876 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x060041ED RID: 16877 RVA: 0x00160534 File Offset: 0x0015E734
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

		// Token: 0x060041EE RID: 16878 RVA: 0x00160561 File Offset: 0x0015E761
		protected override bool TryCastShot()
		{
			return this.ability.Activate(this.currentTarget, this.currentDestination);
		}

		// Token: 0x060041EF RID: 16879 RVA: 0x0016057C File Offset: 0x0015E77C
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

		// Token: 0x060041F0 RID: 16880 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool IsApplicableTo(LocalTargetInfo target, bool showMessages = false)
		{
			return true;
		}

		// Token: 0x060041F1 RID: 16881 RVA: 0x001605C0 File Offset: 0x0015E7C0
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

		// Token: 0x060041F2 RID: 16882 RVA: 0x0016065E File Offset: 0x0015E85E
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.CanHitTarget(target) && this.IsApplicableTo(target, false))
			{
				base.OnGUI(target);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x060041F3 RID: 16883 RVA: 0x00160685 File Offset: 0x0015E885
		public void DrawRadius()
		{
			GenDraw.DrawRadiusRing(this.ability.pawn.Position, this.verbProps.range);
		}

		// Token: 0x060041F4 RID: 16884 RVA: 0x001606A8 File Offset: 0x0015E8A8
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

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x060041F5 RID: 16885 RVA: 0x00160736 File Offset: 0x0015E936
		public override Texture2D UIIcon
		{
			get
			{
				return this.ability.def.uiIcon;
			}
		}

		// Token: 0x0400261E RID: 9758
		public Ability ability;
	}
}
