using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EEC RID: 3820
	public class PawnColumnWorker_Outfit : PawnColumnWorker
	{
		// Token: 0x06005D9D RID: 23965 RVA: 0x0020563C File Offset: 0x0020383C
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "ManageOutfits".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageOutfits(null));
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Outfits, KnowledgeAmount.Total);
			}
			UIHighlighter.HighlightOpportunity(rect2, "ManageOutfits");
		}

		// Token: 0x06005D9E RID: 23966 RVA: 0x002056D0 File Offset: 0x002038D0
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.outfits == null)
			{
				return;
			}
			int num = Mathf.FloorToInt((rect.width - 4f) * 0.714285731f);
			int num2 = Mathf.FloorToInt((rect.width - 4f) * 0.2857143f);
			float num3 = rect.x;
			bool somethingIsForced = pawn.outfits.forcedHandler.SomethingIsForced;
			Rect rect2 = new Rect(num3, rect.y + 2f, (float)num, rect.height - 4f);
			if (somethingIsForced)
			{
				rect2.width -= 4f + (float)num2;
			}
			if (pawn.IsQuestLodger())
			{
				Rect rect3 = new Rect(rect2.x + 10f, rect2.y, rect2.width - 5f, rect2.height);
				Widgets.Label(rect3, "Unchangeable".Translate());
				TooltipHandler.TipRegionByKey(rect3, "QuestRelated_Outfit");
				num3 -= 10f;
			}
			else
			{
				Widgets.Dropdown<Pawn, Outfit>(rect2, pawn, (Pawn p) => p.outfits.CurrentOutfit, new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<Outfit>>>(this.Button_GenerateMenu), pawn.outfits.CurrentOutfit.label.Truncate(rect2.width, null), null, pawn.outfits.CurrentOutfit.label, null, null, true);
			}
			num3 += rect2.width;
			num3 += 4f;
			Rect rect4 = new Rect(num3, rect.y + 2f, (float)num2, rect.height - 4f);
			if (somethingIsForced)
			{
				if (Widgets.ButtonText(rect4, "ClearForcedApparel".Translate(), true, true, true))
				{
					pawn.outfits.forcedHandler.Reset();
				}
				if (Mouse.IsOver(rect4))
				{
					TooltipHandler.TipRegion(rect4, new TipSignal(delegate
					{
						string text = "ForcedApparel".Translate() + ":\n";
						foreach (Apparel apparel in pawn.outfits.forcedHandler.ForcedApparel)
						{
							text = text + "\n   " + apparel.LabelCap;
						}
						return text;
					}, pawn.GetHashCode() * 612));
				}
				num3 += (float)num2;
				num3 += 4f;
			}
			Rect rect5 = new Rect(num3, rect.y + 2f, (float)num2, rect.height - 4f);
			if (!pawn.HasExtraHomeFaction(null) && Widgets.ButtonText(rect5, "AssignTabEdit".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageOutfits(pawn.outfits.CurrentOutfit));
			}
			num3 += (float)num2;
		}

		// Token: 0x06005D9F RID: 23967 RVA: 0x0020596D File Offset: 0x00203B6D
		private IEnumerable<Widgets.DropdownMenuElement<Outfit>> Button_GenerateMenu(Pawn pawn)
		{
			using (List<Outfit>.Enumerator enumerator = Current.Game.outfitDatabase.AllOutfits.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Outfit outfit = enumerator.Current;
					yield return new Widgets.DropdownMenuElement<Outfit>
					{
						option = new FloatMenuOption(outfit.label, delegate
						{
							pawn.outfits.CurrentOutfit = outfit;
						}, MenuOptionPriority.Default, null, null, 0f, null, null),
						payload = outfit
					};
				}
			}
			List<Outfit>.Enumerator enumerator = default(List<Outfit>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06005DA0 RID: 23968 RVA: 0x00204DC3 File Offset: 0x00202FC3
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x06005DA1 RID: 23969 RVA: 0x00204DDB File Offset: 0x00202FDB
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(251f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06005DA2 RID: 23970 RVA: 0x00204AFD File Offset: 0x00202CFD
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x06005DA3 RID: 23971 RVA: 0x00205980 File Offset: 0x00203B80
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06005DA4 RID: 23972 RVA: 0x002059A3 File Offset: 0x00203BA3
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.outfits != null && pawn.outfits.CurrentOutfit != null)
			{
				return pawn.outfits.CurrentOutfit.uniqueId;
			}
			return int.MinValue;
		}

		// Token: 0x040032D0 RID: 13008
		public const int TopAreaHeight = 65;

		// Token: 0x040032D1 RID: 13009
		public const int ManageOutfitsButtonHeight = 32;
	}
}
