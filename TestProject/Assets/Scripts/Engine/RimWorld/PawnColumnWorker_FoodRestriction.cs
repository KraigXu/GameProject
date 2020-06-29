using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class PawnColumnWorker_FoodRestriction : PawnColumnWorker
	{
		
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
			if (Widgets.ButtonText(new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f), "ManageFoodRestrictions".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageFoodRestrictions(null));
			}
		}

		
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.foodRestriction == null)
			{
				return;
			}
			this.DoAssignFoodRestrictionButtons(rect, pawn);
		}

		
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

		
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(251f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.foodRestriction != null && pawn.foodRestriction.CurrentFoodRestriction != null)
			{
				return pawn.foodRestriction.CurrentFoodRestriction.id;
			}
			return int.MinValue;
		}

		
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

		
		private const int TopAreaHeight = 65;

		
		public const int ManageFoodRestrictionsButtonHeight = 32;
	}
}
