using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x0200128B RID: 4747
	public class Dialog_SplitCaravan : Window
	{
		// Token: 0x170012C2 RID: 4802
		// (get) Token: 0x06006F91 RID: 28561 RVA: 0x001D66F9 File Offset: 0x001D48F9
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x170012C3 RID: 4803
		// (get) Token: 0x06006F92 RID: 28562 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170012C4 RID: 4804
		// (get) Token: 0x06006F93 RID: 28563 RVA: 0x0026D565 File Offset: 0x0026B765
		private BiomeDef Biome
		{
			get
			{
				return this.caravan.Biome;
			}
		}

		// Token: 0x170012C5 RID: 4805
		// (get) Token: 0x06006F94 RID: 28564 RVA: 0x0026D572 File Offset: 0x0026B772
		private float SourceMassUsage
		{
			get
			{
				if (this.sourceMassUsageDirty)
				{
					this.sourceMassUsageDirty = false;
					this.cachedSourceMassUsage = CollectionsMassCalculator.MassUsageLeftAfterTransfer(this.transferables, IgnorePawnsInventoryMode.Ignore, false, false);
				}
				return this.cachedSourceMassUsage;
			}
		}

		// Token: 0x170012C6 RID: 4806
		// (get) Token: 0x06006F95 RID: 28565 RVA: 0x0026D5A0 File Offset: 0x0026B7A0
		private float SourceMassCapacity
		{
			get
			{
				if (this.sourceMassCapacityDirty)
				{
					this.sourceMassCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceMassCapacity = CollectionsMassCalculator.CapacityLeftAfterTransfer(this.transferables, stringBuilder);
					this.cachedSourceMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceMassCapacity;
			}
		}

		// Token: 0x170012C7 RID: 4807
		// (get) Token: 0x06006F96 RID: 28566 RVA: 0x0026D5E8 File Offset: 0x0026B7E8
		private float SourceTilesPerDay
		{
			get
			{
				if (this.sourceTilesPerDayDirty)
				{
					this.sourceTilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDayLeftAfterTransfer(this.transferables, this.SourceMassUsage, this.SourceMassCapacity, this.caravan.Tile, this.caravan.pather.Moving ? this.caravan.pather.nextTile : -1, stringBuilder);
					this.cachedSourceTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceTilesPerDay;
			}
		}

		// Token: 0x170012C8 RID: 4808
		// (get) Token: 0x06006F97 RID: 28567 RVA: 0x0026D66C File Offset: 0x0026B86C
		private Pair<float, float> SourceDaysWorthOfFood
		{
			get
			{
				if (this.sourceDaysWorthOfFoodDirty)
				{
					this.sourceDaysWorthOfFoodDirty = false;
					float first;
					float second;
					if (this.caravan.pather.Moving)
					{
						using (Find.WorldPathFinder.FindPath(this.caravan.Tile, this.caravan.pather.Destination, null, null))
						{
							first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFoodLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
							second = DaysUntilRotCalculator.ApproxDaysUntilRotLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
							goto IL_13D;
						}
					}
					first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFoodLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, null, 0f, 3300);
					second = DaysUntilRotCalculator.ApproxDaysUntilRotLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, null, 0f, 3300);
					IL_13D:
					this.cachedSourceDaysWorthOfFood = new Pair<float, float>(first, second);
				}
				return this.cachedSourceDaysWorthOfFood;
			}
		}

		// Token: 0x170012C9 RID: 4809
		// (get) Token: 0x06006F98 RID: 28568 RVA: 0x0026D7DC File Offset: 0x0026B9DC
		private Pair<ThingDef, float> SourceForagedFoodPerDay
		{
			get
			{
				if (this.sourceForagedFoodPerDayDirty)
				{
					this.sourceForagedFoodPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDayLeftAfterTransfer(this.transferables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedSourceForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceForagedFoodPerDay;
			}
		}

		// Token: 0x170012CA RID: 4810
		// (get) Token: 0x06006F99 RID: 28569 RVA: 0x0026D830 File Offset: 0x0026BA30
		private float SourceVisibility
		{
			get
			{
				if (this.sourceVisibilityDirty)
				{
					this.sourceVisibilityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceVisibility = CaravanVisibilityCalculator.VisibilityLeftAfterTransfer(this.transferables, stringBuilder);
					this.cachedSourceVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceVisibility;
			}
		}

		// Token: 0x170012CB RID: 4811
		// (get) Token: 0x06006F9A RID: 28570 RVA: 0x0026D876 File Offset: 0x0026BA76
		private float DestMassUsage
		{
			get
			{
				if (this.destMassUsageDirty)
				{
					this.destMassUsageDirty = false;
					this.cachedDestMassUsage = CollectionsMassCalculator.MassUsageTransferables(this.transferables, IgnorePawnsInventoryMode.Ignore, false, false);
				}
				return this.cachedDestMassUsage;
			}
		}

		// Token: 0x170012CC RID: 4812
		// (get) Token: 0x06006F9B RID: 28571 RVA: 0x0026D8A4 File Offset: 0x0026BAA4
		private float DestMassCapacity
		{
			get
			{
				if (this.destMassCapacityDirty)
				{
					this.destMassCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestMassCapacity = CollectionsMassCalculator.CapacityTransferables(this.transferables, stringBuilder);
					this.cachedDestMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedDestMassCapacity;
			}
		}

		// Token: 0x170012CD RID: 4813
		// (get) Token: 0x06006F9C RID: 28572 RVA: 0x0026D8EC File Offset: 0x0026BAEC
		private float DestTilesPerDay
		{
			get
			{
				if (this.destTilesPerDayDirty)
				{
					this.destTilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.transferables, this.DestMassUsage, this.DestMassCapacity, this.caravan.Tile, this.caravan.pather.Moving ? this.caravan.pather.nextTile : -1, stringBuilder);
					this.cachedDestTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedDestTilesPerDay;
			}
		}

		// Token: 0x170012CE RID: 4814
		// (get) Token: 0x06006F9D RID: 28573 RVA: 0x0026D970 File Offset: 0x0026BB70
		private Pair<float, float> DestDaysWorthOfFood
		{
			get
			{
				if (this.destDaysWorthOfFoodDirty)
				{
					this.destDaysWorthOfFoodDirty = false;
					float first;
					float second;
					if (this.caravan.pather.Moving)
					{
						first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
						second = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
					}
					else
					{
						first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, null, 0f, 3300);
						second = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, null, 0f, 3300);
					}
					this.cachedDestDaysWorthOfFood = new Pair<float, float>(first, second);
				}
				return this.cachedDestDaysWorthOfFood;
			}
		}

		// Token: 0x170012CF RID: 4815
		// (get) Token: 0x06006F9E RID: 28574 RVA: 0x0026DA9C File Offset: 0x0026BC9C
		private Pair<ThingDef, float> DestForagedFoodPerDay
		{
			get
			{
				if (this.destForagedFoodPerDayDirty)
				{
					this.destForagedFoodPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.transferables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedDestForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedDestForagedFoodPerDay;
			}
		}

		// Token: 0x170012D0 RID: 4816
		// (get) Token: 0x06006F9F RID: 28575 RVA: 0x0026DAF0 File Offset: 0x0026BCF0
		private float DestVisibility
		{
			get
			{
				if (this.destVisibilityDirty)
				{
					this.destVisibilityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestVisibility = CaravanVisibilityCalculator.Visibility(this.transferables, stringBuilder);
					this.cachedDestVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedDestVisibility;
			}
		}

		// Token: 0x170012D1 RID: 4817
		// (get) Token: 0x06006FA0 RID: 28576 RVA: 0x0026DB36 File Offset: 0x0026BD36
		private int TicksToArrive
		{
			get
			{
				if (!this.caravan.pather.Moving)
				{
					return 0;
				}
				if (this.ticksToArriveDirty)
				{
					this.ticksToArriveDirty = false;
					this.cachedTicksToArrive = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.caravan, false);
				}
				return this.cachedTicksToArrive;
			}
		}

		// Token: 0x06006FA1 RID: 28577 RVA: 0x0026DB74 File Offset: 0x0026BD74
		public Dialog_SplitCaravan(Caravan caravan)
		{
			this.caravan = caravan;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06006FA2 RID: 28578 RVA: 0x0026DC0C File Offset: 0x0026BE0C
		public override void PostOpen()
		{
			base.PostOpen();
			this.CalculateAndRecacheTransferables();
		}

		// Token: 0x06006FA3 RID: 28579 RVA: 0x0026DC1C File Offset: 0x0026BE1C
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 35f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "SplitCaravan".Translate());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.SourceMassUsage, this.SourceMassCapacity, this.cachedSourceMassCapacityExplanation, this.SourceTilesPerDay, this.cachedSourceTilesPerDayExplanation, this.SourceDaysWorthOfFood, this.SourceForagedFoodPerDay, this.cachedSourceForagedFoodPerDayExplanation, this.SourceVisibility, this.cachedSourceVisibilityExplanation, -1f, -1f, null), new CaravanUIUtility.CaravanInfo?(new CaravanUIUtility.CaravanInfo(this.DestMassUsage, this.DestMassCapacity, this.cachedDestMassCapacityExplanation, this.DestTilesPerDay, this.cachedDestTilesPerDayExplanation, this.DestDaysWorthOfFood, this.DestForagedFoodPerDay, this.cachedDestForagedFoodPerDayExplanation, this.DestVisibility, this.cachedDestVisibilityExplanation, -1f, -1f, null)), this.caravan.Tile, this.caravan.pather.Moving ? new int?(this.TicksToArrive) : null, -9999f, new Rect(12f, 35f, inRect.width - 24f, 40f), true, null, false);
			Dialog_SplitCaravan.tabsList.Clear();
			Dialog_SplitCaravan.tabsList.Add(new TabRecord("PawnsTab".Translate(), delegate
			{
				this.tab = Dialog_SplitCaravan.Tab.Pawns;
			}, this.tab == Dialog_SplitCaravan.Tab.Pawns));
			Dialog_SplitCaravan.tabsList.Add(new TabRecord("ItemsTab".Translate(), delegate
			{
				this.tab = Dialog_SplitCaravan.Tab.Items;
			}, this.tab == Dialog_SplitCaravan.Tab.Items));
			inRect.yMin += 119f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs(inRect, Dialog_SplitCaravan.tabsList, 200f);
			inRect = inRect.ContractedBy(17f);
			GUI.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect inRect2 = rect2;
			inRect2.yMax -= 59f;
			bool flag = false;
			Dialog_SplitCaravan.Tab tab = this.tab;
			if (tab != Dialog_SplitCaravan.Tab.Pawns)
			{
				if (tab == Dialog_SplitCaravan.Tab.Items)
				{
					this.itemsTransfer.OnGUI(inRect2, out flag);
				}
			}
			else
			{
				this.pawnsTransfer.OnGUI(inRect2, out flag);
			}
			if (flag)
			{
				this.CountToTransferChanged();
			}
			GUI.EndGroup();
		}

		// Token: 0x06006FA4 RID: 28580 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x06006FA5 RID: 28581 RVA: 0x0026DE7C File Offset: 0x0026C07C
		private void AddToTransferables(Thing t)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t, this.transferables, TransferAsOneMode.Normal);
			if (transferableOneWay == null)
			{
				transferableOneWay = new TransferableOneWay();
				this.transferables.Add(transferableOneWay);
			}
			if (transferableOneWay.things.Contains(t))
			{
				Log.Error("Tried to add the same thing twice to TransferableOneWay: " + t, false);
				return;
			}
			transferableOneWay.things.Add(t);
		}

		// Token: 0x06006FA6 RID: 28582 RVA: 0x0026DED8 File Offset: 0x0026C0D8
		private void DoBottomButtons(Rect rect)
		{
			Rect rect2 = new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect2, "AcceptButton".Translate(), true, true, true) && this.TrySplitCaravan())
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				this.Close(false);
			}
			if (Widgets.ButtonText(new Rect(rect2.x - 10f - this.BottomButtonSize.x, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y), "ResetButton".Translate(), true, true, true))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				this.CalculateAndRecacheTransferables();
			}
			if (Widgets.ButtonText(new Rect(rect2.xMax + 10f, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y), "CancelButton".Translate(), true, true, true))
			{
				this.Close(true);
			}
		}

		// Token: 0x06006FA7 RID: 28583 RVA: 0x0026E018 File Offset: 0x0026C218
		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			CaravanUIUtility.CreateCaravanTransferableWidgets(this.transferables, out this.pawnsTransfer, out this.itemsTransfer, "SplitCaravanThingCountTip".Translate(), IgnorePawnsInventoryMode.Ignore, () => this.DestMassCapacity - this.DestMassUsage, false, this.caravan.Tile, false);
			this.CountToTransferChanged();
		}

		// Token: 0x06006FA8 RID: 28584 RVA: 0x0026E084 File Offset: 0x0026C284
		private bool TrySplitCaravan()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!this.CheckForErrors(pawnsFromTransferables))
			{
				return false;
			}
			for (int i = 0; i < pawnsFromTransferables.Count; i++)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawnsFromTransferables[i], this.caravan.PawnsListForReading, pawnsFromTransferables);
			}
			for (int j = 0; j < pawnsFromTransferables.Count; j++)
			{
				this.caravan.RemovePawn(pawnsFromTransferables[j]);
			}
			Caravan newCaravan = CaravanMaker.MakeCaravan(pawnsFromTransferables, this.caravan.Faction, this.caravan.Tile, true);
			this.transferables.RemoveAll((TransferableOneWay x) => x.AnyThing is Pawn);
			Action<Thing, int> <>9__1;
			for (int k = 0; k < this.transferables.Count; k++)
			{
				List<Thing> things = this.transferables[k].things;
				int countToTransfer = this.transferables[k].CountToTransfer;
				Action<Thing, int> transfer;
				if ((transfer = <>9__1) == null)
				{
					transfer = (<>9__1 = delegate(Thing thing, int numToTake)
					{
						Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(this.caravan, thing);
						if (ownerOf == null)
						{
							Log.Error("Error while splitting a caravan: Thing " + thing + " has no owner. Where did it come from then?", false);
							return;
						}
						CaravanInventoryUtility.MoveInventoryToSomeoneElse(ownerOf, thing, newCaravan.PawnsListForReading, null, numToTake);
					});
				}
				TransferableUtility.TransferNoSplit(things, countToTransfer, transfer, true, true);
			}
			return true;
		}

		// Token: 0x06006FA9 RID: 28585 RVA: 0x0026E1B8 File Offset: 0x0026C3B8
		private bool CheckForErrors(List<Pawn> pawns)
		{
			if (!pawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed))
			{
				Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), this.caravan, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (!this.AnyNonDownedColonistLeftInSourceCaravan(pawns))
			{
				Messages.Message("SourceCaravanMustHaveAtLeastOneColonist".Translate(), this.caravan, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			return true;
		}

		// Token: 0x06006FAA RID: 28586 RVA: 0x0026E244 File Offset: 0x0026C444
		private void AddPawnsToTransferables()
		{
			List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				this.AddToTransferables(pawnsListForReading[i]);
			}
		}

		// Token: 0x06006FAB RID: 28587 RVA: 0x0026E27C File Offset: 0x0026C47C
		private void AddItemsToTransferables()
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this.caravan);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddToTransferables(list[i]);
			}
		}

		// Token: 0x06006FAC RID: 28588 RVA: 0x0026E2B4 File Offset: 0x0026C4B4
		private bool AnyNonDownedColonistLeftInSourceCaravan(List<Pawn> pawnsToTransfer)
		{
			Predicate<Thing> <>9__1;
			return this.transferables.Any(delegate(TransferableOneWay x)
			{
				List<Thing> things = x.things;
				Predicate<Thing> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = delegate(Thing y)
					{
						Pawn pawn = y as Pawn;
						return pawn != null && CaravanUtility.IsOwner(pawn, Faction.OfPlayer) && !pawn.Downed && !pawnsToTransfer.Contains(pawn);
					});
				}
				return things.Any(predicate);
			});
		}

		// Token: 0x06006FAD RID: 28589 RVA: 0x0026E2E8 File Offset: 0x0026C4E8
		private void CountToTransferChanged()
		{
			this.sourceMassUsageDirty = true;
			this.sourceMassCapacityDirty = true;
			this.sourceTilesPerDayDirty = true;
			this.sourceDaysWorthOfFoodDirty = true;
			this.sourceForagedFoodPerDayDirty = true;
			this.sourceVisibilityDirty = true;
			this.destMassUsageDirty = true;
			this.destMassCapacityDirty = true;
			this.destTilesPerDayDirty = true;
			this.destDaysWorthOfFoodDirty = true;
			this.destForagedFoodPerDayDirty = true;
			this.destVisibilityDirty = true;
			this.ticksToArriveDirty = true;
		}

		// Token: 0x04004488 RID: 17544
		private Caravan caravan;

		// Token: 0x04004489 RID: 17545
		private List<TransferableOneWay> transferables;

		// Token: 0x0400448A RID: 17546
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x0400448B RID: 17547
		private TransferableOneWayWidget itemsTransfer;

		// Token: 0x0400448C RID: 17548
		private Dialog_SplitCaravan.Tab tab;

		// Token: 0x0400448D RID: 17549
		private bool sourceMassUsageDirty = true;

		// Token: 0x0400448E RID: 17550
		private float cachedSourceMassUsage;

		// Token: 0x0400448F RID: 17551
		private bool sourceMassCapacityDirty = true;

		// Token: 0x04004490 RID: 17552
		private float cachedSourceMassCapacity;

		// Token: 0x04004491 RID: 17553
		private string cachedSourceMassCapacityExplanation;

		// Token: 0x04004492 RID: 17554
		private bool sourceTilesPerDayDirty = true;

		// Token: 0x04004493 RID: 17555
		private float cachedSourceTilesPerDay;

		// Token: 0x04004494 RID: 17556
		private string cachedSourceTilesPerDayExplanation;

		// Token: 0x04004495 RID: 17557
		private bool sourceDaysWorthOfFoodDirty = true;

		// Token: 0x04004496 RID: 17558
		private Pair<float, float> cachedSourceDaysWorthOfFood;

		// Token: 0x04004497 RID: 17559
		private bool sourceForagedFoodPerDayDirty = true;

		// Token: 0x04004498 RID: 17560
		private Pair<ThingDef, float> cachedSourceForagedFoodPerDay;

		// Token: 0x04004499 RID: 17561
		private string cachedSourceForagedFoodPerDayExplanation;

		// Token: 0x0400449A RID: 17562
		private bool sourceVisibilityDirty = true;

		// Token: 0x0400449B RID: 17563
		private float cachedSourceVisibility;

		// Token: 0x0400449C RID: 17564
		private string cachedSourceVisibilityExplanation;

		// Token: 0x0400449D RID: 17565
		private bool destMassUsageDirty = true;

		// Token: 0x0400449E RID: 17566
		private float cachedDestMassUsage;

		// Token: 0x0400449F RID: 17567
		private bool destMassCapacityDirty = true;

		// Token: 0x040044A0 RID: 17568
		private float cachedDestMassCapacity;

		// Token: 0x040044A1 RID: 17569
		private string cachedDestMassCapacityExplanation;

		// Token: 0x040044A2 RID: 17570
		private bool destTilesPerDayDirty = true;

		// Token: 0x040044A3 RID: 17571
		private float cachedDestTilesPerDay;

		// Token: 0x040044A4 RID: 17572
		private string cachedDestTilesPerDayExplanation;

		// Token: 0x040044A5 RID: 17573
		private bool destDaysWorthOfFoodDirty = true;

		// Token: 0x040044A6 RID: 17574
		private Pair<float, float> cachedDestDaysWorthOfFood;

		// Token: 0x040044A7 RID: 17575
		private bool destForagedFoodPerDayDirty = true;

		// Token: 0x040044A8 RID: 17576
		private Pair<ThingDef, float> cachedDestForagedFoodPerDay;

		// Token: 0x040044A9 RID: 17577
		private string cachedDestForagedFoodPerDayExplanation;

		// Token: 0x040044AA RID: 17578
		private bool destVisibilityDirty = true;

		// Token: 0x040044AB RID: 17579
		private float cachedDestVisibility;

		// Token: 0x040044AC RID: 17580
		private string cachedDestVisibilityExplanation;

		// Token: 0x040044AD RID: 17581
		private bool ticksToArriveDirty = true;

		// Token: 0x040044AE RID: 17582
		private int cachedTicksToArrive;

		// Token: 0x040044AF RID: 17583
		private const float TitleRectHeight = 35f;

		// Token: 0x040044B0 RID: 17584
		private const float BottomAreaHeight = 55f;

		// Token: 0x040044B1 RID: 17585
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x040044B2 RID: 17586
		private static List<TabRecord> tabsList = new List<TabRecord>();

		// Token: 0x0200203C RID: 8252
		private enum Tab
		{
			// Token: 0x040078E4 RID: 30948
			Pawns,
			// Token: 0x040078E5 RID: 30949
			Items
		}
	}
}
