using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D43 RID: 3395
	[StaticConstructorOnStartup]
	public class Gizmo_RefuelableFuelStatus : Gizmo
	{
		// Token: 0x0600529A RID: 21146 RVA: 0x001A8E38 File Offset: 0x001A7038
		public Gizmo_RefuelableFuelStatus()
		{
			this.order = -100f;
		}

		// Token: 0x0600529B RID: 21147 RVA: 0x001A8E4B File Offset: 0x001A704B
		public override float GetWidth(float maxWidth)
		{
			return 140f;
		}

		// Token: 0x0600529C RID: 21148 RVA: 0x001B993C File Offset: 0x001B7B3C
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			Rect overRect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Find.WindowStack.ImmediateWindow(1523289473, overRect, WindowLayer.GameUI, delegate
			{
				Rect rect2;
				Rect rect = rect2 = overRect.AtZero().ContractedBy(6f);
				rect2.height = overRect.height / 2f;
				Text.Font = GameFont.Tiny;
				Widgets.Label(rect2, this.refuelable.Props.FuelGizmoLabel);
				Rect rect3 = rect;
				rect3.yMin = overRect.height / 2f;
				float fillPercent = this.refuelable.Fuel / this.refuelable.Props.fuelCapacity;
				Widgets.FillableBar(rect3, fillPercent, Gizmo_RefuelableFuelStatus.FullBarTex, Gizmo_RefuelableFuelStatus.EmptyBarTex, false);
				if (this.refuelable.Props.targetFuelLevelConfigurable)
				{
					float num = this.refuelable.TargetFuelLevel / this.refuelable.Props.fuelCapacity;
					float x = rect3.x + num * rect3.width - (float)Gizmo_RefuelableFuelStatus.TargetLevelArrow.width * 0.5f / 2f;
					float y = rect3.y - (float)Gizmo_RefuelableFuelStatus.TargetLevelArrow.height * 0.5f;
					GUI.DrawTexture(new Rect(x, y, (float)Gizmo_RefuelableFuelStatus.TargetLevelArrow.width * 0.5f, (float)Gizmo_RefuelableFuelStatus.TargetLevelArrow.height * 0.5f), Gizmo_RefuelableFuelStatus.TargetLevelArrow);
				}
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect3, this.refuelable.Fuel.ToString("F0") + " / " + this.refuelable.Props.fuelCapacity.ToString("F0"));
				Text.Anchor = TextAnchor.UpperLeft;
			}, true, false, 1f);
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x04002DA3 RID: 11683
		public CompRefuelable refuelable;

		// Token: 0x04002DA4 RID: 11684
		private static readonly Texture2D FullBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.35f, 0.35f, 0.2f));

		// Token: 0x04002DA5 RID: 11685
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(Color.black);

		// Token: 0x04002DA6 RID: 11686
		private static readonly Texture2D TargetLevelArrow = ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarkerRotated", true);

		// Token: 0x04002DA7 RID: 11687
		private const float ArrowScale = 0.5f;
	}
}
