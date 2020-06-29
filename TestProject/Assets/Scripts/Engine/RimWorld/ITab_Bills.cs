using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class ITab_Bills : ITab
	{
		
		
		protected Building_WorkTable SelTable
		{
			get
			{
				return (Building_WorkTable)base.SelThing;
			}
		}

		
		public ITab_Bills()
		{
			this.size = ITab_Bills.WinSize;
			this.labelKey = "TabBills";
			this.tutorTag = "Bills";
		}

		
		protected override void FillTab()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.BillsTab, KnowledgeAmount.FrameDisplayed);
			Rect rect = new Rect(ITab_Bills.WinSize.x - ITab_Bills.PasteX, ITab_Bills.PasteY, ITab_Bills.PasteSize, ITab_Bills.PasteSize);
			if (BillUtility.Clipboard == null)
			{
				GUI.color = Color.gray;
				Widgets.DrawTextureFitted(rect, TexButton.Paste, 1f);
				GUI.color = Color.white;
				TooltipHandler.TipRegionByKey(rect, "PasteBillTip");
			}
			else if (!this.SelTable.def.AllRecipes.Contains(BillUtility.Clipboard.recipe) || !BillUtility.Clipboard.recipe.AvailableNow || !BillUtility.Clipboard.recipe.AvailableOnNow(this.SelTable))
			{
				GUI.color = Color.gray;
				Widgets.DrawTextureFitted(rect, TexButton.Paste, 1f);
				GUI.color = Color.white;
				TooltipHandler.TipRegionByKey(rect, "ClipboardBillNotAvailableHere");
			}
			else if (this.SelTable.billStack.Count >= 15)
			{
				GUI.color = Color.gray;
				Widgets.DrawTextureFitted(rect, TexButton.Paste, 1f);
				GUI.color = Color.white;
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, "PasteBillTip".Translate() + " (" + "PasteBillTip_LimitReached".Translate() + ")");
				}
			}
			else
			{
				if (Widgets.ButtonImageFitted(rect, TexButton.Paste, Color.white))
				{
					Bill bill = BillUtility.Clipboard.Clone();
					bill.InitializeAfterClone();
					this.SelTable.billStack.AddBill(bill);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect, "PasteBillTip");
			}
			Rect rect2 = new Rect(0f, 0f, ITab_Bills.WinSize.x, ITab_Bills.WinSize.y).ContractedBy(10f);
			Func<List<FloatMenuOption>> recipeOptionsMaker = delegate
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				for (int i = 0; i < this.SelTable.def.AllRecipes.Count; i++)
				{
					if (this.SelTable.def.AllRecipes[i].AvailableNow && this.SelTable.def.AllRecipes[i].AvailableOnNow(this.SelTable))
					{
						RecipeDef recipe = this.SelTable.def.AllRecipes[i];

						list.Add(new FloatMenuOption(recipe.LabelCap, delegate
						{
							List<Pawn> freeColonists = this.SelTable.Map.mapPawns.FreeColonists;
							Predicate<Pawn> predicate;
							if ((predicate=default ) == null)
							{
								predicate = ( ((Pawn col) => recipe.PawnSatisfiesSkillRequirements(col)));
							}
							if (!freeColonists.Any(predicate))
							{
								Bill.CreateNoPawnsWithSkillDialog(recipe);
							}
							Bill bill2 = recipe.MakeNewBill();
							this.SelTable.billStack.AddBill(bill2);
							if (recipe.conceptLearned != null)
							{
								PlayerKnowledgeDatabase.KnowledgeDemonstrated(recipe.conceptLearned, KnowledgeAmount.Total);
							}
							if (TutorSystem.TutorialMode)
							{
								TutorSystem.Notify_Event("AddBill-" + recipe.LabelCap.Resolve());
							}
						}, recipe.ProducedThingDef, MenuOptionPriority.Default, null, null, 29f, (Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, recipe), null));
					}
				}
				if (!list.Any<FloatMenuOption>())
				{
					list.Add(new FloatMenuOption("NoneBrackets".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				return list;
			};
			this.mouseoverBill = this.SelTable.billStack.DoListing(rect2, recipeOptionsMaker, ref this.scrollPosition, ref this.viewHeight);
		}

		
		public override void TabUpdate()
		{
			if (this.mouseoverBill != null)
			{
				this.mouseoverBill.TryDrawIngredientSearchRadiusOnMap(this.SelTable.Position);
				this.mouseoverBill = null;
			}
		}

		
		private float viewHeight = 1000f;

		
		private Vector2 scrollPosition;

		
		private Bill mouseoverBill;

		
		private static readonly Vector2 WinSize = new Vector2(420f, 480f);

		
		[TweakValue("Interface", 0f, 128f)]
		private static float PasteX = 48f;

		
		[TweakValue("Interface", 0f, 128f)]
		private static float PasteY = 3f;

		
		[TweakValue("Interface", 0f, 32f)]
		private static float PasteSize = 24f;
	}
}
