using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000AC3 RID: 2755
	[StaticConstructorOnStartup]
	public class Command_Ability : Command
	{
		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06004170 RID: 16752 RVA: 0x0015DE32 File Offset: 0x0015C032
		public override Texture2D BGTexture
		{
			get
			{
				return Command_Ability.BGTex;
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06004171 RID: 16753 RVA: 0x0015DE39 File Offset: 0x0015C039
		public virtual string Tooltip
		{
			get
			{
				return this.ability.def.GetTooltip(this.ability.pawn);
			}
		}

		// Token: 0x06004172 RID: 16754 RVA: 0x0015DE58 File Offset: 0x0015C058
		public Command_Ability(Ability ability)
		{
			this.ability = ability;
			this.order = 5f;
			this.defaultLabel = ability.def.LabelCap;
			this.hotKey = ability.def.hotKey;
			this.icon = ability.def.uiIcon;
		}

		// Token: 0x06004173 RID: 16755 RVA: 0x0015DEB8 File Offset: 0x0015C0B8
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			this.defaultDesc = this.Tooltip;
			string str;
			this.disabled = this.ability.GizmoDisabled(out str);
			if (this.disabled)
			{
				this.DisableWithReason(str.CapitalizeFirst());
			}
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth);
			if (this.ability.CooldownTicksRemaining > 0)
			{
				float num = Mathf.InverseLerp((float)this.ability.CooldownTicksTotal, 0f, (float)this.ability.CooldownTicksRemaining);
				Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
				Widgets.FillableBar(rect, Mathf.Clamp01(num), Command_Ability.cooldownBarTex, null, false);
				if (this.ability.CooldownTicksRemaining > 0)
				{
					Text.Font = GameFont.Tiny;
					Text.Anchor = TextAnchor.UpperCenter;
					Widgets.Label(rect, num.ToStringPercent("F0"));
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
			if (result.State == GizmoState.Interacted && this.ability.CanCast)
			{
				return result;
			}
			return new GizmoResult(result.State);
		}

		// Token: 0x06004174 RID: 16756 RVA: 0x0015DFC0 File Offset: 0x0015C1C0
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			if (this.ability.def.targetRequired)
			{
				Find.Targeter.BeginTargeting(this.ability.verb, null);
				return;
			}
			this.ability.verb.TryStartCastOn(this.ability.pawn, false, true);
		}

		// Token: 0x06004175 RID: 16757 RVA: 0x0015E02C File Offset: 0x0015C22C
		public override void GizmoUpdateOnMouseover()
		{
			Verb_CastAbility verb_CastAbility;
			if ((verb_CastAbility = (this.ability.verb as Verb_CastAbility)) != null && this.ability.def.targetRequired)
			{
				verb_CastAbility.DrawRadius();
			}
		}

		// Token: 0x06004176 RID: 16758 RVA: 0x0015E065 File Offset: 0x0015C265
		protected void DisableWithReason(string reason)
		{
			this.disabledReason = reason;
			this.disabled = true;
		}

		// Token: 0x040025EB RID: 9707
		protected Ability ability;

		// Token: 0x040025EC RID: 9708
		public new static readonly Texture2D BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/AbilityButBG", true);

		// Token: 0x040025ED RID: 9709
		private static readonly Texture2D cooldownBarTex = SolidColorMaterials.NewSolidColorTexture(new Color32(9, 203, 4, 64));
	}
}
