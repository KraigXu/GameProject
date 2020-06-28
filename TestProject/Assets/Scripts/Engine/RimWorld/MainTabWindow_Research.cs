using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EC9 RID: 3785
	[StaticConstructorOnStartup]
	public class MainTabWindow_Research : MainTabWindow
	{
		// Token: 0x170010C2 RID: 4290
		// (get) Token: 0x06005CB3 RID: 23731 RVA: 0x00201C64 File Offset: 0x001FFE64
		// (set) Token: 0x06005CB4 RID: 23732 RVA: 0x00201C6C File Offset: 0x001FFE6C
		private ResearchTabDef CurTab
		{
			get
			{
				return this.curTabInt;
			}
			set
			{
				if (value == this.curTabInt)
				{
					return;
				}
				this.curTabInt = value;
				Vector2 vector = this.ViewSize(this.CurTab);
				this.rightViewWidth = vector.x;
				this.rightViewHeight = vector.y;
				this.rightScrollPosition = Vector2.zero;
			}
		}

		// Token: 0x170010C3 RID: 4291
		// (get) Token: 0x06005CB5 RID: 23733 RVA: 0x00201CBC File Offset: 0x001FFEBC
		private bool ColonistsHaveResearchBench
		{
			get
			{
				bool result = false;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].listerBuildings.ColonistsHaveResearchBench())
					{
						result = true;
						break;
					}
				}
				return result;
			}
		}

		// Token: 0x170010C4 RID: 4292
		// (get) Token: 0x06005CB6 RID: 23734 RVA: 0x00201CFC File Offset: 0x001FFEFC
		public override Vector2 InitialSize
		{
			get
			{
				Vector2 initialSize = base.InitialSize;
				float b = (float)(UI.screenHeight - 35);
				float b2 = this.Margin + 10f + 32f + 10f + DefDatabase<ResearchTabDef>.AllDefs.Max((ResearchTabDef tab) => this.ViewSize(tab).y) + 10f + 10f + this.Margin;
				float a = Mathf.Max(initialSize.y, b2);
				initialSize.y = Mathf.Min(a, b);
				return initialSize;
			}
		}

		// Token: 0x06005CB7 RID: 23735 RVA: 0x00201D7C File Offset: 0x001FFF7C
		private Vector2 ViewSize(ResearchTabDef tab)
		{
			List<ResearchProjectDef> allDefsListForReading = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			float num = 0f;
			float num2 = 0f;
			Text.Font = GameFont.Small;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ResearchProjectDef researchProjectDef = allDefsListForReading[i];
				if (researchProjectDef.tab == tab)
				{
					Rect rect = new Rect(0f, 0f, 140f, 0f);
					Widgets.LabelCacheHeight(ref rect, this.GetLabel(researchProjectDef) + "\n", false, false);
					num = Mathf.Max(num, this.PosX(researchProjectDef) + 140f);
					num2 = Mathf.Max(num2, this.PosY(researchProjectDef) + rect.height);
				}
			}
			return new Vector2(num + 20f, num2 + 20f);
		}

		// Token: 0x06005CB8 RID: 23736 RVA: 0x00201E40 File Offset: 0x00200040
		public override void PreOpen()
		{
			base.PreOpen();
			this.selectedProject = Find.ResearchManager.currentProj;
			if (this.CurTab == null)
			{
				if (this.selectedProject != null)
				{
					this.CurTab = this.selectedProject.tab;
					return;
				}
				this.CurTab = ResearchTabDefOf.Main;
			}
		}

		// Token: 0x06005CB9 RID: 23737 RVA: 0x00201E90 File Offset: 0x00200090
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			this.windowRect.width = (float)UI.screenWidth;
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			float width = Mathf.Max(200f, inRect.width * 0.22f);
			Rect leftOutRect = new Rect(0f, 0f, width, inRect.height);
			Rect rightOutRect = new Rect(leftOutRect.xMax + 10f, 0f, inRect.width - leftOutRect.width - 10f, inRect.height);
			this.DrawLeftRect(leftOutRect);
			this.DrawRightRect(rightOutRect);
		}

		// Token: 0x06005CBA RID: 23738 RVA: 0x00201F38 File Offset: 0x00200138
		private void DrawLeftRect(Rect leftOutRect)
		{
			Rect position = leftOutRect;
			GUI.BeginGroup(position);
			if (this.selectedProject != null)
			{
				Rect outRect = new Rect(0f, 0f, position.width, 520f);
				Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, this.leftScrollViewHeight);
				Widgets.BeginScrollView(outRect, ref this.leftScrollPosition, viewRect, true);
				float num = 0f;
				Text.Font = GameFont.Medium;
				GenUI.SetLabelAlign(TextAnchor.MiddleLeft);
				Rect rect = new Rect(0f, num, viewRect.width - 0f, 50f);
				Widgets.LabelCacheHeight(ref rect, this.selectedProject.LabelCap, true, false);
				GenUI.ResetLabelAlign();
				Text.Font = GameFont.Small;
				num += rect.height;
				Rect rect2 = new Rect(0f, num, viewRect.width, 0f);
				Widgets.LabelCacheHeight(ref rect2, this.selectedProject.description, true, false);
				num += rect2.height;
				Rect rect3 = new Rect(0f, num, viewRect.width, 500f);
				num += this.DrawTechprintInfo(rect3, this.selectedProject);
				if (this.selectedProject.techLevel > Faction.OfPlayer.def.techLevel)
				{
					float num2 = this.selectedProject.CostFactor(Faction.OfPlayer.def.techLevel);
					Rect rect4 = new Rect(0f, num, viewRect.width, 0f);
					string text = "TechLevelTooLow".Translate(Faction.OfPlayer.def.techLevel.ToStringHuman(), this.selectedProject.techLevel.ToStringHuman(), num2.ToStringPercent());
					if (num2 != 1f)
					{
						text += " " + "ResearchCostComparison".Translate(this.selectedProject.baseCost.ToString("F0"), this.selectedProject.CostApparent.ToString("F0"));
					}
					Widgets.LabelCacheHeight(ref rect4, text, true, false);
					num += rect4.height;
				}
				if (!this.ColonistsHaveResearchBench)
				{
					GUI.color = ColoredText.RedReadable;
					Rect rect5 = new Rect(0f, num, viewRect.width, 0f);
					Widgets.LabelCacheHeight(ref rect5, "CannotResearchNoBench".Translate(), true, false);
					num += rect5.height;
					GUI.color = Color.white;
				}
				Rect rect6 = new Rect(0f, num, viewRect.width, 500f);
				num += this.DrawResearchPrereqs(this.selectedProject, rect6);
				Rect rect7 = new Rect(0f, num, viewRect.width, 500f);
				num += this.DrawResearchBenchRequirements(this.selectedProject, rect7);
				Rect rect8 = new Rect(0f, num, viewRect.width, 500f);
				num += this.DrawUnlockableHyperlinks(rect8, this.selectedProject);
				num += 3f;
				this.leftScrollViewHeight = num;
				Widgets.EndScrollView();
				Rect rect9 = new Rect(0f, outRect.yMax + 10f, position.width, 68f);
				if (this.selectedProject.CanStartNow && this.selectedProject != Find.ResearchManager.currentProj)
				{
					if (Widgets.ButtonText(rect9, "Research".Translate(), true, true, true))
					{
						SoundDefOf.ResearchStart.PlayOneShotOnCamera(null);
						Find.ResearchManager.currentProj = this.selectedProject;
						TutorSystem.Notify_Event("StartResearchProject");
						if (!this.ColonistsHaveResearchBench)
						{
							Messages.Message("MessageResearchMenuWithoutBench".Translate(), MessageTypeDefOf.CautionInput, true);
						}
					}
				}
				else
				{
					string text2;
					if (this.selectedProject.IsFinished)
					{
						text2 = "Finished".Translate();
						Text.Anchor = TextAnchor.MiddleCenter;
					}
					else if (this.selectedProject == Find.ResearchManager.currentProj)
					{
						text2 = "InProgress".Translate();
						Text.Anchor = TextAnchor.MiddleCenter;
					}
					else
					{
						text2 = "Locked".Translate() + ":";
						if (!this.selectedProject.PrerequisitesCompleted)
						{
							text2 += "\n  " + "PrerequisitesNotCompleted".Translate();
						}
						if (!this.selectedProject.TechprintRequirementMet)
						{
							text2 += "\n  " + "InsufficientTechprintsApplied".Translate(this.selectedProject.TechprintsApplied, this.selectedProject.techprintCount);
						}
					}
					Widgets.DrawHighlight(rect9);
					Widgets.Label(rect9.ContractedBy(5f), text2);
					Text.Anchor = TextAnchor.UpperLeft;
				}
				Rect rect10 = new Rect(0f, rect9.yMax + 10f, position.width, 35f);
				Widgets.FillableBar(rect10, this.selectedProject.ProgressPercent, MainTabWindow_Research.ResearchBarFillTex, MainTabWindow_Research.ResearchBarBGTex, true);
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect10, this.selectedProject.ProgressApparent.ToString("F0") + " / " + this.selectedProject.CostApparent.ToString("F0"));
				Text.Anchor = TextAnchor.UpperLeft;
				if (Prefs.DevMode && this.selectedProject != Find.ResearchManager.currentProj && !this.selectedProject.IsFinished && Widgets.ButtonText(new Rect(rect9.x, rect9.y - 30f, 120f, 30f), "Debug: Finish now", true, true, true))
				{
					Find.ResearchManager.currentProj = this.selectedProject;
					Find.ResearchManager.FinishProject(this.selectedProject, false, null);
				}
				if (Prefs.DevMode && !this.selectedProject.TechprintRequirementMet && Widgets.ButtonText(new Rect(rect9.x + 120f, rect9.y - 30f, 120f, 30f), "Debug: Apply techprint", true, true, true))
				{
					Find.ResearchManager.ApplyTechprint(this.selectedProject, null);
					SoundDefOf.TechprintApplied.PlayOneShotOnCamera(null);
				}
			}
			GUI.EndGroup();
		}

		// Token: 0x06005CBB RID: 23739 RVA: 0x002025C4 File Offset: 0x002007C4
		private float CoordToPixelsX(float x)
		{
			return x * 190f;
		}

		// Token: 0x06005CBC RID: 23740 RVA: 0x002025CD File Offset: 0x002007CD
		private float CoordToPixelsY(float y)
		{
			return y * 100f;
		}

		// Token: 0x06005CBD RID: 23741 RVA: 0x002025D6 File Offset: 0x002007D6
		private float PixelsToCoordX(float x)
		{
			return x / 190f;
		}

		// Token: 0x06005CBE RID: 23742 RVA: 0x002025DF File Offset: 0x002007DF
		private float PixelsToCoordY(float y)
		{
			return y / 100f;
		}

		// Token: 0x06005CBF RID: 23743 RVA: 0x002025E8 File Offset: 0x002007E8
		private float PosX(ResearchProjectDef d)
		{
			return this.CoordToPixelsX(d.ResearchViewX);
		}

		// Token: 0x06005CC0 RID: 23744 RVA: 0x002025F6 File Offset: 0x002007F6
		private float PosY(ResearchProjectDef d)
		{
			return this.CoordToPixelsY(d.ResearchViewY);
		}

		// Token: 0x06005CC1 RID: 23745 RVA: 0x00202604 File Offset: 0x00200804
		public override void PostOpen()
		{
			base.PostOpen();
			this.tabs.Clear();
			foreach (ResearchTabDef localTabDef2 in DefDatabase<ResearchTabDef>.AllDefs)
			{
				ResearchTabDef localTabDef = localTabDef2;
				this.tabs.Add(new TabRecord(localTabDef.LabelCap, delegate
				{
					this.CurTab = localTabDef;
				}, () => this.CurTab == localTabDef));
			}
		}

		// Token: 0x06005CC2 RID: 23746 RVA: 0x002026A8 File Offset: 0x002008A8
		private void DrawRightRect(Rect rightOutRect)
		{
			rightOutRect.yMin += 32f;
			Widgets.DrawMenuSection(rightOutRect);
			TabDrawer.DrawTabs(rightOutRect, this.tabs, 200f);
			if (Prefs.DevMode)
			{
				Rect rect = rightOutRect;
				rect.yMax = rect.yMin + 20f;
				rect.xMin = rect.xMax - 80f;
				Rect butRect = rect.RightPartPixels(30f);
				rect = rect.LeftPartPixels(rect.width - 30f);
				Widgets.CheckboxLabeled(rect, "Edit", ref this.editMode, false, null, null, false);
				if (Widgets.ButtonImageFitted(butRect, TexButton.Copy))
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (ResearchProjectDef researchProjectDef in from def in DefDatabase<ResearchProjectDef>.AllDefsListForReading
					where def.Debug_IsPositionModified()
					select def)
					{
						stringBuilder.AppendLine(researchProjectDef.defName);
						stringBuilder.AppendLine(string.Format("  <researchViewX>{0}</researchViewX>", researchProjectDef.ResearchViewX.ToString("F2")));
						stringBuilder.AppendLine(string.Format("  <researchViewY>{0}</researchViewY>", researchProjectDef.ResearchViewY.ToString("F2")));
						stringBuilder.AppendLine();
					}
					GUIUtility.systemCopyBuffer = stringBuilder.ToString();
					Messages.Message("Modified data copied to clipboard.", MessageTypeDefOf.SituationResolved, false);
				}
			}
			else
			{
				this.editMode = false;
			}
			bool flag = false;
			Rect outRect = rightOutRect.ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, this.rightViewWidth, this.rightViewHeight);
			rect2.ContractedBy(10f);
			rect2.width = this.rightViewWidth;
			Rect position = rect2.ContractedBy(10f);
			Vector2 start = default(Vector2);
			Vector2 end = default(Vector2);
			Widgets.ScrollHorizontal(outRect, ref this.rightScrollPosition, rect2, 20f);
			Widgets.BeginScrollView(outRect, ref this.rightScrollPosition, rect2, true);
			GUI.BeginGroup(position);
			List<ResearchProjectDef> allDefsListForReading = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < allDefsListForReading.Count; j++)
				{
					ResearchProjectDef researchProjectDef2 = allDefsListForReading[j];
					if (researchProjectDef2.tab == this.CurTab)
					{
						start.x = this.PosX(researchProjectDef2);
						start.y = this.PosY(researchProjectDef2) + 25f;
						for (int k = 0; k < researchProjectDef2.prerequisites.CountAllowNull<ResearchProjectDef>(); k++)
						{
							ResearchProjectDef researchProjectDef3 = researchProjectDef2.prerequisites[k];
							if (researchProjectDef3 != null && researchProjectDef3.tab == this.CurTab)
							{
								end.x = this.PosX(researchProjectDef3) + 140f;
								end.y = this.PosY(researchProjectDef3) + 25f;
								if (this.selectedProject == researchProjectDef2 || this.selectedProject == researchProjectDef3)
								{
									if (i == 1)
									{
										Widgets.DrawLine(start, end, TexUI.HighlightLineResearchColor, 4f);
									}
								}
								else if (i == 0)
								{
									Widgets.DrawLine(start, end, TexUI.DefaultLineResearchColor, 2f);
								}
							}
						}
					}
				}
			}
			Rect other = new Rect(this.rightScrollPosition.x, this.rightScrollPosition.y, outRect.width, outRect.height).ExpandedBy(10f);
			for (int l = 0; l < allDefsListForReading.Count; l++)
			{
				ResearchProjectDef researchProjectDef4 = allDefsListForReading[l];
				if (researchProjectDef4.tab == this.CurTab)
				{
					Rect source = new Rect(this.PosX(researchProjectDef4), this.PosY(researchProjectDef4), 140f, 50f);
					Rect rect3 = new Rect(source);
					string label = this.GetLabel(researchProjectDef4);
					Widgets.LabelCacheHeight(ref rect3, this.GetLabelWithNewlineCached(label), true, false);
					if (rect3.Overlaps(other))
					{
						Color color = Widgets.NormalOptionColor;
						Color color2 = default(Color);
						Color borderColor = default(Color);
						bool flag2 = !researchProjectDef4.IsFinished && !researchProjectDef4.CanStartNow;
						if (researchProjectDef4 == Find.ResearchManager.currentProj)
						{
							color2 = TexUI.ActiveResearchColor;
						}
						else if (researchProjectDef4.IsFinished)
						{
							color2 = TexUI.FinishedResearchColor;
						}
						else if (flag2)
						{
							color2 = TexUI.LockedResearchColor;
						}
						else if (researchProjectDef4.CanStartNow)
						{
							color2 = TexUI.AvailResearchColor;
						}
						if (this.editMode && this.draggingTabs.Contains(researchProjectDef4))
						{
							borderColor = Color.yellow;
						}
						else if (this.selectedProject == researchProjectDef4)
						{
							color2 += TexUI.HighlightBgResearchColor;
							borderColor = TexUI.HighlightBorderResearchColor;
						}
						else
						{
							borderColor = TexUI.DefaultBorderResearchColor;
						}
						if (flag2)
						{
							color = MainTabWindow_Research.ProjectWithMissingPrerequisiteLabelColor;
						}
						if (this.selectedProject != null)
						{
							if ((researchProjectDef4.prerequisites != null && researchProjectDef4.prerequisites.Contains(this.selectedProject)) || (researchProjectDef4.hiddenPrerequisites != null && researchProjectDef4.hiddenPrerequisites.Contains(this.selectedProject)))
							{
								borderColor = TexUI.HighlightLineResearchColor;
							}
							if (!researchProjectDef4.IsFinished && ((this.selectedProject.prerequisites != null && this.selectedProject.prerequisites.Contains(researchProjectDef4)) || (this.selectedProject.hiddenPrerequisites != null && this.selectedProject.hiddenPrerequisites.Contains(researchProjectDef4))))
							{
								borderColor = TexUI.DependencyOutlineResearchColor;
							}
						}
						if (this.requiredByThisFound)
						{
							for (int m = 0; m < researchProjectDef4.requiredByThis.CountAllowNull<ResearchProjectDef>(); m++)
							{
								ResearchProjectDef researchProjectDef5 = researchProjectDef4.requiredByThis[m];
								if (this.selectedProject == researchProjectDef5)
								{
									borderColor = TexUI.HighlightLineResearchColor;
								}
							}
						}
						Rect rect4 = rect3;
						Widgets.LabelCacheHeight(ref rect4, " ", true, false);
						if (Widgets.CustomButtonText(ref rect3, "", color2, color, borderColor, false, 1, true, true))
						{
							SoundDefOf.Click.PlayOneShotOnCamera(null);
							this.selectedProject = researchProjectDef4;
						}
						rect4.y = rect3.y + rect3.height - rect4.height;
						Rect rect5 = rect4;
						rect5.x += 10f;
						rect5.width = rect5.width / 2f - 10f;
						Rect rect6 = rect5;
						rect6.x += rect5.width;
						TextAnchor anchor = Text.Anchor;
						Color color3 = GUI.color;
						GUI.color = color;
						Text.Anchor = TextAnchor.UpperCenter;
						Widgets.Label(rect3, label);
						GUI.color = color;
						Text.Anchor = TextAnchor.MiddleLeft;
						Widgets.Label(rect5, researchProjectDef4.CostApparent.ToString());
						if (researchProjectDef4.techprintCount > 0)
						{
							GUI.color = (researchProjectDef4.TechprintRequirementMet ? MainTabWindow_Research.FulfilledPrerequisiteColor : MainTabWindow_Research.MissingPrerequisiteColor);
							Text.Anchor = TextAnchor.MiddleRight;
							Widgets.Label(rect6, this.GetTechprintsInfoCached(researchProjectDef4.TechprintsApplied, researchProjectDef4.techprintCount));
						}
						GUI.color = color3;
						Text.Anchor = anchor;
						if (this.editMode && Mouse.IsOver(rect3) && Input.GetMouseButtonDown(0))
						{
							flag = true;
							if (Input.GetKey(KeyCode.LeftShift))
							{
								if (!this.draggingTabs.Contains(researchProjectDef4))
								{
									this.draggingTabs.Add(researchProjectDef4);
								}
							}
							else if (!Input.GetKey(KeyCode.LeftControl) && !this.draggingTabs.Contains(researchProjectDef4))
							{
								this.draggingTabs.Clear();
								this.draggingTabs.Add(researchProjectDef4);
							}
							if (Input.GetKey(KeyCode.LeftControl) && this.draggingTabs.Contains(researchProjectDef4))
							{
								this.draggingTabs.Remove(researchProjectDef4);
							}
						}
					}
				}
			}
			GUI.EndGroup();
			Widgets.EndScrollView();
			if (this.editMode)
			{
				if (!flag && Input.GetMouseButtonDown(0))
				{
					this.draggingTabs.Clear();
				}
				if (!this.draggingTabs.NullOrEmpty<ResearchProjectDef>())
				{
					if (Input.GetMouseButtonUp(0))
					{
						for (int n = 0; n < this.draggingTabs.Count; n++)
						{
							this.draggingTabs[n].Debug_SnapPositionData();
						}
						return;
					}
					if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0) && Event.current.type == EventType.Layout)
					{
						for (int num = 0; num < this.draggingTabs.Count; num++)
						{
							this.draggingTabs[num].Debug_ApplyPositionDelta(new Vector2(this.PixelsToCoordX(Event.current.delta.x), this.PixelsToCoordY(Event.current.delta.y)));
						}
					}
				}
			}
		}

		// Token: 0x06005CC3 RID: 23747 RVA: 0x00202F34 File Offset: 0x00201134
		private float DrawResearchPrereqs(ResearchProjectDef project, Rect rect)
		{
			if (project.prerequisites.NullOrEmpty<ResearchProjectDef>())
			{
				return 0f;
			}
			float xMin = rect.xMin;
			float yMin = rect.yMin;
			Widgets.LabelCacheHeight(ref rect, "ResearchPrerequisites".Translate() + ":", true, false);
			rect.yMin += rect.height;
			rect.xMin += 6f;
			for (int i = 0; i < project.prerequisites.Count; i++)
			{
				this.SetPrerequisiteStatusColor(project.prerequisites[i].IsFinished, project);
				Widgets.LabelCacheHeight(ref rect, project.prerequisites[i].LabelCap, true, false);
				rect.yMin += rect.height;
			}
			if (project.hiddenPrerequisites != null)
			{
				for (int j = 0; j < project.hiddenPrerequisites.Count; j++)
				{
					this.SetPrerequisiteStatusColor(project.hiddenPrerequisites[j].IsFinished, project);
					Widgets.LabelCacheHeight(ref rect, project.hiddenPrerequisites[j].LabelCap, true, false);
					rect.yMin += rect.height;
				}
			}
			GUI.color = Color.white;
			rect.xMin = xMin;
			return rect.yMin - yMin;
		}

		// Token: 0x06005CC4 RID: 23748 RVA: 0x00203095 File Offset: 0x00201295
		private string GetLabelWithNewlineCached(string label)
		{
			if (!MainTabWindow_Research.labelsWithNewlineCached.ContainsKey(label))
			{
				MainTabWindow_Research.labelsWithNewlineCached.Add(label, label + "\n");
			}
			return MainTabWindow_Research.labelsWithNewlineCached[label];
		}

		// Token: 0x06005CC5 RID: 23749 RVA: 0x002030C8 File Offset: 0x002012C8
		private string GetTechprintsInfoCached(int applied, int total)
		{
			Pair<int, int> key = new Pair<int, int>(applied, total);
			if (!MainTabWindow_Research.techprintsInfoCached.ContainsKey(key))
			{
				MainTabWindow_Research.techprintsInfoCached.Add(key, string.Format("{0} / {1}", applied.ToString(), total.ToString()));
			}
			return MainTabWindow_Research.techprintsInfoCached[key];
		}


		private float DrawResearchBenchRequirements(ResearchProjectDef project, Rect rect)
		{
			float xMin = rect.xMin;
			float yMin = rect.yMin;
			if (project.requiredResearchBuilding != null)
			{
				bool present = false;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Building> allBuildingsColonist = maps[i].listerBuildings.allBuildingsColonist;
					Predicate<Building> match=x => x.def == project.requiredResearchBuilding;

					if (allBuildingsColonist.Find(match) != null)
					{
						present = true;
						break;
					}
				}
				Widgets.LabelCacheHeight(ref rect, "RequiredResearchBench".Translate() + ":", true, false);
				rect.xMin += 6f;
				rect.yMin += rect.height;
				this.SetPrerequisiteStatusColor(present, project);
				Widgets.LabelCacheHeight(ref rect, project.requiredResearchBuilding.LabelCap, true, false);
				rect.yMin += rect.height;
				GUI.color = Color.white;
				rect.xMin = xMin;
			}
			if (!project.requiredResearchFacilities.NullOrEmpty<ThingDef>())
			{
				Widgets.LabelCacheHeight(ref rect, "RequiredResearchBenchFacilities".Translate() + ":", true, false);
				rect.yMin += rect.height;
				Building_ResearchBench building_ResearchBench = this.FindBenchFulfillingMostRequirements(project.requiredResearchBuilding, project.requiredResearchFacilities);
				CompAffectedByFacilities bestMatchingBench = null;
				if (building_ResearchBench != null)
				{
					bestMatchingBench = building_ResearchBench.TryGetComp<CompAffectedByFacilities>();
				}
				rect.xMin += 6f;
				for (int j = 0; j < project.requiredResearchFacilities.Count; j++)
				{
					this.DrawResearchBenchFacilityRequirement(project.requiredResearchFacilities[j], bestMatchingBench, project, ref rect);
					rect.yMin += rect.height;
				}
			}
			GUI.color = Color.white;
			rect.xMin = xMin;
			return rect.yMin - yMin;
		}

		// Token: 0x06005CC7 RID: 23751 RVA: 0x00203350 File Offset: 0x00201550
		private float DrawUnlockableHyperlinks(Rect rect, ResearchProjectDef project)
		{
			List<Dialog_InfoCard.Hyperlink> infoCardHyperlinks = project.InfoCardHyperlinks;
			if (infoCardHyperlinks.NullOrEmpty<Dialog_InfoCard.Hyperlink>())
			{
				return 0f;
			}
			float yMin = rect.yMin;
			Widgets.LabelCacheHeight(ref rect, "Unlocks".Translate() + ":", true, false);
			rect.x += 6f;
			rect.yMin += rect.height;
			for (int i = 0; i < infoCardHyperlinks.Count; i++)
			{
				Widgets.HyperlinkWithIcon(new Rect(rect.x, rect.yMin, rect.width, 24f), infoCardHyperlinks[i], null, 2f, 6f);
				rect.yMin += 24f;
			}
			return rect.yMin - yMin;
		}

		// Token: 0x06005CC8 RID: 23752 RVA: 0x00203428 File Offset: 0x00201628
		private float DrawTechprintInfo(Rect rect, ResearchProjectDef project)
		{
			if (this.selectedProject.techprintCount == 0)
			{
				return 0f;
			}
			float xMin = rect.xMin;
			float yMin = rect.yMin;
			string text = "ResearchTechprintsFromFactions".Translate();
			float num = Text.CalcHeight(text, rect.width);
			Widgets.Label(new Rect(rect.x, yMin, rect.width, num), text);
			rect.x += 6f;
			if (this.selectedProject.heldByFactionCategoryTags != null)
			{
				foreach (string b in this.selectedProject.heldByFactionCategoryTags)
				{
					foreach (Faction faction in Find.FactionManager.AllFactionsInViewOrder)
					{
						if (faction.def.categoryTag == b)
						{
							string name = faction.Name;
							Rect rect2 = new Rect(rect.x, yMin + num, rect.width, Text.CalcHeight(name, rect.width));
							Widgets.Label(rect2, name);
							num += rect2.height;
						}
					}
				}
			}
			rect.xMin = xMin;
			return num;
		}

		// Token: 0x06005CC9 RID: 23753 RVA: 0x002035A0 File Offset: 0x002017A0
		private string GetLabel(ResearchProjectDef r)
		{
			return r.LabelCap;
		}

		// Token: 0x06005CCA RID: 23754 RVA: 0x002035AD File Offset: 0x002017AD
		private void SetPrerequisiteStatusColor(bool present, ResearchProjectDef project)
		{
			if (project.IsFinished)
			{
				return;
			}
			if (present)
			{
				GUI.color = MainTabWindow_Research.FulfilledPrerequisiteColor;
				return;
			}
			GUI.color = MainTabWindow_Research.MissingPrerequisiteColor;
		}

		// Token: 0x06005CCB RID: 23755 RVA: 0x002035D0 File Offset: 0x002017D0
		private void DrawResearchBenchFacilityRequirement(ThingDef requiredFacility, CompAffectedByFacilities bestMatchingBench, ResearchProjectDef project, ref Rect rect)
		{
			Thing thing = null;
			Thing thing2 = null;
			if (bestMatchingBench != null)
			{
				thing = bestMatchingBench.LinkedFacilitiesListForReading.Find((Thing x) => x.def == requiredFacility);
				thing2 = bestMatchingBench.LinkedFacilitiesListForReading.Find((Thing x) => x.def == requiredFacility && bestMatchingBench.IsFacilityActive(x));
			}
			this.SetPrerequisiteStatusColor(thing2 != null, project);
			string text = requiredFacility.LabelCap;
			if (thing != null && thing2 == null)
			{
				text += " (" + "InactiveFacility".Translate() + ")";
			}
			Widgets.LabelCacheHeight(ref rect, text, true, false);
		}

		// Token: 0x06005CCC RID: 23756 RVA: 0x00203690 File Offset: 0x00201890
		private Building_ResearchBench FindBenchFulfillingMostRequirements(ThingDef requiredResearchBench, List<ThingDef> requiredFacilities)
		{
			MainTabWindow_Research.tmpAllBuildings.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				MainTabWindow_Research.tmpAllBuildings.AddRange(maps[i].listerBuildings.allBuildingsColonist);
			}
			float num = 0f;
			Building_ResearchBench building_ResearchBench = null;
			for (int j = 0; j < MainTabWindow_Research.tmpAllBuildings.Count; j++)
			{
				Building_ResearchBench building_ResearchBench2 = MainTabWindow_Research.tmpAllBuildings[j] as Building_ResearchBench;
				if (building_ResearchBench2 != null && (requiredResearchBench == null || building_ResearchBench2.def == requiredResearchBench))
				{
					float researchBenchRequirementsScore = this.GetResearchBenchRequirementsScore(building_ResearchBench2, requiredFacilities);
					if (building_ResearchBench == null || researchBenchRequirementsScore > num)
					{
						num = researchBenchRequirementsScore;
						building_ResearchBench = building_ResearchBench2;
					}
				}
			}
			MainTabWindow_Research.tmpAllBuildings.Clear();
			return building_ResearchBench;
		}

		// Token: 0x06005CCD RID: 23757 RVA: 0x00203744 File Offset: 0x00201944
		private float GetResearchBenchRequirementsScore(Building_ResearchBench bench, List<ThingDef> requiredFacilities)
		{

			float num = 0f;
			int i;
			int j;
			for (i = 0; i < requiredFacilities.Count; i = j + 1)
			{
				CompAffectedByFacilities benchComp = bench.GetComp<CompAffectedByFacilities>();
				if (benchComp != null)
				{
					List<Thing> linkedFacilitiesListForReading = benchComp.LinkedFacilitiesListForReading;
					if (linkedFacilitiesListForReading.Find((Thing x) => x.def == requiredFacilities[i] && benchComp.IsFacilityActive(x)) != null)
					{
						num += 1f;
					}
					else if (linkedFacilitiesListForReading.Find((Thing x) => x.def == requiredFacilities[i]) != null)
					{
						num += 0.6f;
					}
				}
				j = i;
			}
			return num;
		}

		// Token: 0x0400328D RID: 12941
		protected ResearchProjectDef selectedProject;

		// Token: 0x0400328E RID: 12942
		private bool requiredByThisFound;

		// Token: 0x0400328F RID: 12943
		private Vector2 leftScrollPosition = Vector2.zero;

		// Token: 0x04003290 RID: 12944
		private float leftScrollViewHeight;

		// Token: 0x04003291 RID: 12945
		private Vector2 rightScrollPosition;

		// Token: 0x04003292 RID: 12946
		private float rightViewWidth;

		// Token: 0x04003293 RID: 12947
		private float rightViewHeight;

		// Token: 0x04003294 RID: 12948
		private ResearchTabDef curTabInt;

		// Token: 0x04003295 RID: 12949
		private bool editMode;

		// Token: 0x04003296 RID: 12950
		private List<ResearchProjectDef> draggingTabs = new List<ResearchProjectDef>();

		// Token: 0x04003297 RID: 12951
		private List<TabRecord> tabs = new List<TabRecord>();

		// Token: 0x04003298 RID: 12952
		private const float leftAreaWidthPercent = 0.22f;

		// Token: 0x04003299 RID: 12953
		private const float LeftAreaWidthMin = 200f;

		// Token: 0x0400329A RID: 12954
		private const int ModeSelectButHeight = 40;

		// Token: 0x0400329B RID: 12955
		private const float ProjectTitleHeight = 50f;

		// Token: 0x0400329C RID: 12956
		private const float ProjectTitleLeftMargin = 0f;

		// Token: 0x0400329D RID: 12957
		private const int ResearchItemW = 140;

		// Token: 0x0400329E RID: 12958
		private const int ResearchItemH = 50;

		// Token: 0x0400329F RID: 12959
		private const int ResearchItemPaddingW = 50;

		// Token: 0x040032A0 RID: 12960
		private const int ResearchItemPaddingH = 50;

		// Token: 0x040032A1 RID: 12961
		private const int ColumnMaxProjects = 6;

		// Token: 0x040032A2 RID: 12962
		private const float LineOffsetFactor = 0.48f;

		// Token: 0x040032A3 RID: 12963
		private const float IndentSpacing = 6f;

		// Token: 0x040032A4 RID: 12964
		private const float RowHeight = 24f;

		// Token: 0x040032A5 RID: 12965
		private const KeyCode SelectMultipleKey = KeyCode.LeftShift;

		// Token: 0x040032A6 RID: 12966
		private const KeyCode DeselectKey = KeyCode.LeftControl;

		// Token: 0x040032A7 RID: 12967
		private static readonly Texture2D ResearchBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.8f, 0.85f));

		// Token: 0x040032A8 RID: 12968
		private static readonly Texture2D ResearchBarBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f));

		// Token: 0x040032A9 RID: 12969
		private static readonly Color FulfilledPrerequisiteColor = Color.green;

		// Token: 0x040032AA RID: 12970
		private static readonly Color MissingPrerequisiteColor = ColoredText.RedReadable;

		// Token: 0x040032AB RID: 12971
		private static readonly Color ProjectWithMissingPrerequisiteLabelColor = Color.gray;

		// Token: 0x040032AC RID: 12972
		private static Dictionary<string, string> labelsWithNewlineCached = new Dictionary<string, string>();

		// Token: 0x040032AD RID: 12973
		private static Dictionary<Pair<int, int>, string> techprintsInfoCached = new Dictionary<Pair<int, int>, string>();

		// Token: 0x040032AE RID: 12974
		private static List<Building> tmpAllBuildings = new List<Building>();
	}
}
