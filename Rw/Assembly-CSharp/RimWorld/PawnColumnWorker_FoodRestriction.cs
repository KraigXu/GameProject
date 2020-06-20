using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EE7 RID: 3815
	public class PawnColumnWorker_FoodRestriction : PawnColumnWorker
	{
		// Token: 0x06005D77 RID: 23927 RVA: 0x00205014 File Offset: 0x00203214
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
			if (Widgets.ButtonText(new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f), "ManageFoodRestrictions".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageFoodRestrictions(null));
			}
		}

		// Token: 0x06005D78 RID: 23928 RVA: 0x0020508F File Offset: 0x0020328F
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.foodRestriction == null)
			{
				return;
			}
			this.DoAssignFoodRestrictionButtons(rect, pawn);
		}

		// Token: 0x06005D79 RID: 23929 RVA: 0x002050A2 File Offset: 0x002032A2
		private IEnumerable<Widgets.DropdownMenuElement<FoodRestriction>> Button_GenerateMenu(Pawn pawn)
		{
			using (List<FoodRestriction>.Enumerator enumerator = Current.Game.foodRestrictionDatabase.AllFoodRestrictions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FoodRestriction foodRestriction = enumerator.Current;
					yield return new Widgets.DropdownMenuElement<FoodRestriction>
					{
						option = new FloatMenuOption(foodRestriction.label, delegate
						{
							pawn.foodRestriction.CurrentFoodRestriction = foodRestriction;
						}, MenuOptionPriority.Default, null, null, 0f, null, null),
						payload = foodRestriction
					};
				}
			}
			List<FoodRestriction>.Enumerator enumerator = default(List<FoodRestriction>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06005D7A RID: 23930 RVA: 0x00204DC3 File Offset: 0x00202FC3
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x06005D7B RID: 23931 RVA: 0x00204DDB File Offset: 0x00202FDB
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(251f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06005D7C RID: 23932 RVA: 0x00204AFD File Offset: 0x00202CFD
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x06005D7D RID: 23933 RVA: 0x002050B4 File Offset: 0x002032B4
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06005D7E RID: 23934 RVA: 0x002050D7 File Offset: 0x002032D7
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.foodRestriction != null && pawn.foodRestriction.CurrentFoodRestriction != null)
			{
				return pawn.foodRestriction.CurrentFoodRestriction.id;
			}
			return int.MinValue;
		}

		// Token: 0x06005D7F RID: 23935 RVA: 0x00205104 File Offset: 0x00203304
		private void DoAssignFoodRestrictionButtons(Rect rect, Pawn pawn)
		{
			int num = Mathf.FloorToInt((rect.width - 4f) * 0.714285731f);
			int num2 = Mathf.FloorToInt((rect.width - 4f) * 0.2857143f);
			float num3 = rect.x;
			Rect rect2 = new Rect(num3, rect.y + 2f, (float)num, rect.height - 4f);
			Widgets.Dropdown<Pawn, FoodRestriction>(rect2, pawn, (Pawn p) => p.foodRestriction.CurrentFoodRestriction, new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<FoodRestriction>>>(this.Button_GenerateMenu), pawn.foodRestriction.CurrentFoodRestriction.label.Truncate(rect2.width, null), null, pawn.foodRestriction.CurrentFoodRestriction.label, null, null, true);
			num3 += (float)num;
			num3 += 4f;
			if (Widgets.ButtonText(new Rect(num3, rect.y + 2f, (float)num2, rect.height - 4f), "AssignTabEdit".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageFoodRestrictions(pawn.foodRestriction.CurrentFoodRestriction));
			}
			num3 += (float)num2;
		}

		// Token: 0x040032C9 RID: 13001
		private const int TopAreaHeight = 65;

		// Token: 0x040032CA RID: 13002
		public const int ManageFoodRestrictionsButtonHeight = 32;
	}
}
