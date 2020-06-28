using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000615 RID: 1557
	public abstract class Bill : IExposable, ILoadReferenceable
	{
		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06002A60 RID: 10848 RVA: 0x000F71C4 File Offset: 0x000F53C4
		public Map Map
		{
			get
			{
				return this.billStack.billGiver.Map;
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06002A61 RID: 10849 RVA: 0x000F71D6 File Offset: 0x000F53D6
		public virtual string Label
		{
			get
			{
				return this.recipe.label;
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06002A62 RID: 10850 RVA: 0x000F71E3 File Offset: 0x000F53E3
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.recipe);
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06002A63 RID: 10851 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool CheckIngredientsIfSociallyProper
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06002A64 RID: 10852 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool CompletableEver
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06002A65 RID: 10853 RVA: 0x00019EA1 File Offset: 0x000180A1
		protected virtual string StatusString
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06002A66 RID: 10854 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected virtual float StatusLineMinHeight
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06002A67 RID: 10855 RVA: 0x0001028D File Offset: 0x0000E48D
		protected virtual bool CanCopy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06002A68 RID: 10856 RVA: 0x000F71F8 File Offset: 0x000F53F8
		public bool DeletedOrDereferenced
		{
			get
			{
				if (this.deleted)
				{
					return true;
				}
				Thing thing = this.billStack.billGiver as Thing;
				return thing != null && thing.Destroyed;
			}
		}

		// Token: 0x06002A69 RID: 10857 RVA: 0x000F722E File Offset: 0x000F542E
		public Bill()
		{
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x000F7264 File Offset: 0x000F5464
		public Bill(RecipeDef recipe)
		{
			this.recipe = recipe;
			this.ingredientFilter = new ThingFilter();
			this.ingredientFilter.CopyAllowancesFrom(recipe.defaultIngredientFilter);
			this.InitializeAfterClone();
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x000F72CB File Offset: 0x000F54CB
		public void InitializeAfterClone()
		{
			this.loadID = Find.UniqueIDsManager.GetNextBillID();
		}

		// Token: 0x06002A6C RID: 10860 RVA: 0x000F72E0 File Offset: 0x000F54E0
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Defs.Look<RecipeDef>(ref this.recipe, "recipe");
			Scribe_Values.Look<bool>(ref this.suspended, "suspended", false, false);
			Scribe_Values.Look<float>(ref this.ingredientSearchRadius, "ingredientSearchRadius", 999f, false);
			Scribe_Values.Look<IntRange>(ref this.allowedSkillRange, "allowedSkillRange", default(IntRange), false);
			Scribe_References.Look<Pawn>(ref this.pawnRestriction, "pawnRestriction", false);
			if (Scribe.mode == LoadSaveMode.Saving && this.recipe.fixedIngredientFilter != null)
			{
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (!this.recipe.fixedIngredientFilter.Allows(thingDef))
					{
						this.ingredientFilter.SetAllow(thingDef, false);
					}
				}
			}
			Scribe_Deep.Look<ThingFilter>(ref this.ingredientFilter, "ingredientFilter", Array.Empty<object>());
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x000F73E4 File Offset: 0x000F55E4
		public virtual bool PawnAllowedToStartAnew(Pawn p)
		{
			if (this.pawnRestriction != null)
			{
				return this.pawnRestriction == p;
			}
			if (this.recipe.workSkill != null)
			{
				int level = p.skills.GetSkill(this.recipe.workSkill).Level;
				if (level < this.allowedSkillRange.min)
				{
					JobFailReason.Is("UnderAllowedSkill".Translate(this.allowedSkillRange.min), this.Label);
					return false;
				}
				if (level > this.allowedSkillRange.max)
				{
					JobFailReason.Is("AboveAllowedSkill".Translate(this.allowedSkillRange.max), this.Label);
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_PawnDidWork(Pawn p)
		{
		}

		// Token: 0x06002A6F RID: 10863 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
		}

		// Token: 0x06002A70 RID: 10864
		public abstract bool ShouldDoNow();

		// Token: 0x06002A71 RID: 10865 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_DoBillStarted(Pawn billDoer)
		{
		}

		// Token: 0x06002A72 RID: 10866 RVA: 0x000F74A4 File Offset: 0x000F56A4
		protected virtual void DoConfigInterface(Rect rect, Color baseColor)
		{
			rect.yMin += 29f;
			float y = rect.center.y;
			Widgets.InfoCardButton(rect.xMax - (rect.yMax - y) - 12f, y - 12f, this.recipe);
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void DoStatusLineInterface(Rect rect)
		{
		}

		// Token: 0x06002A74 RID: 10868 RVA: 0x000F74FC File Offset: 0x000F56FC
		public Rect DoInterface(float x, float y, float width, int index)
		{
			Rect rect = new Rect(x, y, width, 53f);
			float num = 0f;
			if (!this.StatusString.NullOrEmpty())
			{
				num = Mathf.Max(17f, this.StatusLineMinHeight);
			}
			rect.height += num;
			Color white = Color.white;
			if (!this.ShouldDoNow())
			{
				white = new Color(1f, 0.7f, 0.7f, 0.7f);
			}
			GUI.color = white;
			Text.Font = GameFont.Small;
			if (index % 2 == 0)
			{
				Widgets.DrawAltRect(rect);
			}
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, 24f, 24f);
			if (this.billStack.IndexOf(this) > 0)
			{
				if (Widgets.ButtonImage(rect2, TexButton.ReorderUp, white, true))
				{
					this.billStack.Reorder(this, -1);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect2, "ReorderBillUpTip");
			}
			if (this.billStack.IndexOf(this) < this.billStack.Count - 1)
			{
				Rect rect3 = new Rect(0f, 24f, 24f, 24f);
				if (Widgets.ButtonImage(rect3, TexButton.ReorderDown, white, true))
				{
					this.billStack.Reorder(this, 1);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect3, "ReorderBillDownTip");
			}
			Widgets.Label(new Rect(28f, 0f, rect.width - 48f - 20f, rect.height + 5f), this.LabelCap);
			this.DoConfigInterface(rect.AtZero(), white);
			Rect rect4 = new Rect(rect.width - 24f, 0f, 24f, 24f);
			if (Widgets.ButtonImage(rect4, TexButton.DeleteX, white, white * GenUI.SubtleMouseoverColor, true))
			{
				this.billStack.Delete(this);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			TooltipHandler.TipRegionByKey(rect4, "DeleteBillTip");
			Rect rect6;
			if (this.CanCopy)
			{
				Rect rect5 = new Rect(rect4);
				rect5.x -= rect5.width + 4f;
				if (Widgets.ButtonImageFitted(rect5, TexButton.Copy, white))
				{
					BillUtility.Clipboard = this.Clone();
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect5, "CopyBillTip");
				rect6 = new Rect(rect5);
			}
			else
			{
				rect6 = new Rect(rect4);
			}
			rect6.x -= rect6.width + 4f;
			if (Widgets.ButtonImage(rect6, TexButton.Suspend, white, true))
			{
				this.suspended = !this.suspended;
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			TooltipHandler.TipRegionByKey(rect6, "SuspendBillTip");
			if (!this.StatusString.NullOrEmpty())
			{
				Text.Font = GameFont.Tiny;
				Rect rect7 = new Rect(24f, rect.height - num, rect.width - 24f, num);
				Widgets.Label(rect7, this.StatusString);
				this.DoStatusLineInterface(rect7);
			}
			GUI.EndGroup();
			if (this.suspended)
			{
				Text.Font = GameFont.Medium;
				Text.Anchor = TextAnchor.MiddleCenter;
				Rect rect8 = new Rect(rect.x + rect.width / 2f - 70f, rect.y + rect.height / 2f - 20f, 140f, 40f);
				GUI.DrawTexture(rect8, TexUI.GrayTextBG);
				Widgets.Label(rect8, "SuspendedCaps".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
			}
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			return rect;
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x000F78A0 File Offset: 0x000F5AA0
		public bool IsFixedOrAllowedIngredient(Thing thing)
		{
			for (int i = 0; i < this.recipe.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = this.recipe.ingredients[i];
				if (ingredientCount.IsFixedIngredient && ingredientCount.filter.Allows(thing))
				{
					return true;
				}
			}
			return this.recipe.fixedIngredientFilter.Allows(thing) && this.ingredientFilter.Allows(thing);
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x000F7914 File Offset: 0x000F5B14
		public bool IsFixedOrAllowedIngredient(ThingDef def)
		{
			for (int i = 0; i < this.recipe.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = this.recipe.ingredients[i];
				if (ingredientCount.IsFixedIngredient && ingredientCount.filter.Allows(def))
				{
					return true;
				}
			}
			return this.recipe.fixedIngredientFilter.Allows(def) && this.ingredientFilter.Allows(def);
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x000F7988 File Offset: 0x000F5B88
		public static void CreateNoPawnsWithSkillDialog(RecipeDef recipe)
		{
			string text = "RecipeRequiresSkills".Translate(recipe.LabelCap);
			text += "\n\n";
			text += recipe.MinSkillString;
			Find.WindowStack.Add(new Dialog_MessageBox(text, null, null, null, null, null, false, null, null));
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x000F79E6 File Offset: 0x000F5BE6
		public virtual BillStoreModeDef GetStoreMode()
		{
			return BillStoreModeDefOf.BestStockpile;
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual Zone_Stockpile GetStoreZone()
		{
			return null;
		}

		// Token: 0x06002A7A RID: 10874 RVA: 0x000F79ED File Offset: 0x000F5BED
		public virtual void SetStoreMode(BillStoreModeDef mode, Zone_Stockpile zone = null)
		{
			Log.ErrorOnce("Tried to set store mode of a non-production bill", 27190980, false);
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x000F7A00 File Offset: 0x000F5C00
		public virtual Bill Clone()
		{
			Bill bill = (Bill)Activator.CreateInstance(base.GetType());
			bill.recipe = this.recipe;
			bill.suspended = this.suspended;
			bill.ingredientFilter = new ThingFilter();
			bill.ingredientFilter.CopyAllowancesFrom(this.ingredientFilter);
			bill.ingredientSearchRadius = this.ingredientSearchRadius;
			bill.allowedSkillRange = this.allowedSkillRange;
			bill.pawnRestriction = this.pawnRestriction;
			return bill;
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x000F7A78 File Offset: 0x000F5C78
		public virtual void ValidateSettings()
		{
			if (this.pawnRestriction != null && (this.pawnRestriction.Dead || this.pawnRestriction.Faction != Faction.OfPlayer || this.pawnRestriction.IsKidnapped()))
			{
				if (this != BillUtility.Clipboard)
				{
					Messages.Message("MessageBillValidationPawnUnavailable".Translate(this.pawnRestriction.LabelShortCap, this.Label, this.billStack.billGiver.LabelShort), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
				}
				this.pawnRestriction = null;
				return;
			}
		}

		// Token: 0x06002A7D RID: 10877 RVA: 0x000F7B2B File Offset: 0x000F5D2B
		public string GetUniqueLoadID()
		{
			return string.Concat(new object[]
			{
				"Bill_",
				this.recipe.defName,
				"_",
				this.loadID
			});
		}

		// Token: 0x06002A7E RID: 10878 RVA: 0x000F7B64 File Offset: 0x000F5D64
		public override string ToString()
		{
			return this.GetUniqueLoadID();
		}

		// Token: 0x04001952 RID: 6482
		[Unsaved(false)]
		public BillStack billStack;

		// Token: 0x04001953 RID: 6483
		private int loadID = -1;

		// Token: 0x04001954 RID: 6484
		public RecipeDef recipe;

		// Token: 0x04001955 RID: 6485
		public bool suspended;

		// Token: 0x04001956 RID: 6486
		public ThingFilter ingredientFilter;

		// Token: 0x04001957 RID: 6487
		public float ingredientSearchRadius = 999f;

		// Token: 0x04001958 RID: 6488
		public IntRange allowedSkillRange = new IntRange(0, 20);

		// Token: 0x04001959 RID: 6489
		public Pawn pawnRestriction;

		// Token: 0x0400195A RID: 6490
		public bool deleted;

		// Token: 0x0400195B RID: 6491
		public int lastIngredientSearchFailTicks = -99999;

		// Token: 0x0400195C RID: 6492
		public const int MaxIngredientSearchRadius = 999;

		// Token: 0x0400195D RID: 6493
		public const float ButSize = 24f;

		// Token: 0x0400195E RID: 6494
		private const float InterfaceBaseHeight = 53f;

		// Token: 0x0400195F RID: 6495
		private const float InterfaceStatusLineHeight = 17f;
	}
}
