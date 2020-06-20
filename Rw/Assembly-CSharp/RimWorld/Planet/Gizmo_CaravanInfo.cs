using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001244 RID: 4676
	public class Gizmo_CaravanInfo : Gizmo
	{
		// Token: 0x06006CFC RID: 27900 RVA: 0x002624A8 File Offset: 0x002606A8
		public Gizmo_CaravanInfo(Caravan caravan)
		{
			this.caravan = caravan;
			this.order = -100f;
		}

		// Token: 0x06006CFD RID: 27901 RVA: 0x002624C2 File Offset: 0x002606C2
		public override float GetWidth(float maxWidth)
		{
			return Mathf.Min(520f, maxWidth);
		}

		// Token: 0x06006CFE RID: 27902 RVA: 0x002624D0 File Offset: 0x002606D0
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			if (!this.caravan.Spawned)
			{
				return new GizmoResult(GizmoState.Clear);
			}
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Widgets.DrawWindowBackground(rect);
			GUI.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			int? ticksToArrive = this.caravan.pather.Moving ? new int?(CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.caravan, true)) : null;
			StringBuilder stringBuilder = new StringBuilder();
			float tilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.caravan, stringBuilder);
			CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.caravan.MassUsage, this.caravan.MassCapacity, this.caravan.MassCapacityExplanation, tilesPerDay, stringBuilder.ToString(), this.caravan.DaysWorthOfFood, this.caravan.forage.ForagedFoodPerDay, this.caravan.forage.ForagedFoodPerDayExplanation, this.caravan.Visibility, this.caravan.VisibilityExplanation, -1f, -1f, null), null, this.caravan.Tile, ticksToArrive, -9999f, rect2, true, null, true);
			GUI.EndGroup();
			GenUI.AbsorbClicksInRect(rect);
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x040043BF RID: 17343
		private Caravan caravan;
	}
}
