using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Dialog_ManageFoodRestrictions : Window
	{
		
		// (get) Token: 0x0600590E RID: 22798 RVA: 0x001DB995 File Offset: 0x001D9B95
		// (set) Token: 0x0600590F RID: 22799 RVA: 0x001DB99D File Offset: 0x001D9B9D
		private FoodRestriction SelectedFoodRestriction
		{
			get
			{
				return this.selFoodRestrictionInt;
			}
			set
			{
				this.CheckSelectedFoodRestrictionHasName();
				this.selFoodRestrictionInt = value;
			}
		}

		
		// (get) Token: 0x06005910 RID: 22800 RVA: 0x001DB9AC File Offset: 0x001D9BAC
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 700f);
			}
		}

		
		private void CheckSelectedFoodRestrictionHasName()
		{
			if (this.SelectedFoodRestriction != null && this.SelectedFoodRestriction.label.NullOrEmpty())
			{
				this.SelectedFoodRestriction.label = "Unnamed";
			}
		}

		
		public Dialog_ManageFoodRestrictions(FoodRestriction selectedFoodRestriction)
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			if (Dialog_ManageFoodRestrictions.foodGlobalFilter == null)
			{
				Dialog_ManageFoodRestrictions.foodGlobalFilter = new ThingFilter();
				Dialog_ManageFoodRestrictions.foodGlobalFilter.SetAllow(ThingCategoryDefOf.Foods, true, null, null);
				Dialog_ManageFoodRestrictions.foodGlobalFilter.SetAllow(ThingCategoryDefOf.CorpsesHumanlike, true, null, null);
				Dialog_ManageFoodRestrictions.foodGlobalFilter.SetAllow(ThingCategoryDefOf.CorpsesAnimal, true, null, null);
			}
			this.SelectedFoodRestriction = selectedFoodRestriction;
		}

		
		public override void DoWindowContents(Rect inRect)
		{
			float num = 0f;
			Rect rect = new Rect(0f, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect, "SelectFoodRestriction".Translate(), true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (FoodRestriction localRestriction3 in Current.Game.foodRestrictionDatabase.AllFoodRestrictions)
				{
					FoodRestriction localRestriction = localRestriction3;
					list.Add(new FloatMenuOption(localRestriction.label, delegate
					{
						this.SelectedFoodRestriction = localRestriction;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			num += 10f;
			Rect rect2 = new Rect(num, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect2, "NewFoodRestriction".Translate(), true, true, true))
			{
				this.SelectedFoodRestriction = Current.Game.foodRestrictionDatabase.MakeNewFoodRestriction();
			}
			num += 10f;
			Rect rect3 = new Rect(num, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect3, "DeleteFoodRestriction".Translate(), true, true, true))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				foreach (FoodRestriction localRestriction2 in Current.Game.foodRestrictionDatabase.AllFoodRestrictions)
				{
					FoodRestriction localRestriction = localRestriction2;
					list2.Add(new FloatMenuOption(localRestriction.label, delegate
					{
						AcceptanceReport acceptanceReport = Current.Game.foodRestrictionDatabase.TryDelete(localRestriction);
						if (!acceptanceReport.Accepted)
						{
							Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
							return;
						}
						if (localRestriction == this.SelectedFoodRestriction)
						{
							this.SelectedFoodRestriction = null;
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			Rect rect4 = new Rect(0f, 40f, inRect.width, inRect.height - 40f - this.CloseButSize.y).ContractedBy(10f);
			if (this.SelectedFoodRestriction == null)
			{
				GUI.color = Color.grey;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect4, "NoFoodRestrictionSelected".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				return;
			}
			GUI.BeginGroup(rect4);
			Dialog_ManageFoodRestrictions.DoNameInputRect(new Rect(0f, 0f, 200f, 30f), ref this.SelectedFoodRestriction.label);
			ThingFilterUI.DoThingFilterConfigWindow(new Rect(0f, 40f, 300f, rect4.height - 45f - 10f), ref this.scrollPosition, this.SelectedFoodRestriction.filter, Dialog_ManageFoodRestrictions.foodGlobalFilter, 1, null, this.HiddenSpecialThingFilters(), true, null, null);
			GUI.EndGroup();
		}

		
		private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
		{
			yield return SpecialThingFilterDefOf.AllowFresh;
			yield break;
		}

		
		public override void PreClose()
		{
			base.PreClose();
			this.CheckSelectedFoodRestrictionHasName();
		}

		
		public static void DoNameInputRect(Rect rect, ref string name)
		{
			name = Widgets.TextField(rect, name, 30, Outfit.ValidNameRegex);
		}

		
		private Vector2 scrollPosition;

		
		private FoodRestriction selFoodRestrictionInt;

		
		private const float TopAreaHeight = 40f;

		
		private const float TopButtonHeight = 35f;

		
		private const float TopButtonWidth = 150f;

		
		private static ThingFilter foodGlobalFilter;
	}
}
