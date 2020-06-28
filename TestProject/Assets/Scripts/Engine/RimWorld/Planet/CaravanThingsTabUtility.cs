using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02001290 RID: 4752
	[StaticConstructorOnStartup]
	public static class CaravanThingsTabUtility
	{
		// Token: 0x06006FC8 RID: 28616 RVA: 0x0026F160 File Offset: 0x0026D360
		public static void DoAbandonButton(Rect rowRect, Thing t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonButtonTex, true))
			{
				CaravanAbandonOrBanishUtility.TryAbandonOrBanishViaInterface(t, caravan);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, false), Gen.HashCombineInt(t.GetHashCode(), 1383004931));
			}
		}

		// Token: 0x06006FC9 RID: 28617 RVA: 0x0026F1F4 File Offset: 0x0026D3F4
		public static void DoAbandonButton(Rect rowRect, TransferableImmutable t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonButtonTex, true))
			{
				CaravanAbandonOrBanishUtility.TryAbandonOrBanishViaInterface(t, caravan);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, false), Gen.HashCombineInt(t.GetHashCode(), 8476546));
			}
		}

		// Token: 0x06006FCA RID: 28618 RVA: 0x0026F288 File Offset: 0x0026D488
		public static void DoAbandonSpecificCountButton(Rect rowRect, Thing t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonSpecificCountButtonTex, true))
			{
				CaravanAbandonOrBanishUtility.TryAbandonSpecificCountViaInterface(t, caravan);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, true), Gen.HashCombineInt(t.GetHashCode(), 1163428609));
			}
		}

		// Token: 0x06006FCB RID: 28619 RVA: 0x0026F31C File Offset: 0x0026D51C
		public static void DoAbandonSpecificCountButton(Rect rowRect, TransferableImmutable t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonSpecificCountButtonTex, true))
			{
				CaravanAbandonOrBanishUtility.TryAbandonSpecificCountViaInterface(t, caravan);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, true), Gen.HashCombineInt(t.GetHashCode(), 1163428609));
			}
		}

		// Token: 0x06006FCC RID: 28620 RVA: 0x0026F3B0 File Offset: 0x0026D5B0
		public static void DoOpenSpecificTabButton(Rect rowRect, Pawn p, ref Pawn specificTabForPawn)
		{
			Color baseColor = (p == specificTabForPawn) ? CaravanThingsTabUtility.OpenedSpecificTabButtonColor : Color.white;
			Color mouseoverColor = (p == specificTabForPawn) ? CaravanThingsTabUtility.OpenedSpecificTabButtonMouseoverColor : GenUI.MouseoverColor;
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.SpecificTabButtonTex, baseColor, mouseoverColor, true))
			{
				if (p == specificTabForPawn)
				{
					specificTabForPawn = null;
					SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				}
				else
				{
					specificTabForPawn = p;
					SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				}
			}
			TooltipHandler.TipRegionByKey(rect, "OpenSpecificTabButtonTip");
			GUI.color = Color.white;
		}

		// Token: 0x06006FCD RID: 28621 RVA: 0x0026F458 File Offset: 0x0026D658
		public static void DrawMass(TransferableImmutable transferable, Rect rect)
		{
			float num = 0f;
			for (int i = 0; i < transferable.things.Count; i++)
			{
				num += transferable.things[i].GetStatValue(StatDefOf.Mass, true) * (float)transferable.things[i].stackCount;
			}
			CaravanThingsTabUtility.DrawMass(num, rect);
		}

		// Token: 0x06006FCE RID: 28622 RVA: 0x0026F4B5 File Offset: 0x0026D6B5
		public static void DrawMass(Thing thing, Rect rect)
		{
			CaravanThingsTabUtility.DrawMass(thing.GetStatValue(StatDefOf.Mass, true) * (float)thing.stackCount, rect);
		}

		// Token: 0x06006FCF RID: 28623 RVA: 0x0026F4D1 File Offset: 0x0026D6D1
		private static void DrawMass(float mass, Rect rect)
		{
			GUI.color = TransferableOneWayWidget.ItemMassColor;
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect, mass.ToStringMass());
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x040044BF RID: 17599
		public const float MassColumnWidth = 60f;

		// Token: 0x040044C0 RID: 17600
		public const float SpaceAroundIcon = 4f;

		// Token: 0x040044C1 RID: 17601
		public const float SpecificTabButtonSize = 24f;

		// Token: 0x040044C2 RID: 17602
		public const float AbandonButtonSize = 24f;

		// Token: 0x040044C3 RID: 17603
		public const float AbandonSpecificCountButtonSize = 24f;

		// Token: 0x040044C4 RID: 17604
		public static readonly Texture2D AbandonButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/Abandon", true);

		// Token: 0x040044C5 RID: 17605
		public static readonly Texture2D AbandonSpecificCountButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/AbandonSpecificCount", true);

		// Token: 0x040044C6 RID: 17606
		public static readonly Texture2D SpecificTabButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/OpenSpecificTab", true);

		// Token: 0x040044C7 RID: 17607
		public static readonly Color OpenedSpecificTabButtonColor = new Color(0f, 0.8f, 0f);

		// Token: 0x040044C8 RID: 17608
		public static readonly Color OpenedSpecificTabButtonMouseoverColor = new Color(0f, 0.5f, 0f);
	}
}
