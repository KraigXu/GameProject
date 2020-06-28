using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000AC7 RID: 2759
	public abstract class CompAbilityEffect : AbilityComp
	{
		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x0600417D RID: 16765 RVA: 0x0015E3FB File Offset: 0x0015C5FB
		public CompProperties_AbilityEffect Props
		{
			get
			{
				return (CompProperties_AbilityEffect)this.props;
			}
		}

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x0600417E RID: 16766 RVA: 0x0015E408 File Offset: 0x0015C608
		protected bool SendLetter
		{
			get
			{
				return this.Props.sendLetter && !this.Props.customLetterText.NullOrEmpty() && !this.Props.customLetterLabel.NullOrEmpty();
			}
		}

		// Token: 0x0600417F RID: 16767 RVA: 0x0015E440 File Offset: 0x0015C640
		public virtual void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (this.Props.screenShakeIntensity > 1.401298E-45f)
			{
				Find.CameraDriver.shaker.DoShake(this.Props.screenShakeIntensity);
			}
			Pawn pawn = this.parent.pawn;
			Pawn pawn2 = target.Pawn;
			if (pawn2 != null)
			{
				Faction factionOrExtraHomeFaction = pawn2.FactionOrExtraHomeFaction;
				if (this.Props.goodwillImpact != 0 && pawn.Faction != null && factionOrExtraHomeFaction != null && !factionOrExtraHomeFaction.HostileTo(pawn.Faction) && (this.Props.applyGoodwillImpactToLodgers || !pawn2.IsQuestLodger()) && !pawn2.IsQuestHelper())
				{
					factionOrExtraHomeFaction.TryAffectGoodwillWith(pawn.Faction, this.Props.goodwillImpact, true, true, "GoodwillChangedReason_UsedAbility".Translate(this.parent.def.LabelCap, pawn2.LabelShort), new GlobalTargetInfo?(pawn2));
				}
			}
			ThingDef moteDef = (!this.Props.psychic) ? ThingDefOf.Mote_PsycastSkipEffect : ThingDefOf.Mote_PsycastPsychicEffect;
			if (target.HasThing)
			{
				MoteMaker.MakeAttachedOverlay(target.Thing, moteDef, Vector3.zero, 1f, -1f);
			}
			else
			{
				MoteMaker.MakeStaticMote(target.Cell, this.parent.pawn.Map, moteDef, 1f);
			}
			if (this.Props.clamorType != null)
			{
				GenClamor.DoClamor(this.parent.pawn, target.Cell, (float)this.Props.clamorRadius, this.Props.clamorType);
			}
			if (this.Props.sound != null)
			{
				this.Props.sound.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
			}
			if (!this.Props.message.NullOrEmpty())
			{
				Messages.Message(this.Props.message, this.parent.pawn, this.Props.messageType ?? MessageTypeDefOf.SilentInput, true);
			}
		}

		// Token: 0x06004180 RID: 16768 RVA: 0x0015E65C File Offset: 0x0015C85C
		public virtual bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Props.availableWhenTargetIsWounded || (target.Pawn.health.hediffSet.BleedRateTotal <= 0f && !target.Pawn.health.HasHediffsNeedingTend(false));
		}

		// Token: 0x06004181 RID: 16769 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void DrawEffectPreview(LocalTargetInfo target)
		{
		}

		// Token: 0x06004182 RID: 16770 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			return true;
		}
	}
}
