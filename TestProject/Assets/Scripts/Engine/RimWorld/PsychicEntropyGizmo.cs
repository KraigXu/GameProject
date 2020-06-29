using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class PsychicEntropyGizmo : Gizmo
	{
		
		public PsychicEntropyGizmo(Pawn_PsychicEntropyTracker tracker)
		{
			this.tracker = tracker;
			this.order = -100f;
			this.LimitedTex = ContentFinder<Texture2D>.Get("UI/Icons/EntropyLimit/Limited", true);
			this.UnlimitedTex = ContentFinder<Texture2D>.Get("UI/Icons/EntropyLimit/Unlimited", true);
		}

		
		private void DrawThreshold(Rect rect, float percent, float entropyValue)
		{
			Rect position = new Rect
			{
				x = rect.x + 3f + (rect.width - 8f) * percent,
				y = rect.y + rect.height - 9f,
				width = 2f,
				height = 6f
			};
			if (entropyValue < percent)
			{
				GUI.DrawTexture(position, BaseContent.GreyTex);
				return;
			}
			GUI.DrawTexture(position, BaseContent.BlackTex);
		}

		
		private void DrawPsyfocusTarget(Rect rect, float percent)
		{
			float num = Mathf.Round((rect.width - 8f) * percent);
			GUI.DrawTexture(new Rect
			{
				x = rect.x + 3f + num,
				y = rect.y,
				width = 2f,
				height = rect.height
			}, PsychicEntropyGizmo.PsyfocusTargetTex);
			float num2 = Widgets.AdjustCoordToUIScalingFloor(rect.x + 2f + num);
			float xMax = Widgets.AdjustCoordToUIScalingCeil(num2 + 4f);
			Rect rect2 = new Rect
			{
				y = rect.y - 3f,
				height = 5f,
				xMin = num2,
				xMax = xMax
			};
			GUI.DrawTexture(rect2, PsychicEntropyGizmo.PsyfocusTargetTex);
			Rect position = rect2;
			position.y = rect.yMax - 2f;
			GUI.DrawTexture(position, PsychicEntropyGizmo.PsyfocusTargetTex);
		}

		
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			Widgets.DrawWindowBackground(rect);
			Text.Font = GameFont.Small;
			Rect rect3 = rect2;
			rect3.y += 6f;
			rect3.height = Text.LineHeight;
			Widgets.Label(rect3, "PsychicEntropyShort".Translate());
			Rect rect4 = rect2;
			rect4.y += 38f;
			rect4.height = Text.LineHeight;
			Widgets.Label(rect4, "Psyfocus".Translate());
			Rect rect5 = rect2;
			rect5.x += 63f;
			rect5.y += 6f;
			rect5.width = 100f;
			rect5.height = 22f;
			float entropyRelativeValue = this.tracker.EntropyRelativeValue;
			Widgets.FillableBar(rect5, Mathf.Min(entropyRelativeValue, 1f), PsychicEntropyGizmo.EntropyBarTex, PsychicEntropyGizmo.EmptyBarTex, true);
			if (this.tracker.EntropyValue > this.tracker.MaxEntropy)
			{
				Widgets.FillableBar(rect5, Mathf.Min(entropyRelativeValue - 1f, 1f), PsychicEntropyGizmo.OverLimitBarTex, PsychicEntropyGizmo.EntropyBarTex, true);
				foreach (KeyValuePair<PsychicEntropySeverity, float> keyValuePair in Pawn_PsychicEntropyTracker.EntropyThresholds)
				{
					if (keyValuePair.Value > 1f && keyValuePair.Value < 2f)
					{
						this.DrawThreshold(rect5, keyValuePair.Value - 1f, entropyRelativeValue);
					}
				}
			}
			string label = this.tracker.EntropyValue.ToString("F0") + " / " + this.tracker.MaxEntropy.ToString("F0");
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect5, label);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Tiny;
			GUI.color = Color.white;
			Rect rect6 = rect2;
			rect6.width = 175f;
			rect6.height = 38f;
			TooltipHandler.TipRegion(rect6, delegate
			{
				float f = this.tracker.EntropyValue / this.tracker.RecoveryRate;
				return string.Format("PawnTooltipPsychicEntropyStats".Translate(), new object[]
				{
					Mathf.Round(this.tracker.EntropyValue),
					Mathf.Round(this.tracker.MaxEntropy),
					this.tracker.RecoveryRate.ToString("0.#"),
					Mathf.Round(f)
				}) + "\n\n" + "PawnTooltipPsychicEntropyDesc".Translate();
			}, Gen.HashCombineInt(this.tracker.GetHashCode(), 133858));
			Rect rect7 = rect2;
			rect7.x += 63f;
			rect7.y += 38f;
			rect7.width = 100f;
			rect7.height = 22f;
			bool flag = Mouse.IsOver(rect7);
			Widgets.FillableBar(rect7, Mathf.Min(this.tracker.CurrentPsyfocus, 1f), flag ? PsychicEntropyGizmo.PsyfocusBarHighlightTex : PsychicEntropyGizmo.PsyfocusBarTex, PsychicEntropyGizmo.EmptyBarTex, true);
			for (int i = 1; i < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages.Count - 1; i++)
			{
				this.DrawThreshold(rect7, Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[i], this.tracker.CurrentPsyfocus);
			}
			this.DrawPsyfocusTarget(rect7, this.tracker.TargetPsyfocus);
			float targetPsyfocus = this.tracker.TargetPsyfocus;
			Vector2 mousePosition = Event.current.mousePosition;
			if (flag && Input.GetMouseButton(0))
			{
				this.tracker.SetPsyfocusTarget(Mathf.Round((mousePosition.x - (rect7.x + 3f)) / (rect7.width - 8f) * 16f) / 16f);
				if (Math.Abs(targetPsyfocus - this.tracker.TargetPsyfocus) > 1.401298E-45f)
				{
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.MeditationDesiredPsyfocus, KnowledgeAmount.Total);
			}
			UIHighlighter.HighlightOpportunity(rect7, "PsyfocusBar");
			GUI.color = Color.white;
			Rect rect8 = rect2;
			rect8.y += 38f;
			rect8.width = 175f;
			rect8.height = 38f;
			TooltipHandler.TipRegion(rect8, () => this.tracker.PsyfocusTipString(), Gen.HashCombineInt(this.tracker.GetHashCode(), 133873));
			if (this.tracker.Pawn.IsColonistPlayerControlled)
			{
				float num = 32f;
				float num2 = 4f;
				float num3 = rect2.height / 2f - num + num2;
				float num4 = rect2.width - num;
				Rect rect9 = new Rect(rect2.x + num4, rect2.y + num3, num, num);
				if (Widgets.ButtonImage(rect9, this.tracker.limitEntropyAmount ? this.LimitedTex : this.UnlimitedTex, true))
				{
					this.tracker.limitEntropyAmount = !this.tracker.limitEntropyAmount;
					if (this.tracker.limitEntropyAmount)
					{
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					}
					else
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
				}
				TooltipHandler.TipRegionByKey(rect9, "PawnTooltipPsychicEntropyLimit");
			}
			if (this.tracker.PainMultiplier > 1f)
			{
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleCenter;
				string recoveryBonus = (this.tracker.PainMultiplier - 1f).ToStringPercent("F0");
				string recoveryBonus2 = recoveryBonus;
				float widthCached = recoveryBonus2.GetWidthCached();
				Rect rect10 = rect2;
				rect10.x += rect2.width - widthCached / 2f - 16f;
				rect10.y += 38f;
				rect10.width = widthCached;
				rect10.height = Text.LineHeight;
				GUI.color = PsychicEntropyGizmo.PainBoostColor;
				Widgets.Label(rect10, recoveryBonus2);
				GUI.color = Color.white;
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.UpperLeft;
				TooltipHandler.TipRegion(rect10.ContractedBy(-1f), () => "PawnTooltipPsychicEntropyPainFocus".Translate(this.tracker.Pawn.health.hediffSet.PainTotal.ToStringPercent("F0"), recoveryBonus), Gen.HashCombineInt(this.tracker.GetHashCode(), 133878));
			}
			return new GizmoResult(GizmoState.Clear);
		}

		
		public override float GetWidth(float maxWidth)
		{
			return 212f;
		}

		
		private Pawn_PsychicEntropyTracker tracker;

		
		private Texture2D LimitedTex;

		
		private Texture2D UnlimitedTex;

		
		private const string LimitedIconPath = "UI/Icons/EntropyLimit/Limited";

		
		private const string UnlimitedIconPath = "UI/Icons/EntropyLimit/Unlimited";

		
		private static readonly Color PainBoostColor = new Color(0.2f, 0.65f, 0.35f);

		
		private static readonly Texture2D EntropyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.46f, 0.34f, 0.35f));

		
		private static readonly Texture2D OverLimitBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.75f, 0.2f, 0.15f));

		
		private static readonly Texture2D PsyfocusBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.34f, 0.42f, 0.43f));

		
		private static readonly Texture2D PsyfocusBarHighlightTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.43f, 0.54f, 0.55f));

		
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.03f, 0.035f, 0.05f));

		
		private static readonly Texture2D PsyfocusTargetTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.74f, 0.97f, 0.8f));
	}
}
