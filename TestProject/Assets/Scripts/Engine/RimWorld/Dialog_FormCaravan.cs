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
	
	public class Dialog_FormCaravan : Window
	{
		
		// (get) Token: 0x0600588D RID: 22669 RVA: 0x001D66EC File Offset: 0x001D48EC
		public int CurrentTile
		{
			get
			{
				return this.map.Tile;
			}
		}

		
		// (get) Token: 0x0600588E RID: 22670 RVA: 0x001D66F9 File Offset: 0x001D48F9
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		
		// (get) Token: 0x0600588F RID: 22671 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		
		// (get) Token: 0x06005890 RID: 22672 RVA: 0x001D670B File Offset: 0x001D490B
		private bool AutoStripSpawnedCorpses
		{
			get
			{
				return this.reform;
			}
		}

		
		// (get) Token: 0x06005891 RID: 22673 RVA: 0x001D670B File Offset: 0x001D490B
		private bool ListPlayerPawnsInventorySeparately
		{
			get
			{
				return this.reform;
			}
		}

		
		// (get) Token: 0x06005892 RID: 22674 RVA: 0x001D6713 File Offset: 0x001D4913
		private BiomeDef Biome
		{
			get
			{
				return this.map.Biome;
			}
		}

		
		// (get) Token: 0x06005893 RID: 22675 RVA: 0x001D6720 File Offset: 0x001D4920
		private bool MustChooseRoute
		{
			get
			{
				return this.canChooseRoute && (!this.reform || this.map.Parent is Settlement);
			}
		}

		
		// (get) Token: 0x06005894 RID: 22676 RVA: 0x001D674C File Offset: 0x001D494C
		private bool ShowCancelButton
		{
			get
			{
				if (!this.mapAboutToBeRemoved)
				{
					return true;
				}
				bool flag = false;
				for (int i = 0; i < this.transferables.Count; i++)
				{
					Pawn pawn = this.transferables[i].AnyThing as Pawn;
					if (pawn != null && pawn.IsColonist && !pawn.Downed)
					{
						flag = true;
						break;
					}
				}
				return !flag;
			}
		}

		
		// (get) Token: 0x06005895 RID: 22677 RVA: 0x001D67AF File Offset: 0x001D49AF
		private IgnorePawnsInventoryMode IgnoreInventoryMode
		{
			get
			{
				if (!this.ListPlayerPawnsInventorySeparately)
				{
					return IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload;
				}
				return IgnorePawnsInventoryMode.IgnoreIfAssignedToUnloadOrPlayerPawn;
			}
		}

		
		// (get) Token: 0x06005896 RID: 22678 RVA: 0x001D67BC File Offset: 0x001D49BC
		public float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					this.cachedMassUsage = CollectionsMassCalculator.MassUsageTransferables(this.transferables, this.IgnoreInventoryMode, false, this.AutoStripSpawnedCorpses);
				}
				return this.cachedMassUsage;
			}
		}

		
		// (get) Token: 0x06005897 RID: 22679 RVA: 0x001D67F4 File Offset: 0x001D49F4
		public float MassCapacity
		{
			get
			{
				if (this.massCapacityDirty)
				{
					this.massCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedMassCapacity = CollectionsMassCalculator.CapacityTransferables(this.transferables, stringBuilder);
					this.cachedMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedMassCapacity;
			}
		}

		
		// (get) Token: 0x06005898 RID: 22680 RVA: 0x001D683C File Offset: 0x001D4A3C
		private float TilesPerDay
		{
			get
			{
				if (this.tilesPerDayDirty)
				{
					this.tilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.transferables, this.MassUsage, this.MassCapacity, this.CurrentTile, this.startingTile, stringBuilder);
					this.cachedTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedTilesPerDay;
			}
		}

		
		// (get) Token: 0x06005899 RID: 22681 RVA: 0x001D689C File Offset: 0x001D4A9C
		private Pair<float, float> DaysWorthOfFood
		{
			get
			{
				if (this.daysWorthOfFoodDirty)
				{
					this.daysWorthOfFoodDirty = false;
					float first;
					float second;
					if (this.destinationTile != -1)
					{
						using (WorldPath worldPath = Find.WorldPathFinder.FindPath(this.CurrentTile, this.destinationTile, null, null))
						{
							int ticksPerMove = CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(this), null);
							first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, Faction.OfPlayer, worldPath, 0f, ticksPerMove);
							second = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, worldPath, 0f, ticksPerMove);
							goto IL_DB;
						}
					}
					first = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, Faction.OfPlayer, null, 0f, 3300);
					second = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, null, 0f, 3300);
					IL_DB:
					this.cachedDaysWorthOfFood = new Pair<float, float>(first, second);
				}
				return this.cachedDaysWorthOfFood;
			}
		}

		
		// (get) Token: 0x0600589A RID: 22682 RVA: 0x001D69A8 File Offset: 0x001D4BA8
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

		
		// (get) Token: 0x0600589B RID: 22683 RVA: 0x001D69FC File Offset: 0x001D4BFC
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

		
		// (get) Token: 0x0600589C RID: 22684 RVA: 0x001D6A44 File Offset: 0x001D4C44
		private int TicksToArrive
		{
			get
			{
				if (this.destinationTile == -1)
				{
					return 0;
				}
				if (this.ticksToArriveDirty)
				{
					this.ticksToArriveDirty = false;
					using (WorldPath worldPath = Find.WorldPathFinder.FindPath(this.CurrentTile, this.destinationTile, null, null))
					{
						this.cachedTicksToArrive = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.CurrentTile, this.destinationTile, worldPath, 0f, CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(this), null), Find.TickManager.TicksAbs);
					}
				}
				return this.cachedTicksToArrive;
			}
		}

		
		// (get) Token: 0x0600589D RID: 22685 RVA: 0x001D6ADC File Offset: 0x001D4CDC
		private bool MostFoodWillRotSoon
		{
			get
			{
				float num = 0f;
				float num2 = 0f;
				for (int i = 0; i < this.transferables.Count; i++)
				{
					TransferableOneWay transferableOneWay = this.transferables[i];
					if (transferableOneWay.HasAnyThing && transferableOneWay.CountToTransfer > 0 && transferableOneWay.ThingDef.IsNutritionGivingIngestible && !(transferableOneWay.AnyThing is Corpse))
					{
						float num3 = 600f;
						CompRottable compRottable = transferableOneWay.AnyThing.TryGetComp<CompRottable>();
						if (compRottable != null)
						{
							num3 = (float)DaysUntilRotCalculator.ApproxTicksUntilRot_AssumeTimePassesBy(compRottable, this.CurrentTile, null) / 60000f;
						}
						float num4 = transferableOneWay.ThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null) * (float)transferableOneWay.CountToTransfer;
						if (num3 < 5f)
						{
							num += num4;
						}
						else
						{
							num2 += num4;
						}
					}
				}
				return (num != 0f || num2 != 0f) && num / (num + num2) >= 0.75f;
			}
		}

		
		public Dialog_FormCaravan(Map map, bool reform = false, Action onClosed = null, bool mapAboutToBeRemoved = false)
		{
			this.map = map;
			this.reform = reform;
			this.onClosed = onClosed;
			this.mapAboutToBeRemoved = mapAboutToBeRemoved;
			this.canChooseRoute = (!reform || !map.retainedCaravanData.HasDestinationTile);
			this.closeOnAccept = !reform;
			this.closeOnCancel = !reform;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		
		public override void PostOpen()
		{
			base.PostOpen();
			this.choosingRoute = false;
			if (!this.thisWindowInstanceEverOpened)
			{
				this.thisWindowInstanceEverOpened = true;
				this.CalculateAndRecacheTransferables();
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.FormCaravan, KnowledgeAmount.Total);
			}
		}

		
		public override void PostClose()
		{
			base.PostClose();
			if (this.onClosed != null && !this.choosingRoute)
			{
				this.onClosed();
			}
		}

		
		public void Notify_NoLongerChoosingRoute()
		{
			this.choosingRoute = false;
			if (!Find.WindowStack.IsOpen(this) && this.onClosed != null)
			{
				this.onClosed();
			}
		}

		
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 35f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, (this.reform ? "ReformCaravan" : "FormCaravan").Translate());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.MassUsage, this.MassCapacity, this.cachedMassCapacityExplanation, this.TilesPerDay, this.cachedTilesPerDayExplanation, this.DaysWorthOfFood, this.ForagedFoodPerDay, this.cachedForagedFoodPerDayExplanation, this.Visibility, this.cachedVisibilityExplanation, -1f, -1f, null), null, this.CurrentTile, (this.destinationTile == -1) ? null : new int?(this.TicksToArrive), this.lastMassFlashTime, new Rect(12f, 35f, inRect.width - 24f, 40f), true, (this.destinationTile == -1) ? null : ("\n" + "DaysWorthOfFoodTooltip_OnlyFirstWaypoint".Translate()), false);
			Dialog_FormCaravan.tabsList.Clear();
			Dialog_FormCaravan.tabsList.Add(new TabRecord("PawnsTab".Translate(), delegate
			{
				this.tab = Dialog_FormCaravan.Tab.Pawns;
			}, this.tab == Dialog_FormCaravan.Tab.Pawns));
			Dialog_FormCaravan.tabsList.Add(new TabRecord("ItemsTab".Translate(), delegate
			{
				this.tab = Dialog_FormCaravan.Tab.Items;
			}, this.tab == Dialog_FormCaravan.Tab.Items));
			inRect.yMin += 119f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs(inRect, Dialog_FormCaravan.tabsList, 200f);
			Dialog_FormCaravan.tabsList.Clear();
			inRect = inRect.ContractedBy(17f);
			inRect.height += 17f;
			GUI.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect inRect2 = rect2;
			inRect2.yMax -= 76f;
			bool flag = false;
			Dialog_FormCaravan.Tab tab = this.tab;
			if (tab != Dialog_FormCaravan.Tab.Pawns)
			{
				if (tab == Dialog_FormCaravan.Tab.Items)
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

		
		public override bool CausesMessageBackground()
		{
			return true;
		}

		
		public void Notify_ChoseRoute(int destinationTile)
		{
			this.destinationTile = destinationTile;
			this.startingTile = CaravanExitMapUtility.BestExitTileToGoTo(destinationTile, this.map);
			this.ticksToArriveDirty = true;
			this.daysWorthOfFoodDirty = true;
			Messages.Message("MessageChoseRoute".Translate(), MessageTypeDefOf.CautionInput, false);
			this.soundAppear.PlayOneShotOnCamera(null);
		}

		
		private void AddToTransferables(Thing t, bool setToTransferMax = false)
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
			if (setToTransferMax)
			{
				transferableOneWay.AdjustTo(transferableOneWay.CountToTransfer + t.stackCount);
			}
		}

		
		private void DoBottomButtons(Rect rect)
		{
			Rect rect2 = new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f - 17f, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect2, "AcceptButton".Translate(), true, true, true))
			{
				if (this.reform)
				{
					if (this.TryReformCaravan())
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.Close(false);
					}
				}
				else
				{
					List<string> list = new List<string>();
					Pair<float, float> daysWorthOfFood = this.DaysWorthOfFood;
					if (daysWorthOfFood.First < 5f)
					{
						list.Add((daysWorthOfFood.First < 0.1f) ? "DaysWorthOfFoodWarningDialog_NoFood".Translate() : "DaysWorthOfFoodWarningDialog".Translate(daysWorthOfFood.First.ToString("0.#")));
					}
					else if (this.MostFoodWillRotSoon)
					{
						list.Add("CaravanFoodWillRotSoonWarningDialog".Translate());
					}
					if (!TransferableUtility.GetPawnsFromTransferables(this.transferables).Any((Pawn pawn) => CaravanUtility.IsOwner(pawn, Faction.OfPlayer) && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled))
					{
						list.Add("CaravanIncapableOfSocial".Translate());
					}
					if (list.Count > 0)
					{
						if (this.CheckForErrors(TransferableUtility.GetPawnsFromTransferables(this.transferables)))
						{
							string str2 = string.Concat((from str in list
							select str + "\n\n").ToArray<string>()) + "CaravanAreYouSure".Translate();
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(str2, delegate
							{
								if (this.TryFormAndSendCaravan())
								{
									this.Close(false);
								}
							}, false, null));
						}
					}
					else if (this.TryFormAndSendCaravan())
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.Close(false);
					}
				}
			}
			if (Widgets.ButtonText(new Rect(rect2.x - 10f - this.BottomButtonSize.x, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y), "ResetButton".Translate(), true, true, true))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				this.CalculateAndRecacheTransferables();
			}
			if (this.ShowCancelButton && Widgets.ButtonText(new Rect(rect2.xMax + 10f, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y), "CancelButton".Translate(), true, true, true))
			{
				this.Close(true);
			}
			if (this.canChooseRoute)
			{
				Rect rect3 = new Rect(rect.width - this.BottomButtonSize.x, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y);
				if (Widgets.ButtonText(rect3, "ChooseRouteButton".Translate(), true, true, true))
				{
					List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
					this.soundClose.PlayOneShotOnCamera(null);
					if (!pawnsFromTransferables.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed))
					{
						Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						Find.WorldRoutePlanner.Start(this);
					}
				}
				if (this.destinationTile != -1)
				{
					Rect rect4 = rect3;
					rect4.y += rect3.height + 4f;
					rect4.height = 200f;
					rect4.xMin -= 200f;
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect4, "CaravanEstimatedDaysToDestination".Translate(((float)this.TicksToArrive / 60000f).ToString("0.#")));
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
			if (Prefs.DevMode)
			{
				float width = 200f;
				float num = this.BottomButtonSize.y / 2f;
				if (Widgets.ButtonText(new Rect(0f, rect.height - 55f - 17f, width, num), "Dev: Send instantly", true, true, true) && this.DebugTryFormCaravanInstantly())
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.Close(false);
				}
				if (Widgets.ButtonText(new Rect(0f, rect.height - 55f - 17f + num, width, num), "Dev: Select everything", true, true, true))
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.SetToSendEverything();
				}
			}
		}

		
		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			CaravanUIUtility.CreateCaravanTransferableWidgets(this.transferables, out this.pawnsTransfer, out this.itemsTransfer, "FormCaravanColonyThingCountTip".Translate(), this.IgnoreInventoryMode, () => this.MassCapacity - this.MassUsage, this.AutoStripSpawnedCorpses, this.CurrentTile, this.mapAboutToBeRemoved);
			this.CountToTransferChanged();
		}

		
		private bool DebugTryFormCaravanInstantly()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!pawnsFromTransferables.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer)))
			{
				Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			this.AddItemsFromTransferablesToRandomInventories(pawnsFromTransferables);
			int num = this.startingTile;
			if (num < 0)
			{
				num = CaravanExitMapUtility.RandomBestExitTileFrom(this.map);
			}
			if (num < 0)
			{
				num = this.CurrentTile;
			}
			CaravanFormingUtility.FormAndCreateCaravan(pawnsFromTransferables, Faction.OfPlayer, this.CurrentTile, num, this.destinationTile);
			return true;
		}

		
		private bool TryFormAndSendCaravan()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!this.CheckForErrors(pawnsFromTransferables))
			{
				return false;
			}
			Direction8Way direction8WayFromTo = Find.WorldGrid.GetDirection8WayFromTo(this.CurrentTile, this.startingTile);
			IntVec3 intVec;
			if (!this.TryFindExitSpot(pawnsFromTransferables, true, out intVec))
			{
				if (!this.TryFindExitSpot(pawnsFromTransferables, false, out intVec))
				{
					Messages.Message("CaravanCouldNotFindExitSpot".Translate(direction8WayFromTo.LabelShort()), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				Messages.Message("CaravanCouldNotFindReachableExitSpot".Translate(direction8WayFromTo.LabelShort()), new GlobalTargetInfo(intVec, this.map, false), MessageTypeDefOf.CautionInput, false);
			}
			IntVec3 meetingPoint;
			if (!this.TryFindRandomPackingSpot(intVec, out meetingPoint))
			{
				Messages.Message("CaravanCouldNotFindPackingSpot".Translate(direction8WayFromTo.LabelShort()), new GlobalTargetInfo(intVec, this.map, false), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			CaravanFormingUtility.StartFormingCaravan((from x in pawnsFromTransferables
			where !x.Downed
			select x).ToList<Pawn>(), (from x in pawnsFromTransferables
			where x.Downed
			select x).ToList<Pawn>(), Faction.OfPlayer, this.transferables, meetingPoint, intVec, this.startingTile, this.destinationTile);
			Messages.Message("CaravanFormationProcessStarted".Translate(), pawnsFromTransferables[0], MessageTypeDefOf.PositiveEvent, false);
			return true;
		}

		
		private bool TryReformCaravan()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!this.CheckForErrors(pawnsFromTransferables))
			{
				return false;
			}
			this.AddItemsFromTransferablesToRandomInventories(pawnsFromTransferables);
			Caravan caravan = CaravanExitMapUtility.ExitMapAndCreateCaravan(pawnsFromTransferables, Faction.OfPlayer, this.CurrentTile, this.CurrentTile, this.destinationTile, false);
			this.map.Parent.CheckRemoveMapNow();
			TaggedString taggedString = "MessageReformedCaravan".Translate();
			if (caravan.pather.Moving && caravan.pather.ArrivalAction != null)
			{
				taggedString += " " + "MessageFormedCaravan_Orders".Translate() + ": " + caravan.pather.ArrivalAction.Label + ".";
			}
			Messages.Message(taggedString, caravan, MessageTypeDefOf.TaskCompletion, false);
			return true;
		}

		
		private void AddItemsFromTransferablesToRandomInventories(List<Pawn> pawns)
		{
			this.transferables.RemoveAll((TransferableOneWay x) => x.AnyThing is Pawn);
			if (this.ListPlayerPawnsInventorySeparately)
			{
				for (int i = 0; i < pawns.Count; i++)
				{
					if (Dialog_FormCaravan.CanListInventorySeparately(pawns[i]))
					{
						ThingOwner<Thing> innerContainer = pawns[i].inventory.innerContainer;
						for (int j = innerContainer.Count - 1; j >= 0; j--)
						{
							this.RemoveCarriedItemFromTransferablesOrDrop(innerContainer[j], pawns[i], this.transferables);
						}
					}
				}
				for (int k = 0; k < this.transferables.Count; k++)
				{
					if (this.transferables[k].things.Any((Thing x) => !x.Spawned))
					{
						this.transferables[k].things.SortBy((Thing x) => x.Spawned);
					}
				}
			}
			//Action<Thing, IThingHolder> 9__3;
			//for (int l = 0; l < this.transferables.Count; l++)
			//{
			//	if (!(this.transferables[l].AnyThing is Corpse))
			//	{
			//		List<Thing> things = this.transferables[l].things;
			//		int countToTransfer = this.transferables[l].CountToTransfer;
			//		Action<Thing, IThingHolder> transferred;
			//		if ((transferred ) == null)
			//		{
			//			transferred = (9__3 = delegate(Thing splitPiece, IThingHolder originalHolder)
			//			{
			//				Thing item = splitPiece.TryMakeMinified();
			//				CaravanInventoryUtility.FindPawnToMoveInventoryTo(item, pawns, null, null).inventory.innerContainer.TryAdd(item, true);
			//			});
			//		}
			//		TransferableUtility.Transfer(things, countToTransfer, transferred);
			//	}
			//}
			//Action<Thing, int> 9__4;
			//for (int m = 0; m < this.transferables.Count; m++)
			//{
			//	if (this.transferables[m].AnyThing is Corpse)
			//	{
			//		List<Thing> things2 = this.transferables[m].things;
			//		int countToTransfer2 = this.transferables[m].CountToTransfer;
			//		Action<Thing, int> transfer;
			//		if ((transfer ) == null)
			//		{
			//			transfer = (9__4 = delegate(Thing originalThing, int numToTake)
			//			{
			//				if (this.AutoStripSpawnedCorpses)
			//				{
			//					Corpse corpse = originalThing as Corpse;
			//					if (corpse != null && corpse.Spawned)
			//					{
			//						corpse.Strip();
			//					}
			//				}
			//				Thing item = originalThing.SplitOff(numToTake);
			//				CaravanInventoryUtility.FindPawnToMoveInventoryTo(item, pawns, null, null).inventory.innerContainer.TryAdd(item, true);
			//			});
			//		}
			//		TransferableUtility.TransferNoSplit(things2, countToTransfer2, transfer, true, true);
			//	}
			//}
		}

		
		private bool CheckForErrors(List<Pawn> pawns)
		{
			if (this.MustChooseRoute && this.destinationTile < 0)
			{
				Messages.Message("MessageMustChooseRouteFirst".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (!this.reform && this.startingTile < 0)
			{
				Messages.Message("MessageNoValidExitTile".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (!pawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed))
			{
				Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (!this.reform && this.MassUsage > this.MassCapacity)
			{
				this.FlashMass();
				Messages.Message("TooBigCaravanMassUsage".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			Pawn pawn = pawns.Find((Pawn x) => !x.IsColonist && !pawns.Any((Pawn y) => y.IsColonist && y.CanReach(x, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn)));
			if (pawn != null)
			{
				Messages.Message("CaravanPawnIsUnreachable".Translate(pawn.LabelShort, pawn), pawn, MessageTypeDefOf.RejectInput, false);
				return false;
			}
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
							if (!t.Spawned || pawns.Any((Pawn x) => x.IsColonist && x.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn)))
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
								Messages.Message("CaravanItemIsUnreachableSingle".Translate(this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
							}
							else
							{
								Messages.Message("CaravanItemIsUnreachableMulti".Translate(countToTransfer, this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
							}
							return false;
						}
					}
				}
			}
			return true;
		}

		
		private bool TryFindExitSpot(List<Pawn> pawns, bool reachableForEveryColonist, out IntVec3 spot)
		{
			Rot4 rot;
			Rot4 rot2;
			CaravanExitMapUtility.GetExitMapEdges(Find.WorldGrid, this.CurrentTile, this.startingTile, out rot, out rot2);
			return (rot != Rot4.Invalid && this.TryFindExitSpot(pawns, reachableForEveryColonist, rot, out spot)) || (rot2 != Rot4.Invalid && this.TryFindExitSpot(pawns, reachableForEveryColonist, rot2, out spot)) || this.TryFindExitSpot(pawns, reachableForEveryColonist, rot.Rotated(RotationDirection.Clockwise), out spot) || this.TryFindExitSpot(pawns, reachableForEveryColonist, rot.Rotated(RotationDirection.Counterclockwise), out spot);
		}

		
		private bool TryFindExitSpot(List<Pawn> pawns, bool reachableForEveryColonist, Rot4 exitDirection, out IntVec3 spot)
		{
			if (this.startingTile < 0)
			{
				Log.Error("Can't find exit spot because startingTile is not set.", false);
				spot = IntVec3.Invalid;
				return false;
			}
			Predicate<IntVec3> validator = (IntVec3 x) => !x.Fogged(this.map) && x.Standable(this.map);
			if (reachableForEveryColonist)
			{
				return CellFinder.TryFindRandomEdgeCellWith(delegate(IntVec3 x)
				{
					if (!validator(x))
					{
						return false;
					}
					for (int j = 0; j < pawns.Count; j++)
					{
						if (pawns[j].IsColonist && !pawns[j].Downed && !pawns[j].CanReach(x, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							return false;
						}
					}
					return true;
				}, this.map, exitDirection, CellFinder.EdgeRoadChance_Always, out spot);
			}
			IntVec3 intVec = IntVec3.Invalid;
			int num = -1;
			foreach (IntVec3 intVec2 in CellRect.WholeMap(this.map).GetEdgeCells(exitDirection).InRandomOrder(null))
			{
				if (validator(intVec2))
				{
					int num2 = 0;
					for (int i = 0; i < pawns.Count; i++)
					{
						if (pawns[i].IsColonist && !pawns[i].Downed && pawns[i].CanReach(intVec2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							num2++;
						}
					}
					if (num2 > num)
					{
						num = num2;
						intVec = intVec2;
					}
				}
			}
			spot = intVec;
			return intVec.IsValid;
		}

		
		private bool TryFindRandomPackingSpot(IntVec3 exitSpot, out IntVec3 packingSpot)
		{
			Dialog_FormCaravan.tmpPackingSpots.Clear();
			List<Thing> list = this.map.listerThings.ThingsOfDef(ThingDefOf.CaravanPackingSpot);
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false);
			for (int i = 0; i < list.Count; i++)
			{
				if (this.map.reachability.CanReach(exitSpot, list[i], PathEndMode.OnCell, traverseParams))
				{
					Dialog_FormCaravan.tmpPackingSpots.Add(list[i]);
				}
			}
			if (Dialog_FormCaravan.tmpPackingSpots.Any<Thing>())
			{
				Thing thing = Dialog_FormCaravan.tmpPackingSpots.RandomElement<Thing>();
				Dialog_FormCaravan.tmpPackingSpots.Clear();
				packingSpot = thing.Position;
				return true;
			}
			return RCellFinder.TryFindRandomSpotJustOutsideColony(exitSpot, this.map, out packingSpot);
		}

		
		private void AddPawnsToTransferables()
		{
			List<Pawn> list = Dialog_FormCaravan.AllSendablePawns(this.map, this.reform);
			for (int i = 0; i < list.Count; i++)
			{
				bool setToTransferMax = (this.reform || this.mapAboutToBeRemoved) && !CaravanUtility.ShouldAutoCapture(list[i], Faction.OfPlayer);
				this.AddToTransferables(list[i], setToTransferMax);
			}
		}

		
		private void AddItemsToTransferables()
		{
			List<Thing> list = CaravanFormingUtility.AllReachableColonyItems(this.map, this.reform, this.reform, this.reform);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddToTransferables(list[i], false);
			}
			if (this.AutoStripSpawnedCorpses)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].Spawned)
					{
						this.TryAddCorpseInventoryAndGearToTransferables(list[j]);
					}
				}
			}
			if (this.ListPlayerPawnsInventorySeparately)
			{
				List<Pawn> list2 = Dialog_FormCaravan.AllSendablePawns(this.map, this.reform);
				for (int k = 0; k < list2.Count; k++)
				{
					if (Dialog_FormCaravan.CanListInventorySeparately(list2[k]))
					{
						ThingOwner<Thing> innerContainer = list2[k].inventory.innerContainer;
						for (int l = 0; l < innerContainer.Count; l++)
						{
							this.AddToTransferables(innerContainer[l], true);
							if (this.AutoStripSpawnedCorpses && innerContainer[l].Spawned)
							{
								this.TryAddCorpseInventoryAndGearToTransferables(innerContainer[l]);
							}
						}
					}
				}
			}
		}

		
		private void TryAddCorpseInventoryAndGearToTransferables(Thing potentiallyCorpse)
		{
			Corpse corpse = potentiallyCorpse as Corpse;
			if (corpse != null)
			{
				this.AddCorpseInventoryAndGearToTransferables(corpse);
			}
		}

		
		private void AddCorpseInventoryAndGearToTransferables(Corpse corpse)
		{
			Pawn_InventoryTracker inventory = corpse.InnerPawn.inventory;
			Pawn_ApparelTracker apparel = corpse.InnerPawn.apparel;
			Pawn_EquipmentTracker equipment = corpse.InnerPawn.equipment;
			for (int i = 0; i < inventory.innerContainer.Count; i++)
			{
				this.AddToTransferables(inventory.innerContainer[i], false);
			}
			if (apparel != null)
			{
				List<Apparel> wornApparel = apparel.WornApparel;
				for (int j = 0; j < wornApparel.Count; j++)
				{
					this.AddToTransferables(wornApparel[j], false);
				}
			}
			if (equipment != null)
			{
				List<ThingWithComps> allEquipmentListForReading = equipment.AllEquipmentListForReading;
				for (int k = 0; k < allEquipmentListForReading.Count; k++)
				{
					this.AddToTransferables(allEquipmentListForReading[k], false);
				}
			}
		}

		
		private void RemoveCarriedItemFromTransferablesOrDrop(Thing carried, Pawn carrier, List<TransferableOneWay> transferables)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(carried, transferables, TransferAsOneMode.PodsOrCaravanPacking);
			int num;
			if (transferableOneWay == null)
			{
				num = carried.stackCount;
			}
			else if (transferableOneWay.CountToTransfer >= carried.stackCount)
			{
				transferableOneWay.AdjustBy(-carried.stackCount);
				transferableOneWay.things.Remove(carried);
				num = 0;
			}
			else
			{
				num = carried.stackCount - transferableOneWay.CountToTransfer;
				transferableOneWay.AdjustTo(0);
			}
			if (num > 0)
			{
				Thing thing = carried.SplitOff(num);
				if (carrier.SpawnedOrAnyParentSpawned)
				{
					GenPlace.TryPlaceThing(thing, carrier.PositionHeld, carrier.MapHeld, ThingPlaceMode.Near, null, null, default(Rot4));
					return;
				}
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		
		private void FlashMass()
		{
			this.lastMassFlashTime = Time.time;
		}

		
		public static bool CanListInventorySeparately(Pawn p)
		{
			return p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer;
		}

		
		private void SetToSendEverything()
		{
			for (int i = 0; i < this.transferables.Count; i++)
			{
				this.transferables[i].AdjustTo(this.transferables[i].GetMaximumToTransfer());
			}
			this.CountToTransferChanged();
		}

		
		private void CountToTransferChanged()
		{
			this.massUsageDirty = true;
			this.massCapacityDirty = true;
			this.tilesPerDayDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.foragedFoodPerDayDirty = true;
			this.visibilityDirty = true;
			this.ticksToArriveDirty = true;
		}

		
		public static List<Pawn> AllSendablePawns(Map map, bool reform)
		{
			return CaravanFormingUtility.AllSendablePawns(map, true, reform, reform, reform, false, -1);
		}

		
		private Map map;

		
		private bool reform;

		
		private Action onClosed;

		
		private bool canChooseRoute;

		
		private bool mapAboutToBeRemoved;

		
		public bool choosingRoute;

		
		private bool thisWindowInstanceEverOpened;

		
		public List<TransferableOneWay> transferables;

		
		private TransferableOneWayWidget pawnsTransfer;

		
		private TransferableOneWayWidget itemsTransfer;

		
		private Dialog_FormCaravan.Tab tab;

		
		private float lastMassFlashTime = -9999f;

		
		private int startingTile = -1;

		
		private int destinationTile = -1;

		
		private bool massUsageDirty = true;

		
		private float cachedMassUsage;

		
		private bool massCapacityDirty = true;

		
		private float cachedMassCapacity;

		
		private string cachedMassCapacityExplanation;

		
		private bool tilesPerDayDirty = true;

		
		private float cachedTilesPerDay;

		
		private string cachedTilesPerDayExplanation;

		
		private bool daysWorthOfFoodDirty = true;

		
		private Pair<float, float> cachedDaysWorthOfFood;

		
		private bool foragedFoodPerDayDirty = true;

		
		private Pair<ThingDef, float> cachedForagedFoodPerDay;

		
		private string cachedForagedFoodPerDayExplanation;

		
		private bool visibilityDirty = true;

		
		private float cachedVisibility;

		
		private string cachedVisibilityExplanation;

		
		private bool ticksToArriveDirty = true;

		
		private int cachedTicksToArrive;

		
		private const float TitleRectHeight = 35f;

		
		private const float BottomAreaHeight = 55f;

		
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		
		private const float MaxDaysWorthOfFoodToShowWarningDialog = 5f;

		
		private static List<TabRecord> tabsList = new List<TabRecord>();

		
		private static List<Thing> tmpPackingSpots = new List<Thing>();

		
		private enum Tab
		{
			
			Pawns,
			
			Items
		}
	}
}
