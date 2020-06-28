using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002E0 RID: 736
	public class Corpse : ThingWithComps, IThingHolder, IThoughtGiver, IStrippable, IBillGiver
	{
		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060014A9 RID: 5289 RVA: 0x0007A1AE File Offset: 0x000783AE
		// (set) Token: 0x060014AA RID: 5290 RVA: 0x0007A1CC File Offset: 0x000783CC
		public Pawn InnerPawn
		{
			get
			{
				if (this.innerContainer.Count > 0)
				{
					return this.innerContainer[0];
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this.innerContainer.Clear();
					return;
				}
				if (this.innerContainer.Count > 0)
				{
					Log.Error("Setting InnerPawn in corpse that already has one.", false);
					this.innerContainer.Clear();
				}
				this.innerContainer.TryAdd(value, true);
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060014AB RID: 5291 RVA: 0x0007A21A File Offset: 0x0007841A
		// (set) Token: 0x060014AC RID: 5292 RVA: 0x0007A22D File Offset: 0x0007842D
		public int Age
		{
			get
			{
				return Find.TickManager.TicksGame - this.timeOfDeath;
			}
			set
			{
				this.timeOfDeath = Find.TickManager.TicksGame - value;
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x060014AD RID: 5293 RVA: 0x0007A244 File Offset: 0x00078444
		public override string LabelNoCount
		{
			get
			{
				if (this.Bugged)
				{
					Log.ErrorOnce("Corpse.Label while Bugged", 57361644, false);
					return "";
				}
				return "DeadLabel".Translate(this.InnerPawn.Label, this.InnerPawn);
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x060014AE RID: 5294 RVA: 0x0007A299 File Offset: 0x00078499
		public override bool IngestibleNow
		{
			get
			{
				if (this.Bugged)
				{
					Log.Error("IngestibleNow on Corpse while Bugged.", false);
					return false;
				}
				return base.IngestibleNow && this.InnerPawn.RaceProps.IsFlesh && this.GetRotStage() == RotStage.Fresh;
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x060014AF RID: 5295 RVA: 0x0007A2DC File Offset: 0x000784DC
		public RotDrawMode CurRotDrawMode
		{
			get
			{
				CompRottable comp = base.GetComp<CompRottable>();
				if (comp != null)
				{
					if (comp.Stage == RotStage.Rotting)
					{
						return RotDrawMode.Rotting;
					}
					if (comp.Stage == RotStage.Dessicated)
					{
						return RotDrawMode.Dessicated;
					}
				}
				return RotDrawMode.Fresh;
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x060014B0 RID: 5296 RVA: 0x0007A30C File Offset: 0x0007850C
		private bool ShouldVanish
		{
			get
			{
				return this.InnerPawn.RaceProps.Animal && this.vanishAfterTimestamp > 0 && this.Age >= this.vanishAfterTimestamp && base.Spawned && this.GetRoom(RegionType.Set_Passable) != null && this.GetRoom(RegionType.Set_Passable).TouchesMapEdge && !base.Map.roofGrid.Roofed(base.Position);
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x0007A37C File Offset: 0x0007857C
		public BillStack BillStack
		{
			get
			{
				return this.operationsBillStack;
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x060014B2 RID: 5298 RVA: 0x0007A384 File Offset: 0x00078584
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				yield return this.InteractionCell;
				yield break;
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x060014B3 RID: 5299 RVA: 0x0007A394 File Offset: 0x00078594
		public bool Bugged
		{
			get
			{
				return this.innerContainer.Count == 0 || this.innerContainer[0] == null || this.innerContainer[0].def == null || this.innerContainer[0].kindDef == null;
			}
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x0007A3E5 File Offset: 0x000785E5
		public Corpse()
		{
			this.operationsBillStack = new BillStack(this);
			this.innerContainer = new ThingOwner<Pawn>(this, true, LookMode.Reference);
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x0007A418 File Offset: 0x00078618
		public bool CurrentlyUsableForBills()
		{
			return this.InteractionCell.IsValid;
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x0007A433 File Offset: 0x00078633
		public bool UsableForBillsAfterFueling()
		{
			return this.CurrentlyUsableForBills();
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x0007A43B File Offset: 0x0007863B
		public bool AnythingToStrip()
		{
			return this.InnerPawn.AnythingToStrip();
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x0007A448 File Offset: 0x00078648
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x0007A450 File Offset: 0x00078650
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x0007A45E File Offset: 0x0007865E
		public override void PostMake()
		{
			base.PostMake();
			this.timeOfDeath = Find.TickManager.TicksGame;
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x0007A476 File Offset: 0x00078676
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Bugged)
			{
				Log.Error(this + " spawned in bugged state.", false);
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			this.InnerPawn.Rotation = Rot4.South;
			this.NotifyColonistBar();
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x0007A4B0 File Offset: 0x000786B0
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			if (!this.Bugged)
			{
				this.NotifyColonistBar();
			}
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0007A4C8 File Offset: 0x000786C8
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Pawn pawn = null;
			if (!this.Bugged)
			{
				pawn = this.InnerPawn;
				this.NotifyColonistBar();
				this.innerContainer.Clear();
			}
			base.Destroy(mode);
			if (pawn != null)
			{
				Corpse.PostCorpseDestroy(pawn);
			}
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0007A508 File Offset: 0x00078708
		public static void PostCorpseDestroy(Pawn pawn)
		{
			if (pawn.ownership != null)
			{
				pawn.ownership.UnclaimAll();
			}
			if (pawn.equipment != null)
			{
				pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
			}
			pawn.inventory.DestroyAll(DestroyMode.Vanish);
			if (pawn.apparel != null)
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x0007A55C File Offset: 0x0007875C
		public override void TickRare()
		{
			base.TickRare();
			if (base.Destroyed)
			{
				return;
			}
			if (this.Bugged)
			{
				Log.Error(this + " has null innerPawn. Destroying.", false);
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			this.InnerPawn.TickRare();
			if (this.vanishAfterTimestamp < 0 || this.GetRotStage() != RotStage.Dessicated)
			{
				this.vanishAfterTimestamp = this.Age + 6000000;
			}
			if (this.ShouldVanish)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x0007A5D8 File Offset: 0x000787D8
		protected override void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			BodyPartRecord bodyPartRecord = this.GetBestBodyPartToEat(ingester, nutritionWanted);
			if (bodyPartRecord == null)
			{
				Log.Error(string.Concat(new object[]
				{
					ingester,
					" ate ",
					this,
					" but no body part was found. Replacing with core part."
				}), false);
				bodyPartRecord = this.InnerPawn.RaceProps.body.corePart;
			}
			float bodyPartNutrition = FoodUtility.GetBodyPartNutrition(this, bodyPartRecord);
			if (bodyPartRecord == this.InnerPawn.RaceProps.body.corePart)
			{
				if (PawnUtility.ShouldSendNotificationAbout(this.InnerPawn) && this.InnerPawn.RaceProps.Humanlike)
				{
					Messages.Message("MessageEatenByPredator".Translate(this.InnerPawn.LabelShort, ingester.Named("PREDATOR"), this.InnerPawn.Named("EATEN")).CapitalizeFirst(), ingester, MessageTypeDefOf.NegativeEvent, true);
				}
				numTaken = 1;
			}
			else
			{
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.InnerPawn, bodyPartRecord);
				hediff_MissingPart.lastInjury = HediffDefOf.Bite;
				hediff_MissingPart.IsFresh = true;
				this.InnerPawn.health.AddHediff(hediff_MissingPart, null, null, null);
				numTaken = 0;
			}
			nutritionIngested = bodyPartNutrition;
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x0007A714 File Offset: 0x00078914
		public override IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			foreach (Thing thing in this.InnerPawn.ButcherProducts(butcher, efficiency))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			if (this.InnerPawn.RaceProps.BloodDef != null)
			{
				FilthMaker.TryMakeFilth(butcher.Position, butcher.Map, this.InnerPawn.RaceProps.BloodDef, this.InnerPawn.LabelIndefinite(), 1, FilthSourceFlags.None);
			}
			if (this.InnerPawn.RaceProps.Humanlike)
			{
				if (butcher.needs.mood != null)
				{
					butcher.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ButcheredHumanlikeCorpse, null);
				}
				foreach (Pawn pawn in butcher.Map.mapPawns.SpawnedPawnsInFaction(butcher.Faction))
				{
					if (pawn != butcher && pawn.needs != null && pawn.needs.mood != null && pawn.needs.mood.thoughts != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowButcheredHumanlikeCorpse, null);
					}
				}
				TaleRecorder.RecordTale(TaleDefOf.ButcheredHumanlikeCorpse, new object[]
				{
					butcher
				});
			}
			yield break;
			yield break;
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0007A734 File Offset: 0x00078934
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.timeOfDeath, "timeOfDeath", 0, false);
			Scribe_Values.Look<int>(ref this.vanishAfterTimestamp, "vanishAfterTimestamp", 0, false);
			Scribe_Values.Look<bool>(ref this.everBuriedInSarcophagus, "everBuriedInSarcophagus", false, false);
			Scribe_Deep.Look<BillStack>(ref this.operationsBillStack, "operationsBillStack", new object[]
			{
				this
			});
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x0007A7B1 File Offset: 0x000789B1
		public void Strip()
		{
			this.InnerPawn.Strip();
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x0007A7BE File Offset: 0x000789BE
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.InnerPawn.Drawer.renderer.RenderPawnAt(drawLoc);
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0007A7D8 File Offset: 0x000789D8
		public Thought_Memory GiveObservedThought()
		{
			if (!this.InnerPawn.RaceProps.Humanlike)
			{
				return null;
			}
			if (this.StoringThing() == null)
			{
				Thought_MemoryObservation thought_MemoryObservation;
				if (this.IsNotFresh())
				{
					thought_MemoryObservation = (Thought_MemoryObservation)ThoughtMaker.MakeThought(ThoughtDefOf.ObservedLayingRottingCorpse);
				}
				else
				{
					thought_MemoryObservation = (Thought_MemoryObservation)ThoughtMaker.MakeThought(ThoughtDefOf.ObservedLayingCorpse);
				}
				thought_MemoryObservation.Target = this;
				return thought_MemoryObservation;
			}
			return null;
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x0007A838 File Offset: 0x00078A38
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.InnerPawn.Faction != null)
			{
				stringBuilder.AppendLineTagged("Faction".Translate() + ": " + this.InnerPawn.Faction.NameColored);
			}
			stringBuilder.AppendLine("DeadTime".Translate(this.Age.ToStringTicksToPeriodVague(true, false)));
			float num = 1f - this.InnerPawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(this.InnerPawn.RaceProps.body.corePart);
			if (num != 0f)
			{
				stringBuilder.AppendLine("CorpsePercentMissing".Translate() + ": " + num.ToStringPercent());
			}
			stringBuilder.AppendLine(base.GetInspectString());
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x0007A92B File Offset: 0x00078B2B
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in this.<>n__0())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.GetRotStage() == RotStage.Fresh)
			{
				StatDef meatAmount = StatDefOf.MeatAmount;
				yield return new StatDrawEntry(meatAmount.category, meatAmount, this.InnerPawn.GetStatValue(meatAmount, true), StatRequest.For(this.InnerPawn), ToStringNumberSense.Undefined, null, false);
				StatDef leatherAmount = StatDefOf.LeatherAmount;
				yield return new StatDrawEntry(leatherAmount.category, leatherAmount, this.InnerPawn.GetStatValue(leatherAmount, true), StatRequest.For(this.InnerPawn), ToStringNumberSense.Undefined, null, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x0007A93B File Offset: 0x00078B3B
		public void RotStageChanged()
		{
			PortraitsCache.SetDirty(this.InnerPawn);
			this.NotifyColonistBar();
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0007A950 File Offset: 0x00078B50
		private BodyPartRecord GetBestBodyPartToEat(Pawn ingester, float nutritionWanted)
		{
			IEnumerable<BodyPartRecord> source = from x in this.InnerPawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.depth == BodyPartDepth.Outside && FoodUtility.GetBodyPartNutrition(this, x) > 0.001f
			select x;
			if (!source.Any<BodyPartRecord>())
			{
				return null;
			}
			return source.MinBy((BodyPartRecord x) => Mathf.Abs(FoodUtility.GetBodyPartNutrition(this, x) - nutritionWanted));
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0007A9B8 File Offset: 0x00078BB8
		private void NotifyColonistBar()
		{
			if (this.InnerPawn.Faction == Faction.OfPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x04000DBF RID: 3519
		private ThingOwner<Pawn> innerContainer;

		// Token: 0x04000DC0 RID: 3520
		public int timeOfDeath = -1;

		// Token: 0x04000DC1 RID: 3521
		private int vanishAfterTimestamp = -1;

		// Token: 0x04000DC2 RID: 3522
		private BillStack operationsBillStack;

		// Token: 0x04000DC3 RID: 3523
		public bool everBuriedInSarcophagus;

		// Token: 0x04000DC4 RID: 3524
		private const int VanishAfterTicksSinceDessicated = 6000000;
	}
}
