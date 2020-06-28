using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AE7 RID: 2791
	public class Verb_CastPsycast : Verb_CastAbility
	{
		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x060041F7 RID: 16887 RVA: 0x00160748 File Offset: 0x0015E948
		public Psycast Psycast
		{
			get
			{
				return this.ability as Psycast;
			}
		}

		// Token: 0x060041F8 RID: 16888 RVA: 0x00160758 File Offset: 0x0015E958
		public override bool IsApplicableTo(LocalTargetInfo target, bool showMessages = false)
		{
			if (!base.IsApplicableTo(target, showMessages))
			{
				return false;
			}
			if (!this.Psycast.def.HasAreaOfEffect && !this.Psycast.CanApplyPsycastTo(target))
			{
				if (showMessages)
				{
					Messages.Message(this.ability.def.LabelCap + ": " + "AbilityTargetPsychicallyDeaf".Translate(), MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			return true;
		}

		// Token: 0x060041F9 RID: 16889 RVA: 0x001607D0 File Offset: 0x0015E9D0
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			if (!this.IsApplicableTo(target, false))
			{
				return;
			}
			base.OrderForceTarget(target);
		}

		// Token: 0x060041FA RID: 16890 RVA: 0x001607E4 File Offset: 0x0015E9E4
		public override bool ValidateTarget(LocalTargetInfo target)
		{
			if (!base.ValidateTarget(target))
			{
				return false;
			}
			if (this.CasterPawn.GetStatValue(StatDefOf.PsychicSensitivity, true) < 1.401298E-45f)
			{
				Messages.Message("CommandPsycastZeroPsychicSensitivity".Translate(), this.caster, MessageTypeDefOf.RejectInput, true);
				return false;
			}
			if (this.Psycast.def.EntropyGain > 1.401298E-45f && this.CasterPawn.psychicEntropy.WouldOverflowEntropy(this.Psycast.def.EntropyGain + PsycastUtility.TotalEntropyFromQueuedPsycasts(this.CasterPawn)))
			{
				Messages.Message("CommandPsycastWouldExceedEntropy".Translate(), this.caster, MessageTypeDefOf.RejectInput, true);
				return false;
			}
			return true;
		}

		// Token: 0x060041FB RID: 16891 RVA: 0x001608A8 File Offset: 0x0015EAA8
		public override void OnGUI(LocalTargetInfo target)
		{
			bool flag = this.ability.EffectComps.Any((CompAbilityEffect e) => e.Props.psychic);
			Texture2D texture2D = this.UIIcon;
			if (!this.Psycast.CanApplyPsycastTo(target))
			{
				texture2D = TexCommand.CannotShoot;
				this.DrawIneffectiveWarning(target);
			}
			if (target.IsValid && this.CanHitTarget(target) && this.IsApplicableTo(target, false))
			{
				if (flag)
				{
					foreach (LocalTargetInfo target2 in this.ability.GetAffectedTargets(target))
					{
						if (this.Psycast.CanApplyPsycastTo(target2))
						{
							this.DrawSensitivityStat(target2);
						}
						else
						{
							this.DrawIneffectiveWarning(target2);
						}
					}
				}
				if (this.ability.EffectComps.Any((CompAbilityEffect e) => !e.Valid(target, false)))
				{
					texture2D = TexCommand.CannotShoot;
				}
			}
			else
			{
				texture2D = TexCommand.CannotShoot;
			}
			if (ThingRequiringRoyalPermissionUtility.IsViolatingRulesOfAnyFaction_NewTemp(HediffDefOf.PsychicAmplifier, this.CasterPawn, this.Psycast.def.level, true) && this.Psycast.def.DetectionChance > 0f)
			{
				TaggedString taggedString = "Illegal".Translate().ToUpper() + "\n" + this.Psycast.def.DetectionChance.ToStringPercent() + " " + "DetectionChance".Translate();
				Text.Font = GameFont.Small;
				GenUI.DrawMouseAttachment(texture2D, taggedString, 0f, default(Vector2), null, true, new Color(0.25f, 0f, 0f));
				return;
			}
			GenUI.DrawMouseAttachment(texture2D);
		}

		// Token: 0x060041FC RID: 16892 RVA: 0x00160AB4 File Offset: 0x0015ECB4
		private void DrawIneffectiveWarning(LocalTargetInfo target)
		{
			if (target.Pawn != null)
			{
				Vector3 drawPos = target.Pawn.DrawPos;
				drawPos.z += 1f;
				GenMapUI.DrawText(new Vector2(drawPos.x, drawPos.z), "Ineffective".Translate(), Color.red);
			}
		}

		// Token: 0x060041FD RID: 16893 RVA: 0x00160B14 File Offset: 0x0015ED14
		private void DrawSensitivityStat(LocalTargetInfo target)
		{
			if (target.Pawn != null)
			{
				Pawn pawn = target.Pawn;
				float statValue = pawn.GetStatValue(StatDefOf.PsychicSensitivity, true);
				Vector3 drawPos = pawn.DrawPos;
				drawPos.z += 1f;
				GenMapUI.DrawText(new Vector2(drawPos.x, drawPos.z), StatDefOf.PsychicSensitivity.LabelCap + ": " + statValue, (statValue > float.Epsilon) ? Color.white : Color.red);
			}
		}

		// Token: 0x0400261F RID: 9759
		private const float StatLabelOffsetY = 1f;
	}
}
