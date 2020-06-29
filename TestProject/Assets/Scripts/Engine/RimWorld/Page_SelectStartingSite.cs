﻿using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class Page_SelectStartingSite : Page
	{
		
		// (get) Token: 0x06005AC0 RID: 23232 RVA: 0x001EE790 File Offset: 0x001EC990
		public override string PageTitle
		{
			get
			{
				return "SelectStartingSite".TranslateWithBackup("SelectLandingSite");
			}
		}

		
		// (get) Token: 0x06005AC1 RID: 23233 RVA: 0x001EE7AB File Offset: 0x001EC9AB
		public override Vector2 InitialSize
		{
			get
			{
				return Vector2.zero;
			}
		}

		
		// (get) Token: 0x06005AC2 RID: 23234 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		
		public Page_SelectStartingSite()
		{
			this.absorbInputAroundWindow = false;
			this.shadowAlpha = 0f;
			this.preventCameraMotion = false;
		}

		
		public override void PreOpen()
		{
			base.PreOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.Planet;
			Find.WorldInterface.Reset();
			((MainButtonWorker_ToggleWorld)MainButtonDefOf.World.Worker).resetViewNextTime = true;
		}

		
		public override void PostOpen()
		{
			base.PostOpen();
			Find.GameInitData.ChooseRandomStartingTile();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.WorldCameraMovement, OpportunityType.Important);
			TutorSystem.Notify_Event("PageStart-SelectStartingSite");
			this.tutorialStartTilePatch = null;
			if (TutorSystem.TutorialMode && Find.Tutor.activeLesson != null && Find.Tutor.activeLesson.Current != null && Find.Tutor.activeLesson.Current.Instruction == InstructionDefOf.ChooseLandingSite)
			{
				Find.WorldCameraDriver.ResetAltitude();
				Find.WorldCameraDriver.Update();
				List<int> list = new List<int>();
				float[] array = new float[Find.WorldGrid.TilesCount];
				WorldGrid worldGrid = Find.WorldGrid;
				Vector2 a = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
				float num = Vector2.Distance(a, Vector2.zero);
				for (int i = 0; i < worldGrid.TilesCount; i++)
				{
					Tile tile = worldGrid[i];
					if (TutorSystem.AllowAction("ChooseBiome-" + tile.biome.defName + "-" + tile.hilliness.ToString()))
					{
						Page_SelectStartingSite.tmpTileVertices.Clear();
						worldGrid.GetTileVertices(i, Page_SelectStartingSite.tmpTileVertices);
						Vector3 vector = Vector3.zero;
						for (int j = 0; j < Page_SelectStartingSite.tmpTileVertices.Count; j++)
						{
							vector += Page_SelectStartingSite.tmpTileVertices[j];
						}
						vector /= (float)Page_SelectStartingSite.tmpTileVertices.Count;
						Vector3 vector2 = Find.WorldCamera.WorldToScreenPoint(vector) / Prefs.UIScale;
						vector2.y = (float)UI.screenHeight - vector2.y;
						vector2.x = Mathf.Clamp(vector2.x, 0f, (float)UI.screenWidth);
						vector2.y = Mathf.Clamp(vector2.y, 0f, (float)UI.screenHeight);
						float num2 = 1f - Vector2.Distance(a, vector2) / num;
						Vector3 normalized = (vector - Find.WorldCamera.transform.position).normalized;
						float num3 = Vector3.Dot(Find.WorldCamera.transform.forward, normalized);
						array[i] = num2 * num3;
					}
					else
					{
						array[i] = float.NegativeInfinity;
					}
				}
				for (int k = 0; k < 16; k++)
				{
					for (int l = 0; l < array.Length; l++)
					{
						list.Clear();
						worldGrid.GetTileNeighbors(l, list);
						float num4 = array[l];
						if (num4 >= 0f)
						{
							for (int m = 0; m < list.Count; m++)
							{
								float num5 = array[list[m]];
								if (num5 >= 0f)
								{
									num4 += num5;
								}
							}
							array[l] = num4 / (float)list.Count;
						}
					}
				}
				float num6 = float.NegativeInfinity;
				int num7 = -1;
				for (int n = 0; n < array.Length; n++)
				{
					if (array[n] > 0f && num6 < array[n])
					{
						num6 = array[n];
						num7 = n;
					}
				}
				if (num7 != -1)
				{
					this.tutorialStartTilePatch = new int?(num7);
				}
			}
		}

		
		public override void PostClose()
		{
			base.PostClose();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		
		public override void DoWindowContents(Rect rect)
		{
			if (Find.WorldInterface.SelectedTile >= 0)
			{
				Find.GameInitData.startingTile = Find.WorldInterface.SelectedTile;
				return;
			}
			if (Find.WorldSelector.FirstSelectedObject != null)
			{
				Find.GameInitData.startingTile = Find.WorldSelector.FirstSelectedObject.Tile;
			}
		}

		
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			Text.Anchor = TextAnchor.UpperCenter;
			base.DrawPageTitle(new Rect(0f, 5f, (float)UI.screenWidth, 300f));
			Text.Anchor = TextAnchor.UpperLeft;
			this.DoCustomBottomButtons();
			if (this.tutorialStartTilePatch != null)
			{
				Page_SelectStartingSite.tmpTileVertices.Clear();
				Find.WorldGrid.GetTileVertices(this.tutorialStartTilePatch.Value, Page_SelectStartingSite.tmpTileVertices);
				Vector3 a = Vector3.zero;
				for (int i = 0; i < Page_SelectStartingSite.tmpTileVertices.Count; i++)
				{
					a += Page_SelectStartingSite.tmpTileVertices[i];
				}
				Color color = GUI.color;
				GUI.color = Color.white;
				GenUI.DrawArrowPointingAtWorldspace(a / (float)Page_SelectStartingSite.tmpTileVertices.Count, Find.WorldCamera);
				GUI.color = color;
			}
		}

		
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			int selectedTile = Find.WorldInterface.SelectedTile;
			if (selectedTile < 0)
			{
				Messages.Message("MustSelectStartingSite".TranslateWithBackup("MustSelectLandingSite"), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (!TileFinder.IsValidTileForNewSettlement(selectedTile, stringBuilder))
			{
				Messages.Message(stringBuilder.ToString(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			Tile tile = Find.WorldGrid[selectedTile];
			return TutorSystem.AllowAction("ChooseBiome-" + tile.biome.defName + "-" + tile.hilliness.ToString());
		}

		
		protected override void DoNext()
		{
			int selTile = Find.WorldInterface.SelectedTile;
			SettlementProximityGoodwillUtility.CheckConfirmSettle(selTile, delegate
			{
				Find.GameInitData.startingTile = selTile;

			});
		}

		
		private void DoCustomBottomButtons()
		{
			int num = TutorSystem.TutorialMode ? 4 : 5;
			int num2;
			if (num >= 4 && (float)UI.screenWidth < 1340f)
			{
				num2 = 2;
			}
			else
			{
				num2 = 1;
			}
			int num3 = Mathf.CeilToInt((float)num / (float)num2);
			float num4 = Page.BottomButSize.x * (float)num3 + 10f * (float)(num3 + 1);
			float num5 = (float)num2 * Page.BottomButSize.y + 10f * (float)(num2 + 1);
			Rect rect = new Rect(((float)UI.screenWidth - num4) / 2f, (float)UI.screenHeight - num5 - 4f, num4, num5);
			WorldInspectPane worldInspectPane = Find.WindowStack.WindowOfType<WorldInspectPane>();
			if (worldInspectPane != null && rect.x < InspectPaneUtility.PaneWidthFor(worldInspectPane) + 4f)
			{
				rect.x = InspectPaneUtility.PaneWidthFor(worldInspectPane) + 4f;
			}
			Widgets.DrawWindowBackground(rect);
			float num6 = rect.xMin + 10f;
			float num7 = rect.yMin + 10f;
			Text.Font = GameFont.Small;
			if (Widgets.ButtonText(new Rect(num6, num7, Page.BottomButSize.x, Page.BottomButSize.y), "Back".Translate(), true, true, true) && this.CanDoBack())
			{
				this.DoBack();
			}
			num6 += Page.BottomButSize.x + 10f;
			if (!TutorSystem.TutorialMode)
			{
				if (Widgets.ButtonText(new Rect(num6, num7, Page.BottomButSize.x, Page.BottomButSize.y), "Advanced".Translate(), true, true, true))
				{
					Find.WindowStack.Add(new Dialog_AdvancedGameConfig(Find.WorldInterface.SelectedTile));
				}
				num6 += Page.BottomButSize.x + 10f;
			}
			if (Widgets.ButtonText(new Rect(num6, num7, Page.BottomButSize.x, Page.BottomButSize.y), "SelectRandomSite".Translate(), true, true, true))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				Find.WorldInterface.SelectedTile = TileFinder.RandomStartingTile();
				Find.WorldCameraDriver.JumpTo(Find.WorldGrid.GetTileCenter(Find.WorldInterface.SelectedTile));
			}
			num6 += Page.BottomButSize.x + 10f;
			if (num2 == 2)
			{
				num6 = rect.xMin + 10f;
				num7 += Page.BottomButSize.y + 10f;
			}
			if (Widgets.ButtonText(new Rect(num6, num7, Page.BottomButSize.x, Page.BottomButSize.y), "WorldFactionsTab".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_FactionDuringLanding());
			}
			num6 += Page.BottomButSize.x + 10f;
			if (Widgets.ButtonText(new Rect(num6, num7, Page.BottomButSize.x, Page.BottomButSize.y), "Next".Translate(), true, true, true) && this.CanDoNext())
			{
				this.DoNext();
			}
			num6 += Page.BottomButSize.x + 10f;
			GenUI.AbsorbClicksInRect(rect);
		}

		
		private const float GapBetweenBottomButtons = 10f;

		
		private const float UseTwoRowsIfScreenWidthBelow = 1340f;

		
		private static List<Vector3> tmpTileVertices = new List<Vector3>();

		
		private int? tutorialStartTilePatch;
	}
}
