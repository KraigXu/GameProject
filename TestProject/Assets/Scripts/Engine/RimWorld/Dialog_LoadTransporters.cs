using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000E56 RID: 3670
	public class Dialog_LoadTransporters : Window
	{
		// Token: 0x17000FF8 RID: 4088
		// (get) Token: 0x060058D3 RID: 22739 RVA: 0x001D9293 File Offset: 0x001D7493
		public bool CanChangeAssignedThingsAfterStarting
		{
			get
			{
				return this.transporters[0].Props.canChangeAssignedThingsAfterStarting;
			}
		}

		// Token: 0x17000FF9 RID: 4089
		// (get) Token: 0x060058D4 RID: 22740 RVA: 0x001D92AB File Offset: 0x001D74AB
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.transporters[0].LoadingInProgressOrReadyToLaunch;
			}
		}

		// Token: 0x17000FFA RID: 4090
		// (get) Token: 0x060058D5 RID: 22741 RVA: 0x001D66F9 File Offset: 0x001D48F9
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x17000FFB RID: 4091
		// (get) Token: 0x060058D6 RID: 22742 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000FFC RID: 4092
		// (get) Token: 0x060058D7 RID: 22743 RVA: 0x001D92C0 File Offset: 0x001D74C0
		private float MassCapacity
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.transporters.Count; i++)
				{
					num += this.transporters[i].Props.massCapacity;
				}
				return num;
			}
		}

		// Token: 0x17000FFD RID: 4093
		// (get) Token: 0x060058D8 RID: 22744 RVA: 0x001D9304 File Offset: 0x001D7504
		private float CaravanMassCapacity
		{
			get
			{
				if (this.caravanMassCapacityDirty)
				{
					this.caravanMassCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedCaravanMassCapacity = CollectionsMassCalculator.CapacityTransferables(this.transferables, stringBuilder);
					this.cachedCaravanMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedCaravanMassCapacity;
			}
		}

		// Token: 0x17000FFE RID: 4094
		// (get) Token: 0x060058D9 RID: 22745 RVA: 0x001D934C File Offset: 0x001D754C
		private string TransportersLabel
		{
			get
			{
				if (this.transporters[0].Props.max1PerGroup)
				{
					return this.transporters[0].parent.Label;
				}
				return Find.ActiveLanguageWorker.Pluralize(this.transporters[0].parent.Label, -1);
			}
		}

		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x060058DA RID: 22746 RVA: 0x001D93A9 File Offset: 0x001D75A9
		private string TransportersLabelCap
		{
			get
			{
				return this.TransportersLabel.CapitalizeFirst();
			}
		}

		// Token: 0x17001000 RID: 4096
		// (get) Token: 0x060058DB RID: 22747 RVA: 0x001D93B6 File Offset: 0x001D75B6
		private BiomeDef Biome
		{
			get
			{
				return this.map.Biome;
			}
		}

		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x060058DC RID: 22748 RVA: 0x001D93C3 File Offset: 0x001D75C3
		private float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					this.cachedMassUsage = CollectionsMassCalculator.MassUsageTransferables(this.transferables, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, false);
				}
				return this.cachedMassUsage;
			}
		}

		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x060058DD RID: 22749 RVA: 0x001D93EE File Offset: 0x001D75EE
		public float CaravanMassUsage
		{
			get
			{
				if (this.caravanMassUsageDirty)
				{
					this.caravanMassUsageDirty = false;
					this.cachedCaravanMassUsage = CollectionsMassCalculator.MassUsageTransferables(this.transferables, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, false, false);
				}
				return this.cachedCaravanMassUsage;
			}
		}

		// Token: 0x17001003 RID: 4099
		// (get) Token: 0x060058DE RID: 22750 RVA: 0x001D941C File Offset: 0x001D761C
		private float TilesPerDay
		{
			get
			{
				if (this.tilesPerDayDirty)
				{
					this.tilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.transferables, this.MassUsage, this.MassCapacity, this.map.Tile, -1, stringBuilder);
					this.cachedTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedTilesPerDay;
			}
		}

		// Token: 0x17001004 RID: 4100
		// (get) Token: 0x060058DF RID: 22751 RVA: 0x001D947C File Offset: 0x001D767C
		private Pair<float, float> DaysWorthOfFood
		{
			get
			{
				if (this.daysWorthOfFoodDirty)
				{
					this.daysWorthOfFoodDirty = false;
					float first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.map.Tile, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, Faction.OfPlayer, null, 0f, 3300);
					this.cachedDaysWorthOfFood = new Pair<float, float>(first, DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.map.Tile, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, null, 0f, 3300));
				}
				return this.cachedDaysWorthOfFood;
			}
		}

		// Token: 0x17001005 RID: 4101
		// (get) Token: 0x060058E0 RID: 22752 RVA: 0x001D94F4 File Offset: 0x001D76F4
		private Pair<ThingDef, float> ForagedFoodPerDay
		{
			get
			{
				if (this.foragedFoodPerDayDirty)
				{
					this.foragedFoodPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.transferables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedForagedFoodPerDay;
			}
		}

		// Token: 0x17001006 RID: 4102
		// (get) Token: 0x060058E1 RID: 22753 RVA: 0x001D9548 File Offset: 0x001D7748
		private float Visibility
		{
			get
			{
				if (this.visibilityDirty)
				{
					this.visibilityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedVisibility = CaravanVisibilityCalculator.Visibility(this.transferables, stringBuilder);
					this.cachedVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedVisibility;
			}
		}

		// Token: 0x060058E2 RID: 22754 RVA: 0x001D9590 File Offset: 0x001D7790
		public Dialog_LoadTransporters(Map map, List<CompTransporter> transporters)
		{
			this.map = map;
			this.transporters = new List<CompTransporter>();
			this.transporters.AddRange(transporters);
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x060058E3 RID: 22755 RVA: 0x001D9620 File Offset: 0x001D7820
		public override void PostOpen()
		{
			base.PostOpen();
			this.CalculateAndRecacheTransferables();
			if (this.CanChangeAssignedThingsAfterStarting && this.LoadingInProgressOrReadyToLaunch)
			{
				this.SetLoadedItemsToLoad();
			}
		}

		// Token: 0x060058E4 RID: 22756 RVA: 0x001D9644 File Offset: 0x001D7844
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 35f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "LoadTransporters".Translate(this.TransportersLabel));
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			if (this.transporters[0].Props.showOverallStats)
			{
				CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.MassUsage, this.MassCapacity, "", this.TilesPerDay, this.cachedTilesPerDayExplanation, this.DaysWorthOfFood, this.ForagedFoodPerDay, this.cachedForagedFoodPerDayExplanation, this.Visibility, this.cachedVisibilityExplanation, this.CaravanMassUsage, this.CaravanMassCapacity, this.cachedCaravanMassCapacityExplanation), null, this.map.Tile, null, this.lastMassFlashTime, new Rect(12f, 35f, inRect.width - 24f, 40f), false, null, false);
				inRect.yMin += 52f;
			}
			Dialog_LoadTransporters.tabsList.Clear();
			Dialog_LoadTransporters.tabsList.Add(new TabRecord("PawnsTab".Translate(), delegate
			{
				this.tab = Dialog_LoadTransporters.Tab.Pawns;
			}, this.tab == Dialog_LoadTransporters.Tab.Pawns));
			Dialog_LoadTransporters.tabsList.Add(new TabRecord("ItemsTab".Translate(), delegate
			{
				this.tab = Dialog_LoadTransporters.Tab.Items;
			}, this.tab == Dialog_LoadTransporters.Tab.Items));
			inRect.yMin += 67f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs(inRect, Dialog_LoadTransporters.tabsList, 200f);
			inRect = inRect.ContractedBy(17f);
			GUI.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect inRect2 = rect2;
			inRect2.yMax -= 59f;
			bool flag = false;
			Dialog_LoadTransporters.Tab tab = this.tab;
			if (tab != Dialog_LoadTransporters.Tab.Pawns)
			{
				if (tab == Dialog_LoadTransporters.Tab.Items)
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

		// Token: 0x060058E5 RID: 22757 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x060058E6 RID: 22758 RVA: 0x001D9880 File Offset: 0x001D7A80
		private void AddToTransferables(Thing t)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t, this.transferables, TransferAsOneMode.PodsOrCaravanPacking);
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

		// Token: 0x060058E7 RID: 22759 RVA: 0x001D98DC File Offset: 0x001D7ADC
		private void DoBottomButtons(Rect rect)
		{
			Rect rect2 = new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect2, "AcceptButton".Translate(), true, true, true))
			{
				if (this.CaravanMassUsage > this.CaravanMassCapacity && this.CaravanMassCapacity != 0f)
				{
					if (this.CheckForErrors(TransferableUtility.GetPawnsFromTransferables(this.transferables)))
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("TransportersCaravanWillBeImmobile".Translate(), delegate
						{
							if (this.TryAccept())
							{
								SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
								this.Close(false);
							}
						}, false, null));
					}
				}
				else if (this.TryAccept())
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.Close(false);
				}
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
			if (Prefs.DevMode)
			{
				float width = 200f;
				float num = this.BottomButtonSize.y / 2f;
				if (!this.LoadingInProgressOrReadyToLaunch && Widgets.ButtonText(new Rect(0f, rect.height - 55f, width, num), "Dev: Load instantly", true, true, true) && this.DebugTryLoadInstantly())
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.Close(false);
				}
				if (Widgets.ButtonText(new Rect(0f, rect.height - 55f + num, width, num), "Dev: Select everything", true, true, true))
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.SetToLoadEverything();
				}
			}
		}

		// Token: 0x060058E8 RID: 22760 RVA: 0x001D9B18 File Offset: 0x001D7D18
		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			if (this.CanChangeAssignedThingsAfterStarting && this.LoadingInProgressOrReadyToLaunch)
			{
				for (int i = 0; i < this.transporters.Count; i++)
				{
					for (int j = 0; j < this.transporters[i].innerContainer.Count; j++)
					{
						this.AddToTransferables(this.transporters[i].innerContainer[j]);
					}
				}
				foreach (Thing t in TransporterUtility.ThingsBeingHauledTo(this.transporters, this.map))
				{
					this.AddToTransferables(t);
				}
			}
			this.pawnsTransfer = new TransferableOneWayWidget(null, null, null, "FormCaravanColonyThingCountTip".Translate(), true, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, () => this.MassCapacity - this.MassUsage, 0f, false, this.map.Tile, true, true, true, false, true, false, false);
			CaravanUIUtility.AddPawnsSections(this.pawnsTransfer, this.transferables);
			this.itemsTransfer = new TransferableOneWayWidget(from x in this.transferables
			where x.ThingDef.category != ThingCategory.Pawn
			select x, null, null, "FormCaravanColonyThingCountTip".Translate(), true, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, () => this.MassCapacity - this.MassUsage, 0f, false, this.map.Tile, true, false, false, true, false, true, false);
			this.CountToTransferChanged();
		}

		// Token: 0x060058E9 RID: 22761 RVA: 0x001D9CBC File Offset: 0x001D7EBC
		private bool DebugTryLoadInstantly()
		{
			TransporterUtility.InitiateLoading(this.transporters);
			int i;
			int j;
			for (i = 0; i < this.transferables.Count; i = j + 1)
			{
				TransferableUtility.Transfer(this.transferables[i].things, this.transferables[i].CountToTransfer, delegate(Thing splitPiece, IThingHolder originalThing)
				{
					this.transporters[i % this.transporters.Count].GetDirectlyHeldThings().TryAdd(splitPiece, true);
				});
				j = i;
			}
			return true;
		}

		// Token: 0x060058EA RID: 22762 RVA: 0x001D9D4C File Offset: 0x001D7F4C
		private bool TryAccept()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!this.CheckForErrors(pawnsFromTransferables))
			{
				return false;
			}
			if (this.LoadingInProgressOrReadyToLaunch)
			{
				this.AssignTransferablesToRandomTransporters();
				TransporterUtility.MakeLordsAsAppropriate(pawnsFromTransferables, this.transporters, this.map);
				List<Pawn> allPawnsSpawned = this.map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToTransporter && this.transporters.Contains(((JobDriver_HaulToTransporter)allPawnsSpawned[i].jobs.curDriver).Transporter))
					{
						allPawnsSpawned[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
			else
			{
				TransporterUtility.InitiateLoading(this.transporters);
				this.AssignTransferablesToRandomTransporters();
				TransporterUtility.MakeLordsAsAppropriate(pawnsFromTransferables, this.transporters, this.map);
				if (this.transporters[0].Props.max1PerGroup)
				{
					Messages.Message("MessageTransporterSingleLoadingProcessStarted".Translate(), this.transporters[0].parent, MessageTypeDefOf.TaskCompletion, false);
				}
				else
				{
					Messages.Message("MessageTransportersLoadingProcessStarted".Translate(), this.transporters[0].parent, MessageTypeDefOf.TaskCompletion, false);
				}
			}
			return true;
		}

		// Token: 0x060058EB RID: 22763 RVA: 0x001D9EA8 File Offset: 0x001D80A8
		private void SetLoadedItemsToLoad()
		{
			int i;
			int num;
			for (i = 0; i < this.transporters.Count; i = num + 1)
			{
				int j;
				for (j = 0; j < this.transporters[i].innerContainer.Count; j = num + 1)
				{
					TransferableOneWay transferableOneWay = this.transferables.Find((TransferableOneWay x) => x.things.Contains(this.transporters[i].innerContainer[j]));
					if (transferableOneWay != null && transferableOneWay.CanAdjustBy(this.transporters[i].innerContainer[j].stackCount).Accepted)
					{
						transferableOneWay.AdjustBy(this.transporters[i].innerContainer[j].stackCount);
					}
					num = j;
				}
				if (this.transporters[i].leftToLoad != null)
				{
					for (int k = 0; k < this.transporters[i].leftToLoad.Count; k++)
					{
						TransferableOneWay transferableOneWay2 = this.transporters[i].leftToLoad[k];
						if (transferableOneWay2.CountToTransfer != 0 && transferableOneWay2.HasAnyThing)
						{
							TransferableOneWay transferableOneWay3 = TransferableUtility.TransferableMatchingDesperate(transferableOneWay2.AnyThing, this.transferables, TransferAsOneMode.PodsOrCaravanPacking);
							if (transferableOneWay3 != null && transferableOneWay3.CanAdjustBy(transferableOneWay2.CountToTransferToDestination).Accepted)
							{
								transferableOneWay3.AdjustBy(transferableOneWay2.CountToTransferToDestination);
							}
						}
					}
				}
				num = i;
			}
		}

		// Token: 0x060058EC RID: 22764 RVA: 0x001DA08C File Offset: 0x001D828C
		private void AssignTransferablesToRandomTransporters()
		{
			Dialog_LoadTransporters.tmpLeftToLoadCopy.Clear();
			for (int i3 = 0; i3 < this.transporters.Count; i3++)
			{
				Dialog_LoadTransporters.tmpLeftToLoadCopy.Add((this.transporters[i3].leftToLoad != null) ? this.transporters[i3].leftToLoad.ToList<TransferableOneWay>() : new List<TransferableOneWay>());
				if (this.transporters[i3].leftToLoad != null)
				{
					this.transporters[i3].leftToLoad.Clear();
				}
			}
			Dialog_LoadTransporters.tmpLeftCountToTransfer.Clear();
			for (int j = 0; j < this.transferables.Count; j++)
			{
				Dialog_LoadTransporters.tmpLeftCountToTransfer.Add(this.transferables[j], this.transferables[j].CountToTransfer);
			}
			if (this.LoadingInProgressOrReadyToLaunch)
			{
				int i2;
				int i;
				Func<Thing, bool> <>9__0;
				for (i = 0; i < this.transferables.Count; i = i2 + 1)
				{
					if (this.transferables[i].HasAnyThing && Dialog_LoadTransporters.tmpLeftCountToTransfer[this.transferables[i]] > 0)
					{
						for (int k = 0; k < Dialog_LoadTransporters.tmpLeftToLoadCopy.Count; k++)
						{
							TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(this.transferables[i].AnyThing, Dialog_LoadTransporters.tmpLeftToLoadCopy[k], TransferAsOneMode.PodsOrCaravanPacking);
							if (transferableOneWay != null)
							{
								int num = Mathf.Min(Dialog_LoadTransporters.tmpLeftCountToTransfer[this.transferables[i]], transferableOneWay.CountToTransfer);
								if (num > 0)
								{
									this.transporters[k].AddToTheToLoadList(this.transferables[i], num);
									Dictionary<TransferableOneWay, int> dictionary = Dialog_LoadTransporters.tmpLeftCountToTransfer;
									TransferableOneWay key = this.transferables[i];
									dictionary[key] -= num;
								}
							}
							IEnumerable<Thing> innerContainer = this.transporters[k].innerContainer;
							Func<Thing, bool> predicate;
							if ((predicate = <>9__0) == null)
							{
								predicate = (<>9__0 = ((Thing x) => TransferableUtility.TransferAsOne(this.transferables[i].AnyThing, x, TransferAsOneMode.PodsOrCaravanPacking)));
							}
							Thing thing = innerContainer.FirstOrDefault(predicate);
							if (thing != null)
							{
								int num2 = Mathf.Min(Dialog_LoadTransporters.tmpLeftCountToTransfer[this.transferables[i]], thing.stackCount);
								if (num2 > 0)
								{
									this.transporters[k].AddToTheToLoadList(this.transferables[i], num2);
									Dictionary<TransferableOneWay, int> dictionary = Dialog_LoadTransporters.tmpLeftCountToTransfer;
									TransferableOneWay key = this.transferables[i];
									dictionary[key] -= num2;
								}
							}
						}
					}
					i2 = i;
				}
			}
			Dialog_LoadTransporters.tmpLeftToLoadCopy.Clear();
			if (this.transferables.Any<TransferableOneWay>())
			{
				TransferableOneWay transferableOneWay2 = this.transferables.MaxBy((TransferableOneWay x) => Dialog_LoadTransporters.tmpLeftCountToTransfer[x]);
				int num3 = 0;
				for (int l = 0; l < this.transferables.Count; l++)
				{
					if (this.transferables[l] != transferableOneWay2 && Dialog_LoadTransporters.tmpLeftCountToTransfer[this.transferables[l]] > 0)
					{
						this.transporters[num3 % this.transporters.Count].AddToTheToLoadList(this.transferables[l], Dialog_LoadTransporters.tmpLeftCountToTransfer[this.transferables[l]]);
						num3++;
					}
				}
				if (num3 < this.transporters.Count)
				{
					int num4 = Dialog_LoadTransporters.tmpLeftCountToTransfer[transferableOneWay2];
					int num5 = num4 / (this.transporters.Count - num3);
					for (int m = num3; m < this.transporters.Count; m++)
					{
						int num6 = (m == this.transporters.Count - 1) ? num4 : num5;
						if (num6 > 0)
						{
							this.transporters[m].AddToTheToLoadList(transferableOneWay2, num6);
						}
						num4 -= num6;
					}
				}
				else
				{
					this.transporters[num3 % this.transporters.Count].AddToTheToLoadList(transferableOneWay2, Dialog_LoadTransporters.tmpLeftCountToTransfer[transferableOneWay2]);
				}
			}
			Dialog_LoadTransporters.tmpLeftCountToTransfer.Clear();
			for (int n = 0; n < this.transporters.Count; n++)
			{
				for (int num7 = 0; num7 < this.transporters[n].innerContainer.Count; num7++)
				{
					Thing thing2 = this.transporters[n].innerContainer[num7];
					int num8 = this.transporters[n].SubtractFromToLoadList(thing2, thing2.stackCount, false);
					if (num8 < thing2.stackCount)
					{
						Thing thing3;
						this.transporters[n].innerContainer.TryDrop(thing2, ThingPlaceMode.Near, thing2.stackCount - num8, out thing3, null, null);
					}
				}
			}
		}

		// Token: 0x060058ED RID: 22765 RVA: 0x001DA5C8 File Offset: 0x001D87C8
		private bool CheckForErrors(List<Pawn> pawns)
		{
			if (!this.CanChangeAssignedThingsAfterStarting)
			{
				if (!this.transferables.Any((TransferableOneWay x) => x.CountToTransfer != 0))
				{
					if (this.transporters[0].Props.max1PerGroup)
					{
						Messages.Message("CantSendEmptyTransporterSingle".Translate(), MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						Messages.Message("CantSendEmptyTransportPods".Translate(), MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
			}
			if (this.MassUsage > this.MassCapacity)
			{
				this.FlashMass();
				if (this.transporters[0].Props.max1PerGroup)
				{
					Messages.Message("TooBigTransporterSingleMassUsage".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("TooBigTransportersMassUsage".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			Pawn pawn = pawns.Find((Pawn x) => !x.MapHeld.reachability.CanReach(x.PositionHeld, this.transporters[0].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)) && !this.transporters.Any((CompTransporter y) => y.innerContainer.Contains(x)));
			if (pawn != null)
			{
				if (this.transporters[0].Props.max1PerGroup)
				{
					Messages.Message("PawnCantReachTransporterSingle".Translate(pawn.LabelShort, pawn).CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("PawnCantReachTransporters".Translate(pawn.LabelShort, pawn).CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			Map map = this.transporters[0].parent.Map;
			for (int i = 0; i < this.transferables.Count; i++)
			{
				if (this.transferables[i].ThingDef.category == ThingCategory.Item)
				{
					int countToTransfer = this.transferables[i].CountToTransfer;
					int num = 0;
					if (countToTransfer > 0)
					{
						for (int j = 0; j < this.transferables[i].things.Count; j++)
						{
							Thing t = this.transferables[i].things[j];
							Pawn_CarryTracker pawn_CarryTracker = t.ParentHolder as Pawn_CarryTracker;
							if (map.reachability.CanReach(t.Position, this.transporters[0].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)) || this.transporters.Any((CompTransporter x) => x.innerContainer.Contains(t)) || (pawn_CarryTracker != null && pawn_CarryTracker.pawn.MapHeld.reachability.CanReach(pawn_CarryTracker.pawn.PositionHeld, this.transporters[0].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false))))
							{
								num += t.stackCount;
								if (num >= countToTransfer)
								{
									break;
								}
							}
						}
						if (num < countToTransfer)
						{
							if (countToTransfer == 1)
							{
								if (this.transporters[0].Props.max1PerGroup)
								{
									Messages.Message("TransporterSingleItemIsUnreachableSingle".Translate(this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
								}
								else
								{
									Messages.Message("TransporterItemIsUnreachableSingle".Translate(this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
								}
							}
							else if (this.transporters[0].Props.max1PerGroup)
							{
								Messages.Message("TransporterSingleItemIsUnreachableMulti".Translate(countToTransfer, this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
							}
							else
							{
								Messages.Message("TransporterItemIsUnreachableMulti".Translate(countToTransfer, this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
							}
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060058EE RID: 22766 RVA: 0x001DAA0C File Offset: 0x001D8C0C
		private void AddPawnsToTransferables()
		{
			foreach (Pawn t in TransporterUtility.AllSendablePawns(this.transporters, this.map))
			{
				this.AddToTransferables(t);
			}
		}

		// Token: 0x060058EF RID: 22767 RVA: 0x001DAA64 File Offset: 0x001D8C64
		private void AddItemsToTransferables()
		{
			foreach (Thing t in TransporterUtility.AllSendableItems(this.transporters, this.map))
			{
				this.AddToTransferables(t);
			}
		}

		// Token: 0x060058F0 RID: 22768 RVA: 0x001DAABC File Offset: 0x001D8CBC
		private void FlashMass()
		{
			this.lastMassFlashTime = Time.time;
		}

		// Token: 0x060058F1 RID: 22769 RVA: 0x001DAACC File Offset: 0x001D8CCC
		private void SetToLoadEverything()
		{
			for (int i = 0; i < this.transferables.Count; i++)
			{
				this.transferables[i].AdjustTo(this.transferables[i].GetMaximumToTransfer());
			}
			this.CountToTransferChanged();
		}

		// Token: 0x060058F2 RID: 22770 RVA: 0x001DAB17 File Offset: 0x001D8D17
		private void CountToTransferChanged()
		{
			this.massUsageDirty = true;
			this.caravanMassUsageDirty = true;
			this.caravanMassCapacityDirty = true;
			this.tilesPerDayDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.foragedFoodPerDayDirty = true;
			this.visibilityDirty = true;
		}

		// Token: 0x04003006 RID: 12294
		private Map map;

		// Token: 0x04003007 RID: 12295
		private List<CompTransporter> transporters;

		// Token: 0x04003008 RID: 12296
		private List<TransferableOneWay> transferables;

		// Token: 0x04003009 RID: 12297
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x0400300A RID: 12298
		private TransferableOneWayWidget itemsTransfer;

		// Token: 0x0400300B RID: 12299
		private Dialog_LoadTransporters.Tab tab;

		// Token: 0x0400300C RID: 12300
		private float lastMassFlashTime = -9999f;

		// Token: 0x0400300D RID: 12301
		private bool massUsageDirty = true;

		// Token: 0x0400300E RID: 12302
		private float cachedMassUsage;

		// Token: 0x0400300F RID: 12303
		private bool caravanMassUsageDirty = true;

		// Token: 0x04003010 RID: 12304
		private float cachedCaravanMassUsage;

		// Token: 0x04003011 RID: 12305
		private bool caravanMassCapacityDirty = true;

		// Token: 0x04003012 RID: 12306
		private float cachedCaravanMassCapacity;

		// Token: 0x04003013 RID: 12307
		private string cachedCaravanMassCapacityExplanation;

		// Token: 0x04003014 RID: 12308
		private bool tilesPerDayDirty = true;

		// Token: 0x04003015 RID: 12309
		private float cachedTilesPerDay;

		// Token: 0x04003016 RID: 12310
		private string cachedTilesPerDayExplanation;

		// Token: 0x04003017 RID: 12311
		private bool daysWorthOfFoodDirty = true;

		// Token: 0x04003018 RID: 12312
		private Pair<float, float> cachedDaysWorthOfFood;

		// Token: 0x04003019 RID: 12313
		private bool foragedFoodPerDayDirty = true;

		// Token: 0x0400301A RID: 12314
		private Pair<ThingDef, float> cachedForagedFoodPerDay;

		// Token: 0x0400301B RID: 12315
		private string cachedForagedFoodPerDayExplanation;

		// Token: 0x0400301C RID: 12316
		private bool visibilityDirty = true;

		// Token: 0x0400301D RID: 12317
		private float cachedVisibility;

		// Token: 0x0400301E RID: 12318
		private string cachedVisibilityExplanation;

		// Token: 0x0400301F RID: 12319
		private const float TitleRectHeight = 35f;

		// Token: 0x04003020 RID: 12320
		private const float BottomAreaHeight = 55f;

		// Token: 0x04003021 RID: 12321
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04003022 RID: 12322
		private static List<TabRecord> tabsList = new List<TabRecord>();

		// Token: 0x04003023 RID: 12323
		private static List<List<TransferableOneWay>> tmpLeftToLoadCopy = new List<List<TransferableOneWay>>();

		// Token: 0x04003024 RID: 12324
		private static Dictionary<TransferableOneWay, int> tmpLeftCountToTransfer = new Dictionary<TransferableOneWay, int>();

		// Token: 0x02001D16 RID: 7446
		private enum Tab
		{
			// Token: 0x04006E35 RID: 28213
			Pawns,
			// Token: 0x04006E36 RID: 28214
			Items
		}
	}
}
