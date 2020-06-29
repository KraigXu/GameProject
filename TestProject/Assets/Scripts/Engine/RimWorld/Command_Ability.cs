using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class Command_Ability : Command
	{
		
		// (get) Token: 0x06004170 RID: 16752 RVA: 0x0015DE32 File Offset: 0x0015C032
		public override Texture2D BGTexture
		{
			get
			{
				return Command_Ability.BGTex;
			}
		}

		
		// (get) Token: 0x06004171 RID: 16753 RVA: 0x0015DE39 File Offset: 0x0015C039
		public virtual string Tooltip
		{
			get
			{
				return this.ability.def.GetTooltip(this.ability.pawn);
			}
		}

		
		public Command_Ability(Ability ability)
		{
			this.ability = ability;
			this.order = 5f;
			this.defaultLabel = ability.def.LabelCap;
			this.hotKey = ability.def.hotKey;
			this.icon = ability.def.uiIcon;
		}

		
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

		
		public override void GizmoUpdateOnMouseover()
		{
			Verb_CastAbility verb_CastAbility;
			if ((verb_CastAbility = (this.ability.verb as Verb_CastAbility)) != null && this.ability.def.targetRequired)
			{
				verb_CastAbility.DrawRadius();
			}
		}

		
		protected void DisableWithReason(string reason)
		{
			this.disabledReason = reason;
			this.disabled = true;
		}

		
		protected Ability ability;

		
		public new static readonly Texture2D BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/AbilityButBG", true);

		
		private static readonly Texture2D cooldownBarTex = SolidColorMaterials.NewSolidColorTexture(new Color32(9, 203, 4, 64));
	}
}
