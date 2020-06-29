using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class Bill_Medical : Bill
	{
		
		// (get) Token: 0x06004211 RID: 16913 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool CheckIngredientsIfSociallyProper
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06004212 RID: 16914 RVA: 0x00010306 File Offset: 0x0000E506
		protected override bool CanCopy
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06004213 RID: 16915 RVA: 0x00161154 File Offset: 0x0015F354
		public override bool CompletableEver
		{
			get
			{
				return !this.recipe.targetsBodyPart || this.recipe.Worker.GetPartsToApplyOn(this.GiverPawn, this.recipe).Contains(this.part);
			}
		}

		
		// (get) Token: 0x06004214 RID: 16916 RVA: 0x0016118F File Offset: 0x0015F38F
		// (set) Token: 0x06004215 RID: 16917 RVA: 0x00161197 File Offset: 0x0015F397
		public BodyPartRecord Part
		{
			get
			{
				return this.part;
			}
			set
			{
				if (this.billStack == null && this.part != null)
				{
					Log.Error("Can only set Bill_Medical.Part after the bill has been added to a pawn's bill stack.", false);
					return;
				}
				this.part = value;
			}
		}

		
		// (get) Token: 0x06004216 RID: 16918 RVA: 0x001611BC File Offset: 0x0015F3BC
		public Pawn GiverPawn
		{
			get
			{
				Pawn pawn = this.billStack.billGiver as Pawn;
				Corpse corpse = this.billStack.billGiver as Corpse;
				if (corpse != null)
				{
					pawn = corpse.InnerPawn;
				}
				if (pawn == null)
				{
					throw new InvalidOperationException("Medical bill on non-pawn.");
				}
				return pawn;
			}
		}

		
		// (get) Token: 0x06004217 RID: 16919 RVA: 0x00161204 File Offset: 0x0015F404
		public override string Label
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.recipe.Worker.GetLabelWhenUsedOn(this.GiverPawn, this.part));
				if (this.Part != null && !this.recipe.hideBodyPartNames)
				{
					stringBuilder.Append(" (" + this.Part.Label + ")");
				}
				return stringBuilder.ToString();
			}
		}

		
		public Bill_Medical()
		{
		}

		
		public Bill_Medical(RecipeDef recipe) : base(recipe)
		{
		}

		
		public override bool ShouldDoNow()
		{
			return !this.suspended;
		}

		
		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			base.Notify_IterationCompleted(billDoer, ingredients);
			if (this.CompletableEver)
			{
				Pawn giverPawn = this.GiverPawn;
				this.recipe.Worker.ApplyOnPawn(giverPawn, this.Part, billDoer, ingredients, this);
				if (giverPawn.RaceProps.IsFlesh)
				{
					giverPawn.records.Increment(RecordDefOf.OperationsReceived);
					billDoer.records.Increment(RecordDefOf.OperationsPerformed);
				}
			}
			this.billStack.Delete(this);
		}

		
		public override void Notify_DoBillStarted(Pawn billDoer)
		{
			base.Notify_DoBillStarted(billDoer);
			this.consumedInitialMedicineDef = null;
			if (!this.GiverPawn.Dead && this.recipe.anesthetize && HealthUtility.TryAnesthetize(this.GiverPawn))
			{
				List<ThingCountClass> placedThings = billDoer.CurJob.placedThings;
				int i = 0;
				while (i < placedThings.Count)
				{
					if (placedThings[i].thing is Medicine)
					{
						this.recipe.Worker.ConsumeIngredient(placedThings[i].thing.SplitOff(1), this.recipe, billDoer.MapHeld);
						ThingCountClass thingCountClass = placedThings[i];
						int count = thingCountClass.Count;
						thingCountClass.Count = count - 1;
						this.consumedInitialMedicineDef = placedThings[i].thing.def;
						if (placedThings[i].thing.Destroyed || placedThings[i].Count <= 0)
						{
							placedThings.RemoveAt(i);
							return;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_BodyParts.Look(ref this.part, "part", null);
			Scribe_Defs.Look<ThingDef>(ref this.consumedInitialMedicineDef, "consumedInitialMedicineDef");
			BackCompatibility.PostExposeData(this);
		}

		
		public override Bill Clone()
		{
			Bill_Medical bill_Medical = (Bill_Medical)base.Clone();
			bill_Medical.part = this.part;
			bill_Medical.consumedInitialMedicineDef = this.consumedInitialMedicineDef;
			return bill_Medical;
		}

		
		private BodyPartRecord part;

		
		public ThingDef consumedInitialMedicineDef;

		
		public int temp_partIndexToSetLater;
	}
}
