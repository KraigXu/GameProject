using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	
	public abstract class Bill : IExposable, ILoadReferenceable
	{
		
		// (get) Token: 0x06002A60 RID: 10848 RVA: 0x000F71C4 File Offset: 0x000F53C4
		public Map Map
		{
			get
			{
				return this.billStack.billGiver.Map;
			}
		}

		
		// (get) Token: 0x06002A61 RID: 10849 RVA: 0x000F71D6 File Offset: 0x000F53D6
		public virtual string Label
		{
			get
			{
				return this.recipe.label;
			}
		}

		
		// (get) Token: 0x06002A62 RID: 10850 RVA: 0x000F71E3 File Offset: 0x000F53E3
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.recipe);
			}
		}

		
		// (get) Token: 0x06002A63 RID: 10851 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool CheckIngredientsIfSociallyProper
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x06002A64 RID: 10852 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool CompletableEver
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x06002A65 RID: 10853 RVA: 0x00019EA1 File Offset: 0x000180A1
		protected virtual string StatusString
		{
			get
			{
				return null;
			}
		}

		
		// (get) Token: 0x06002A66 RID: 10854 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected virtual float StatusLineMinHeight
		{
			get
			{
				return 0f;
			}
		}

		
		// (get) Token: 0x06002A67 RID: 10855 RVA: 0x0001028D File Offset: 0x0000E48D
		protected virtual bool CanCopy
		{
			get
			{
				return true;
			}
		}

		
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

		
		public Bill()
		{
		}

		
		public Bill(RecipeDef recipe)
		{
			this.recipe = recipe;
			this.ingredientFilter = new ThingFilter();
			this.ingredientFilter.CopyAllowancesFrom(recipe.defaultIngredientFilter);
			this.InitializeAfterClone();
		}

		
		public void InitializeAfterClone()
		{
			this.loadID = Find.UniqueIDsManager.GetNextBillID();
		}

		
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

		
		public virtual void Notify_PawnDidWork(Pawn p)
		{
		}

		
		public virtual void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
		}

		
		public abstract bool ShouldDoNow();

		
		public virtual void Notify_DoBillStarted(Pawn billDoer)
		{
		}

		
		protected virtual void DoConfigInterface(Rect rect, Color baseColor)
		{
			rect.yMin += 29f;
			float y = rect.center.y;
			Widgets.InfoCardButton(rect.xMax - (rect.yMax - y) - 12f, y - 12f, this.recipe);
		}

		
		public virtual void DoStatusLineInterface(Rect rect)
		{
		}

		
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

		
		public static void CreateNoPawnsWithSkillDialog(RecipeDef recipe)
		{
			string text = "RecipeRequiresSkills".Translate(recipe.LabelCap);
			text += "\n\n";
			text += recipe.MinSkillString;
			Find.WindowStack.Add(new Dialog_MessageBox(text, null, null, null, null, null, false, null, null));
		}

		
		public virtual BillStoreModeDef GetStoreMode()
		{
			return BillStoreModeDefOf.BestStockpile;
		}

		
		public virtual Zone_Stockpile GetStoreZone()
		{
			return null;
		}

		
		public virtual void SetStoreMode(BillStoreModeDef mode, Zone_Stockpile zone = null)
		{
			Log.ErrorOnce("Tried to set store mode of a non-production bill", 27190980, false);
		}

		
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

		
		public override string ToString()
		{
			return this.GetUniqueLoadID();
		}

		
		[Unsaved(false)]
		public BillStack billStack;

		
		private int loadID = -1;

		
		public RecipeDef recipe;

		
		public bool suspended;

		
		public ThingFilter ingredientFilter;

		
		public float ingredientSearchRadius = 999f;

		
		public IntRange allowedSkillRange = new IntRange(0, 20);

		
		public Pawn pawnRestriction;

		
		public bool deleted;

		
		public int lastIngredientSearchFailTicks = -99999;

		
		public const int MaxIngredientSearchRadius = 999;

		
		public const float ButSize = 24f;

		
		private const float InterfaceBaseHeight = 53f;

		
		private const float InterfaceStatusLineHeight = 17f;
	}
}
