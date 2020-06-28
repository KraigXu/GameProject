using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{

	[StaticConstructorOnStartup]
	public class WorldRoutePlanner
	{

		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		public bool FormingCaravan
		{
			get
			{
				return this.Active && this.currentFormCaravanDialog != null;
			}
		}

		// Token: 0x170012FE RID: 4862
		// (get) Token: 0x06007059 RID: 28761 RVA: 0x00272C14 File Offset: 0x00270E14
		private bool ShouldStop
		{
			get
			{
				return !this.active || !WorldRendererUtility.WorldRenderedNow || (Current.ProgramState == ProgramState.Playing && Find.TickManager.CurTimeSpeed != TimeSpeed.Paused);
			}
		}

		// Token: 0x170012FF RID: 4863
		// (get) Token: 0x0600705A RID: 28762 RVA: 0x00272C40 File Offset: 0x00270E40
		private int CaravanTicksPerMove
		{
			get
			{
				CaravanTicksPerMoveUtility.CaravanInfo? caravanInfo = this.CaravanInfo;
				if (caravanInfo != null && caravanInfo.Value.pawns.Any<Pawn>())
				{
					return CaravanTicksPerMoveUtility.GetTicksPerMove(caravanInfo.Value, null);
				}
				return 3464;
			}
		}

		// Token: 0x17001300 RID: 4864
		// (get) Token: 0x0600705B RID: 28763 RVA: 0x00272C84 File Offset: 0x00270E84
		private CaravanTicksPerMoveUtility.CaravanInfo? CaravanInfo
		{
			get
			{
				if (this.currentFormCaravanDialog != null)
				{
					return this.caravanInfoFromFormCaravanDialog;
				}
				Caravan caravanAtTheFirstWaypoint = this.CaravanAtTheFirstWaypoint;
				if (caravanAtTheFirstWaypoint != null)
				{
					return new CaravanTicksPerMoveUtility.CaravanInfo?(new CaravanTicksPerMoveUtility.CaravanInfo(caravanAtTheFirstWaypoint));
				}
				return null;
			}
		}

		// Token: 0x17001301 RID: 4865
		// (get) Token: 0x0600705C RID: 28764 RVA: 0x00272CBF File Offset: 0x00270EBF
		private Caravan CaravanAtTheFirstWaypoint
		{
			get
			{
				if (!this.waypoints.Any<RoutePlannerWaypoint>())
				{
					return null;
				}
				return Find.WorldObjects.PlayerControlledCaravanAt(this.waypoints[0].Tile);
			}
		}

		// Token: 0x0600705D RID: 28765 RVA: 0x00272CEB File Offset: 0x00270EEB
		public void Start()
		{
			if (this.active)
			{
				this.Stop();
			}
			this.active = true;
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				Find.TickManager.Pause();
			}
		}

		// Token: 0x0600705E RID: 28766 RVA: 0x00272D24 File Offset: 0x00270F24
		public void Start(Dialog_FormCaravan formCaravanDialog)
		{
			if (this.active)
			{
				this.Stop();
			}
			this.currentFormCaravanDialog = formCaravanDialog;
			this.caravanInfoFromFormCaravanDialog = new CaravanTicksPerMoveUtility.CaravanInfo?(new CaravanTicksPerMoveUtility.CaravanInfo(formCaravanDialog));
			formCaravanDialog.choosingRoute = true;
			Find.WindowStack.TryRemove(formCaravanDialog, true);
			this.Start();
			this.TryAddWaypoint(formCaravanDialog.CurrentTile, true);
			this.cantRemoveFirstWaypoint = true;
		}

		// Token: 0x0600705F RID: 28767 RVA: 0x00272D88 File Offset: 0x00270F88
		public void Stop()
		{
			this.active = false;
			for (int i = 0; i < this.waypoints.Count; i++)
			{
				this.waypoints[i].Destroy();
			}
			this.waypoints.Clear();
			this.cachedTicksToWaypoint.Clear();
			if (this.currentFormCaravanDialog != null)
			{
				this.currentFormCaravanDialog.Notify_NoLongerChoosingRoute();
			}
			this.caravanInfoFromFormCaravanDialog = null;
			this.currentFormCaravanDialog = null;
			this.cantRemoveFirstWaypoint = false;
			this.ReleasePaths();
		}

		// Token: 0x06007060 RID: 28768 RVA: 0x00272E0C File Offset: 0x0027100C
		public void WorldRoutePlannerUpdate()
		{
			if (this.active && this.ShouldStop)
			{
				this.Stop();
			}
			if (!this.active)
			{
				return;
			}
			for (int i = 0; i < this.paths.Count; i++)
			{
				this.paths[i].DrawPath(null);
			}
		}

		// Token: 0x06007061 RID: 28769 RVA: 0x00272E60 File Offset: 0x00271060
		public void WorldRoutePlannerOnGUI()
		{
			if (!this.active)
			{
				return;
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				if (this.currentFormCaravanDialog != null)
				{
					Find.WindowStack.Add(this.currentFormCaravanDialog);
				}
				else
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				this.Stop();
				Event.current.Use();
				return;
			}
			GenUI.DrawMouseAttachment(WorldRoutePlanner.MouseAttachment);
			if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
			{
				Caravan caravan = Find.WorldSelector.SelectableObjectsUnderMouse().FirstOrDefault<WorldObject>() as Caravan;
				int tile = (caravan != null) ? caravan.Tile : GenWorld.MouseTile(true);
				if (tile >= 0)
				{
					RoutePlannerWaypoint waypoint = this.MostRecentWaypointAt(tile);
					if (waypoint != null)
					{
						if (waypoint == this.waypoints[this.waypoints.Count - 1])
						{
							this.TryRemoveWaypoint(waypoint, true);
						}
						else
						{
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							list.Add(new FloatMenuOption("AddWaypoint".Translate(), delegate
							{
								this.TryAddWaypoint(tile, true);
							}, MenuOptionPriority.Default, null, null, 0f, null, null));
							list.Add(new FloatMenuOption("RemoveWaypoint".Translate(), delegate
							{
								this.TryRemoveWaypoint(waypoint, true);
							}, MenuOptionPriority.Default, null, null, 0f, null, null));
							Find.WindowStack.Add(new FloatMenu(list));
						}
					}
					else
					{
						this.TryAddWaypoint(tile, true);
					}
					Event.current.Use();
				}
			}
			this.DoRouteDetailsBox();
			if (this.DoChooseRouteButton())
			{
				return;
			}
			this.DoTileTooltips();
		}

		// Token: 0x06007062 RID: 28770 RVA: 0x00273038 File Offset: 0x00271238
		private void DoRouteDetailsBox()
		{
			//WorldRoutePlanner.<>c__DisplayClass31_0 <>c__DisplayClass31_ = new WorldRoutePlanner.<>c__DisplayClass31_0();
			//<>c__DisplayClass31_.<>4__this = this;
			//<>c__DisplayClass31_.rect = new Rect(((float)UI.screenWidth - WorldRoutePlanner.BottomWindowSize.x) / 2f, (float)UI.screenHeight - WorldRoutePlanner.BottomWindowSize.y - 45f, WorldRoutePlanner.BottomWindowSize.x, WorldRoutePlanner.BottomWindowSize.y);
			//if (Current.ProgramState == ProgramState.Entry)
			//{
			//	WorldRoutePlanner.<>c__DisplayClass31_0 <>c__DisplayClass31_2 = <>c__DisplayClass31_;
			//	<>c__DisplayClass31_2.rect.y = <>c__DisplayClass31_2.rect.y - 22f;
			//}
			//Find.WindowStack.ImmediateWindow(1373514241, <>c__DisplayClass31_.rect, WindowLayer.Dialog, delegate
			//{
			//	if (!<>c__DisplayClass31_.<>4__this.active)
			//	{
			//		return;
			//	}
			//	GUI.color = Color.white;
			//	Text.Anchor = TextAnchor.UpperCenter;
			//	Text.Font = GameFont.Small;
			//	float num = 6f;
			//	if (<>c__DisplayClass31_.<>4__this.waypoints.Count >= 2)
			//	{
			//		Widgets.Label(new Rect(0f, num, <>c__DisplayClass31_.rect.width, 25f), "RoutePlannerEstTimeToFinalDest".Translate(<>c__DisplayClass31_.<>4__this.GetTicksToWaypoint(<>c__DisplayClass31_.<>4__this.waypoints.Count - 1).ToStringTicksToDays("0.#")));
			//	}
			//	else if (<>c__DisplayClass31_.<>4__this.cantRemoveFirstWaypoint)
			//	{
			//		Widgets.Label(new Rect(0f, num, <>c__DisplayClass31_.rect.width, 25f), "RoutePlannerAddOneOrMoreWaypoints".Translate());
			//	}
			//	else
			//	{
			//		Widgets.Label(new Rect(0f, num, <>c__DisplayClass31_.rect.width, 25f), "RoutePlannerAddTwoOrMoreWaypoints".Translate());
			//	}
			//	num += 20f;
			//	if (<>c__DisplayClass31_.<>4__this.CaravanInfo == null || !<>c__DisplayClass31_.<>4__this.CaravanInfo.Value.pawns.Any<Pawn>())
			//	{
			//		GUI.color = new Color(0.8f, 0.6f, 0.6f);
			//		Widgets.Label(new Rect(0f, num, <>c__DisplayClass31_.rect.width, 25f), "RoutePlannerUsingAverageTicksPerMoveWarning".Translate());
			//	}
			//	else if (<>c__DisplayClass31_.<>4__this.currentFormCaravanDialog == null && <>c__DisplayClass31_.<>4__this.CaravanAtTheFirstWaypoint != null)
			//	{
			//		GUI.color = Color.gray;
			//		Widgets.Label(new Rect(0f, num, <>c__DisplayClass31_.rect.width, 25f), "RoutePlannerUsingTicksPerMoveOfCaravan".Translate(<>c__DisplayClass31_.<>4__this.CaravanAtTheFirstWaypoint.LabelCap));
			//	}
			//	num += 20f;
			//	GUI.color = Color.gray;
			//	Widgets.Label(new Rect(0f, num, <>c__DisplayClass31_.rect.width, 25f), "RoutePlannerPressRMBToAddAndRemoveWaypoints".Translate());
			//	num += 20f;
			//	if (<>c__DisplayClass31_.<>4__this.currentFormCaravanDialog != null)
			//	{
			//		Widgets.Label(new Rect(0f, num, <>c__DisplayClass31_.rect.width, 25f), "RoutePlannerPressEscapeToReturnToCaravanFormationDialog".Translate());
			//	}
			//	else
			//	{
			//		Widgets.Label(new Rect(0f, num, <>c__DisplayClass31_.rect.width, 25f), "RoutePlannerPressEscapeToExit".Translate());
			//	}
			//	num += 20f;
			//	GUI.color = Color.white;
			//	Text.Anchor = TextAnchor.UpperLeft;
			//}, true, false, 1f);
		}

		// Token: 0x06007063 RID: 28771 RVA: 0x002730E8 File Offset: 0x002712E8
		private bool DoChooseRouteButton()
		{
			if (this.currentFormCaravanDialog == null || this.waypoints.Count < 2)
			{
				return false;
			}
			if (Widgets.ButtonText(new Rect(((float)UI.screenWidth - WorldRoutePlanner.BottomButtonSize.x) / 2f, (float)UI.screenHeight - WorldRoutePlanner.BottomWindowSize.y - 45f - 10f - WorldRoutePlanner.BottomButtonSize.y, WorldRoutePlanner.BottomButtonSize.x, WorldRoutePlanner.BottomButtonSize.y), "ChooseRouteButton".Translate(), true, true, true))
			{
				Find.WindowStack.Add(this.currentFormCaravanDialog);
				this.currentFormCaravanDialog.Notify_ChoseRoute(this.waypoints[1].Tile);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				this.Stop();
				return true;
			}
			return false;
		}

		// Token: 0x06007064 RID: 28772 RVA: 0x002731C0 File Offset: 0x002713C0
		private void DoTileTooltips()
		{
			if (Mouse.IsInputBlockedNow)
			{
				return;
			}
			int num = GenWorld.MouseTile(true);
			if (num == -1)
			{
				return;
			}
			for (int i = 0; i < this.paths.Count; i++)
			{
				if (this.paths[i].NodesReversed.Contains(num))
				{
					string str = this.GetTileTip(num, i);
					Text.Font = GameFont.Small;
					Vector2 vector = Text.CalcSize(str);
					vector.x += 20f;
					vector.y += 20f;
					Vector2 mouseAttachedWindowPos = GenUI.GetMouseAttachedWindowPos(vector.x, vector.y);
					Rect rect = new Rect(mouseAttachedWindowPos, vector);
					Find.WindowStack.ImmediateWindow(1859615246, rect, WindowLayer.Super, delegate
					{
						Text.Font = GameFont.Small;
						Widgets.Label(rect.AtZero().ContractedBy(10f), str);
					}, true, false, 1f);
					return;
				}
			}
		}

		// Token: 0x06007065 RID: 28773 RVA: 0x002732AC File Offset: 0x002714AC
		private string GetTileTip(int tile, int pathIndex)
		{
			int num = this.paths[pathIndex].NodesReversed.IndexOf(tile);
			int num2;
			if (num > 0)
			{
				num2 = this.paths[pathIndex].NodesReversed[num - 1];
			}
			else if (pathIndex < this.paths.Count - 1 && this.paths[pathIndex + 1].NodesReversed.Count >= 2)
			{
				num2 = this.paths[pathIndex + 1].NodesReversed[this.paths[pathIndex + 1].NodesReversed.Count - 2];
			}
			else
			{
				num2 = -1;
			}
			int num3 = this.cachedTicksToWaypoint[pathIndex] + CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.paths[pathIndex].FirstNode, tile, this.paths[pathIndex], 0f, this.CaravanTicksPerMove, GenTicks.TicksAbs + this.cachedTicksToWaypoint[pathIndex]);
			int num4 = GenTicks.TicksAbs + num3;
			StringBuilder stringBuilder = new StringBuilder();
			if (num3 != 0)
			{
				stringBuilder.AppendLine("EstimatedTimeToTile".Translate(num3.ToStringTicksToDays("0.##")));
			}
			stringBuilder.AppendLine("ForagedFoodAmount".Translate() + ": " + Find.WorldGrid[tile].biome.forageability.ToStringPercent());
			stringBuilder.Append(VirtualPlantsUtility.GetVirtualPlantsStatusExplanationAt(tile, num4));
			if (num2 != -1)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				StringBuilder stringBuilder2 = new StringBuilder();
				float num5 = WorldPathGrid.CalculatedMovementDifficultyAt(num2, false, new int?(num4), stringBuilder2);
				float roadMovementDifficultyMultiplier = Find.WorldGrid.GetRoadMovementDifficultyMultiplier(tile, num2, stringBuilder2);
				stringBuilder.Append("TileMovementDifficulty".Translate() + ":\n" + stringBuilder2.ToString().Indented("  "));
				stringBuilder.AppendLine();
				stringBuilder.Append("  = ");
				stringBuilder.Append((num5 * roadMovementDifficultyMultiplier).ToString("0.#"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06007066 RID: 28774 RVA: 0x002734D8 File Offset: 0x002716D8
		public void DoRoutePlannerButton(ref float curBaseY)
		{
			float num = 57f;
			float num2 = 33f;
			Rect rect = new Rect((float)UI.screenWidth - 10f - num, curBaseY - 10f - num2, num, num2);
			if (Widgets.ButtonImage(rect, WorldRoutePlanner.ButtonTex, Color.white, new Color(0.8f, 0.8f, 0.8f), true))
			{
				if (this.active)
				{
					this.Stop();
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				else
				{
					this.Start();
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
			}
			TooltipHandler.TipRegionByKey(rect, "RoutePlannerButtonTip");
			curBaseY -= num2 + 20f;
		}

		// Token: 0x06007067 RID: 28775 RVA: 0x00273579 File Offset: 0x00271779
		public int GetTicksToWaypoint(int index)
		{
			return this.cachedTicksToWaypoint[index];
		}

		// Token: 0x06007068 RID: 28776 RVA: 0x00273588 File Offset: 0x00271788
		private void TryAddWaypoint(int tile, bool playSound = true)
		{
			if (Find.World.Impassable(tile))
			{
				Messages.Message("MessageCantAddWaypointBecauseImpassable".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (this.waypoints.Any<RoutePlannerWaypoint>() && !Find.WorldReachability.CanReach(this.waypoints[this.waypoints.Count - 1].Tile, tile))
			{
				Messages.Message("MessageCantAddWaypointBecauseUnreachable".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (this.waypoints.Count >= 25)
			{
				Messages.Message("MessageCantAddWaypointBecauseLimit".Translate(25), MessageTypeDefOf.RejectInput, false);
				return;
			}
			RoutePlannerWaypoint routePlannerWaypoint = (RoutePlannerWaypoint)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.RoutePlannerWaypoint);
			routePlannerWaypoint.Tile = tile;
			Find.WorldObjects.Add(routePlannerWaypoint);
			this.waypoints.Add(routePlannerWaypoint);
			this.RecreatePaths();
			if (playSound)
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06007069 RID: 28777 RVA: 0x00273684 File Offset: 0x00271884
		public void TryRemoveWaypoint(RoutePlannerWaypoint point, bool playSound = true)
		{
			if (this.cantRemoveFirstWaypoint && this.waypoints.Any<RoutePlannerWaypoint>() && point == this.waypoints[0])
			{
				Messages.Message("MessageCantRemoveWaypointBecauseFirst".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			point.Destroy();
			this.waypoints.Remove(point);
			for (int i = this.waypoints.Count - 1; i >= 1; i--)
			{
				if (this.waypoints[i].Tile == this.waypoints[i - 1].Tile)
				{
					this.waypoints[i].Destroy();
					this.waypoints.RemoveAt(i);
				}
			}
			this.RecreatePaths();
			if (playSound)
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600706A RID: 28778 RVA: 0x00273754 File Offset: 0x00271954
		private void ReleasePaths()
		{
			for (int i = 0; i < this.paths.Count; i++)
			{
				this.paths[i].ReleaseToPool();
			}
			this.paths.Clear();
		}

		// Token: 0x0600706B RID: 28779 RVA: 0x00273794 File Offset: 0x00271994
		private void RecreatePaths()
		{
			this.ReleasePaths();
			WorldPathFinder worldPathFinder = Find.WorldPathFinder;
			for (int i = 1; i < this.waypoints.Count; i++)
			{
				this.paths.Add(worldPathFinder.FindPath(this.waypoints[i - 1].Tile, this.waypoints[i].Tile, null, null));
			}
			this.cachedTicksToWaypoint.Clear();
			int num = 0;
			int caravanTicksPerMove = this.CaravanTicksPerMove;
			for (int j = 0; j < this.waypoints.Count; j++)
			{
				if (j == 0)
				{
					this.cachedTicksToWaypoint.Add(0);
				}
				else
				{
					num += CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.waypoints[j - 1].Tile, this.waypoints[j].Tile, this.paths[j - 1], 0f, caravanTicksPerMove, GenTicks.TicksAbs + num);
					this.cachedTicksToWaypoint.Add(num);
				}
			}
		}

		// Token: 0x0600706C RID: 28780 RVA: 0x00273890 File Offset: 0x00271A90
		private RoutePlannerWaypoint MostRecentWaypointAt(int tile)
		{
			for (int i = this.waypoints.Count - 1; i >= 0; i--)
			{
				if (this.waypoints[i].Tile == tile)
				{
					return this.waypoints[i];
				}
			}
			return null;
		}

		// Token: 0x0400450F RID: 17679
		private bool active;

		// Token: 0x04004510 RID: 17680
		private CaravanTicksPerMoveUtility.CaravanInfo? caravanInfoFromFormCaravanDialog;

		// Token: 0x04004511 RID: 17681
		private Dialog_FormCaravan currentFormCaravanDialog;

		// Token: 0x04004512 RID: 17682
		private List<WorldPath> paths = new List<WorldPath>();

		// Token: 0x04004513 RID: 17683
		private List<int> cachedTicksToWaypoint = new List<int>();

		// Token: 0x04004514 RID: 17684
		public List<RoutePlannerWaypoint> waypoints = new List<RoutePlannerWaypoint>();

		// Token: 0x04004515 RID: 17685
		private bool cantRemoveFirstWaypoint;

		// Token: 0x04004516 RID: 17686
		private const int MaxCount = 25;

		// Token: 0x04004517 RID: 17687
		private static readonly Texture2D ButtonTex = ContentFinder<Texture2D>.Get("UI/Misc/WorldRoutePlanner", true);

		// Token: 0x04004518 RID: 17688
		private static readonly Texture2D MouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/WaypointMouseAttachment", true);

		// Token: 0x04004519 RID: 17689
		private static readonly Vector2 BottomWindowSize = new Vector2(500f, 95f);

		// Token: 0x0400451A RID: 17690
		private static readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x0400451B RID: 17691
		private const float BottomWindowBotMargin = 45f;

		// Token: 0x0400451C RID: 17692
		private const float BottomWindowEntryExtraBotMargin = 22f;
	}
}
