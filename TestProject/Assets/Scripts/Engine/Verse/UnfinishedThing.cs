using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class UnfinishedThing : ThingWithComps
	{
		
		// (get) Token: 0x0600171C RID: 5916 RVA: 0x00084D10 File Offset: 0x00082F10
		// (set) Token: 0x0600171D RID: 5917 RVA: 0x00084D18 File Offset: 0x00082F18
		public Pawn Creator
		{
			get
			{
				return this.creatorInt;
			}
			set
			{
				if (value == null)
				{
					Log.Error("Cannot set creator to null.", false);
					return;
				}
				this.creatorInt = value;
				this.creatorName = value.LabelShort;
			}
		}

		
		// (get) Token: 0x0600171E RID: 5918 RVA: 0x00084D3C File Offset: 0x00082F3C
		public RecipeDef Recipe
		{
			get
			{
				return this.recipeInt;
			}
		}

		
		// (get) Token: 0x0600171F RID: 5919 RVA: 0x00084D44 File Offset: 0x00082F44
		// (set) Token: 0x06001720 RID: 5920 RVA: 0x00084D78 File Offset: 0x00082F78
		public Bill_ProductionWithUft BoundBill
		{
			get
			{
				if (this.boundBillInt != null && (this.boundBillInt.DeletedOrDereferenced || this.boundBillInt.BoundUft != this))
				{
					this.boundBillInt = null;
				}
				return this.boundBillInt;
			}
			set
			{
				if (value == this.boundBillInt)
				{
					return;
				}
				Bill_ProductionWithUft bill_ProductionWithUft = this.boundBillInt;
				this.boundBillInt = value;
				if (bill_ProductionWithUft != null && bill_ProductionWithUft.BoundUft == this)
				{
					bill_ProductionWithUft.SetBoundUft(null, false);
				}
				if (value != null)
				{
					this.recipeInt = value.recipe;
					if (value.BoundUft != this)
					{
						value.SetBoundUft(this, false);
					}
				}
			}
		}

		
		// (get) Token: 0x06001721 RID: 5921 RVA: 0x00084DD4 File Offset: 0x00082FD4
		public Thing BoundWorkTable
		{
			get
			{
				if (this.BoundBill == null)
				{
					return null;
				}
				Thing thing = this.BoundBill.billStack.billGiver as Thing;
				if (thing.Destroyed)
				{
					return null;
				}
				return thing;
			}
		}

		
		// (get) Token: 0x06001722 RID: 5922 RVA: 0x00084E0C File Offset: 0x0008300C
		public override string LabelNoCount
		{
			get
			{
				if (this.Recipe == null)
				{
					return base.LabelNoCount;
				}
				if (base.Stuff == null)
				{
					return "UnfinishedItem".Translate(this.Recipe.products[0].thingDef.label);
				}
				return "UnfinishedItemWithStuff".Translate(base.Stuff.LabelAsStuff, this.Recipe.products[0].thingDef.label);
			}
		}

		
		// (get) Token: 0x06001723 RID: 5923 RVA: 0x00084E9F File Offset: 0x0008309F
		public override string DescriptionDetailed
		{
			get
			{
				if (this.Recipe == null)
				{
					return base.LabelNoCount;
				}
				return this.Recipe.ProducedThingDef.DescriptionDetailed;
			}
		}

		
		// (get) Token: 0x06001724 RID: 5924 RVA: 0x00084EC0 File Offset: 0x000830C0
		public override string DescriptionFlavor
		{
			get
			{
				if (this.Recipe == null)
				{
					return base.LabelNoCount;
				}
				return this.Recipe.ProducedThingDef.description;
			}
		}

		
		// (get) Token: 0x06001725 RID: 5925 RVA: 0x00084EE1 File Offset: 0x000830E1
		public bool Initialized
		{
			get
			{
				return this.workLeft > -5000f;
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving && this.boundBillInt != null && this.boundBillInt.DeletedOrDereferenced)
			{
				this.boundBillInt = null;
			}
			Scribe_References.Look<Pawn>(ref this.creatorInt, "creator", false);
			Scribe_Values.Look<string>(ref this.creatorName, "creatorName", null, false);
			Scribe_References.Look<Bill_ProductionWithUft>(ref this.boundBillInt, "bill", false);
			Scribe_Defs.Look<RecipeDef>(ref this.recipeInt, "recipe");
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Collections.Look<Thing>(ref this.ingredients, "ingredients", LookMode.Deep, Array.Empty<object>());
		}

		
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode == DestroyMode.Cancel)
			{
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					int num = GenMath.RoundRandom((float)this.ingredients[i].stackCount * 0.75f);
					if (num > 0)
					{
						this.ingredients[i].stackCount = num;
						GenPlace.TryPlaceThing(this.ingredients[i], base.Position, base.Map, ThingPlaceMode.Near, null, null, default(Rot4));
					}
				}
				this.ingredients.Clear();
			}
			base.Destroy(mode);
			this.BoundBill = null;
		}

		
		public override IEnumerable<Gizmo> GetGizmos()
		{

			IEnumerator<Gizmo> enumerator = null;
			yield return new Command_Action
			{
				defaultLabel = "CommandCancelConstructionLabel".Translate(),
				defaultDesc = "CommandCancelConstructionDesc".Translate(),
				icon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true),
				hotKey = KeyBindingDefOf.Designator_Cancel,
				action = delegate
				{
					this.Destroy(DestroyMode.Cancel);
				}
			};
			yield break;
			yield break;
		}

		
		public Bill_ProductionWithUft BillOnTableForMe(Thing workTable)
		{
			if (this.Recipe.AllRecipeUsers.Contains(workTable.def))
			{
				IBillGiver billGiver = (IBillGiver)workTable;
				for (int i = 0; i < billGiver.BillStack.Count; i++)
				{
					Bill_ProductionWithUft bill_ProductionWithUft = billGiver.BillStack[i] as Bill_ProductionWithUft;
					if (bill_ProductionWithUft != null && bill_ProductionWithUft.ShouldDoNow() && bill_ProductionWithUft != null && bill_ProductionWithUft.recipe == this.Recipe)
					{
						return bill_ProductionWithUft;
					}
				}
			}
			return null;
		}

		
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.BoundWorkTable != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), this.BoundWorkTable.TrueCenter());
			}
		}

		
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "Author".Translate() + ": " + this.creatorName;
			return text + ("\n" + "WorkLeft".Translate() + ": " + this.workLeft.ToStringWorkAmount());
		}

		
		private Pawn creatorInt;

		
		private string creatorName = "ErrorCreatorName";

		
		private RecipeDef recipeInt;

		
		public List<Thing> ingredients = new List<Thing>();

		
		private Bill_ProductionWithUft boundBillInt;

		
		public float workLeft = -10000f;

		
		private const float CancelIngredientRecoveryFraction = 0.75f;
	}
}
