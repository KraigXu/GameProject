using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000E98 RID: 3736
	[StaticConstructorOnStartup]
	public class LearningReadout : IExposable
	{
		// Token: 0x17001054 RID: 4180
		// (get) Token: 0x06005B0E RID: 23310 RVA: 0x001F554F File Offset: 0x001F374F
		public int ActiveConceptsCount
		{
			get
			{
				return this.activeConcepts.Count;
			}
		}

		// Token: 0x17001055 RID: 4181
		// (get) Token: 0x06005B0F RID: 23311 RVA: 0x001F555C File Offset: 0x001F375C
		public bool ShowAllMode
		{
			get
			{
				return this.showAllMode;
			}
		}

		// Token: 0x06005B10 RID: 23312 RVA: 0x001F5564 File Offset: 0x001F3764
		public LearningReadout()
		{
			this.windowOnGUICached = new Action(this.WindowOnGUI);
		}

		// Token: 0x06005B11 RID: 23313 RVA: 0x001F55B8 File Offset: 0x001F37B8
		public void ExposeData()
		{
			Scribe_Collections.Look<ConceptDef>(ref this.activeConcepts, "activeConcepts", LookMode.Undefined, Array.Empty<object>());
			Scribe_Defs.Look<ConceptDef>(ref this.selectedConcept, "selectedConcept");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.activeConcepts.RemoveAll((ConceptDef c) => PlayerKnowledgeDatabase.IsComplete(c));
			}
		}

		// Token: 0x06005B12 RID: 23314 RVA: 0x001F561E File Offset: 0x001F381E
		public bool TryActivateConcept(ConceptDef conc)
		{
			if (this.activeConcepts.Contains(conc))
			{
				return false;
			}
			this.activeConcepts.Add(conc);
			SoundDefOf.Lesson_Activated.PlayOneShotOnCamera(null);
			this.lastConceptActivateRealTime = RealTime.LastRealTime;
			return true;
		}

		// Token: 0x06005B13 RID: 23315 RVA: 0x001F5653 File Offset: 0x001F3853
		public bool IsActive(ConceptDef conc)
		{
			return this.activeConcepts.Contains(conc);
		}

		// Token: 0x06005B14 RID: 23316 RVA: 0x00002681 File Offset: 0x00000881
		public void LearningReadoutUpdate()
		{
		}

		// Token: 0x06005B15 RID: 23317 RVA: 0x001F5664 File Offset: 0x001F3864
		public void Notify_ConceptNewlyLearned(ConceptDef conc)
		{
			if (this.activeConcepts.Contains(conc) || this.selectedConcept == conc)
			{
				SoundDefOf.Lesson_Deactivated.PlayOneShotOnCamera(null);
				SoundDefOf.CommsWindow_Close.PlayOneShotOnCamera(null);
			}
			if (this.activeConcepts.Contains(conc))
			{
				this.activeConcepts.Remove(conc);
			}
			if (this.selectedConcept == conc)
			{
				this.selectedConcept = null;
			}
		}

		// Token: 0x06005B16 RID: 23318 RVA: 0x001F56C9 File Offset: 0x001F38C9
		private string FilterSearchStringInput(string input)
		{
			if (input == this.searchString)
			{
				return input;
			}
			if (input.Length > 20)
			{
				input = input.Substring(0, 20);
			}
			return input;
		}

		// Token: 0x06005B17 RID: 23319 RVA: 0x001F56F4 File Offset: 0x001F38F4
		public void LearningReadoutOnGUI()
		{
			if (TutorSystem.TutorialMode || !TutorSystem.AdaptiveTrainingEnabled)
			{
				return;
			}
			if (!Find.PlaySettings.showLearningHelper && this.activeConcepts.Count == 0)
			{
				return;
			}
			if (Find.WindowStack.IsOpen<Screen_Credits>())
			{
				return;
			}
			float b = (float)UI.screenHeight / 2f;
			float a = this.contentHeight + 14f;
			this.windowRect = new Rect((float)UI.screenWidth - 8f - 200f, 8f, 200f, Mathf.Min(a, b));
			Rect rect = this.windowRect;
			Find.WindowStack.ImmediateWindow(76136312, this.windowRect, WindowLayer.Super, this.windowOnGUICached, false, false, 1f);
			float num = Time.realtimeSinceStartup - this.lastConceptActivateRealTime;
			if (num < 1f && num > 0f)
			{
				GenUI.DrawFlash(rect.x, rect.center.y, (float)UI.screenWidth * 0.6f, Pulser.PulseBrightness(1f, 1f, num) * 0.85f, new Color(0.8f, 0.77f, 0.53f));
			}
			ConceptDef conceptDef = (this.selectedConcept != null) ? this.selectedConcept : this.mouseoverConcept;
			if (conceptDef != null)
			{
				this.DrawInfoPane(conceptDef);
				conceptDef.HighlightAllTags();
			}
			this.mouseoverConcept = null;
		}

		// Token: 0x06005B18 RID: 23320 RVA: 0x001F5848 File Offset: 0x001F3A48
		private void WindowOnGUI()
		{
			Rect rect = this.windowRect.AtZero().ContractedBy(7f);
			Rect viewRect = rect.AtZero();
			bool flag = this.contentHeight > rect.height;
			Widgets.DrawWindowBackgroundTutor(this.windowRect.AtZero());
			if (flag)
			{
				viewRect.height = this.contentHeight + 40f;
				viewRect.width -= 20f;
				this.scrollPosition = GUI.BeginScrollView(rect, this.scrollPosition, viewRect);
			}
			else
			{
				GUI.BeginGroup(rect);
			}
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(0f, 0f, viewRect.width - 24f, 24f);
			Widgets.Label(rect2, "LearningHelper".Translate());
			float num = rect2.yMax;
			if (Widgets.ButtonImage(new Rect(rect2.xMax, rect2.y, 24f, 24f), (!this.showAllMode) ? TexButton.Plus : TexButton.Minus, true))
			{
				this.showAllMode = !this.showAllMode;
				if (this.showAllMode)
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
			if (this.showAllMode)
			{
				Rect rect3 = new Rect(0f, num, viewRect.width - 20f - 2f, 28f);
				this.searchString = this.FilterSearchStringInput(Widgets.TextField(rect3, this.searchString));
				if (this.searchString == "")
				{
					GUI.color = new Color(0.6f, 0.6f, 0.6f, 1f);
					Text.Anchor = TextAnchor.MiddleLeft;
					Rect rect4 = rect3;
					rect4.xMin += 7f;
					Widgets.Label(rect4, "Filter".Translate() + "...");
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = Color.white;
				}
				if (Widgets.ButtonImage(new Rect(viewRect.width - 20f, num + 14f - 10f, 20f, 20f), TexButton.CloseXSmall, true))
				{
					this.searchString = "";
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				}
				num = rect3.yMax + 4f;
			}
			LearningReadout.tmpConceptsToShow.Clear();
			if (this.showAllMode)
			{
				LearningReadout.tmpConceptsToShow.AddRange(DefDatabase<ConceptDef>.AllDefsListForReading);
			}
			else
			{
				LearningReadout.tmpConceptsToShow.AddRange(this.activeConcepts);
			}
			if (LearningReadout.tmpConceptsToShow.Any<ConceptDef>())
			{
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				Widgets.DrawLineHorizontal(0f, num, viewRect.width);
				GUI.color = Color.white;
				num += 4f;
			}
			if (this.showAllMode)
			{
				LearningReadout.tmpConceptsToShow.SortBy((ConceptDef x) => -this.DisplayPriority(x), (ConceptDef x) => x.label);
			}
			for (int i = 0; i < LearningReadout.tmpConceptsToShow.Count; i++)
			{
				if (!LearningReadout.tmpConceptsToShow[i].TriggeredDirect)
				{
					num = this.DrawConceptListRow(0f, num, viewRect.width, LearningReadout.tmpConceptsToShow[i]).yMax;
				}
			}
			LearningReadout.tmpConceptsToShow.Clear();
			this.contentHeight = num;
			if (flag)
			{
				GUI.EndScrollView();
				return;
			}
			GUI.EndGroup();
		}

		// Token: 0x06005B19 RID: 23321 RVA: 0x001F5BD4 File Offset: 0x001F3DD4
		private int DisplayPriority(ConceptDef conc)
		{
			int num = 1;
			if (this.MatchesSearchString(conc))
			{
				num += 10000;
			}
			return num;
		}

		// Token: 0x06005B1A RID: 23322 RVA: 0x001F5BF5 File Offset: 0x001F3DF5
		private bool MatchesSearchString(ConceptDef conc)
		{
			return this.searchString != "" && conc.label.IndexOf(this.searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x06005B1B RID: 23323 RVA: 0x001F5C24 File Offset: 0x001F3E24
		private Rect DrawConceptListRow(float x, float y, float width, ConceptDef conc)
		{
			float knowledge = PlayerKnowledgeDatabase.GetKnowledge(conc);
			bool flag = PlayerKnowledgeDatabase.IsComplete(conc);
			object obj = !flag && knowledge > 0f;
			float num = Text.CalcHeight(conc.LabelCap, width);
			object obj2 = obj;
			if (obj2 != null)
			{
				num += 0f;
			}
			Rect rect = new Rect(x, y, width, num);
			if (obj2 != null)
			{
				Rect rect2 = new Rect(rect);
				rect2.yMin += 1f;
				rect2.yMax -= 1f;
				Widgets.FillableBar(rect2, PlayerKnowledgeDatabase.GetKnowledge(conc), LearningReadout.ProgressBarFillTex, LearningReadout.ProgressBarBGTex, false);
			}
			if (flag)
			{
				GUI.DrawTexture(rect, BaseContent.GreyTex);
			}
			if (this.selectedConcept == conc)
			{
				GUI.DrawTexture(rect, TexUI.HighlightSelectedTex);
			}
			Widgets.DrawHighlightIfMouseover(rect);
			if (this.MatchesSearchString(conc))
			{
				Widgets.DrawHighlight(rect);
			}
			Widgets.Label(rect, conc.LabelCap);
			if (Mouse.IsOver(rect) && this.selectedConcept == null)
			{
				this.mouseoverConcept = conc;
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				if (this.selectedConcept == conc)
				{
					this.selectedConcept = null;
				}
				else
				{
					this.selectedConcept = conc;
				}
				SoundDefOf.PageChange.PlayOneShotOnCamera(null);
			}
			return rect;
		}

		// Token: 0x06005B1C RID: 23324 RVA: 0x001F5D50 File Offset: 0x001F3F50
		private Rect DrawInfoPane(ConceptDef conc)
		{
			float knowledge = PlayerKnowledgeDatabase.GetKnowledge(conc);
			bool complete = PlayerKnowledgeDatabase.IsComplete(conc);
			bool drawProgressBar = !complete && knowledge > 0f;
			Text.Font = GameFont.Medium;
			float titleHeight = Text.CalcHeight(conc.LabelCap, 276f);
			Text.Font = GameFont.Small;
			float textHeight = Text.CalcHeight(conc.HelpTextAdjusted, 296f);
			float num = titleHeight + textHeight + 14f + 5f;
			if (this.selectedConcept == conc)
			{
				num += 40f;
			}
			if (drawProgressBar)
			{
				num += 30f;
			}
			Rect outRect = new Rect((float)UI.screenWidth - 8f - 200f - 8f - 310f, 8f, 310f, num);
			Rect outRect2 = outRect;
			Find.WindowStack.ImmediateWindow(987612111, outRect, WindowLayer.Super, delegate
			{
				outRect = outRect.AtZero();
				Rect rect = outRect.ContractedBy(7f);
				Widgets.DrawShadowAround(outRect);
				Widgets.DrawWindowBackgroundTutor(outRect);
				Rect rect2 = rect;
				rect2.width -= 20f;
				rect2.height = titleHeight + 5f;
				Text.Font = GameFont.Medium;
				Widgets.Label(rect2, conc.LabelCap);
				Text.Font = GameFont.Small;
				Rect rect3 = rect;
				rect3.yMin = rect2.yMax;
				rect3.height = textHeight;
				Widgets.Label(rect3, conc.HelpTextAdjusted);
				if (drawProgressBar)
				{
					Rect rect4 = rect;
					rect4.yMin = rect3.yMax;
					rect4.height = 30f;
					Widgets.FillableBar(rect4, PlayerKnowledgeDatabase.GetKnowledge(conc), LearningReadout.ProgressBarFillTex);
				}
				if (this.selectedConcept == conc)
				{
					if (Widgets.CloseButtonFor(outRect))
					{
						this.selectedConcept = null;
						SoundDefOf.PageChange.PlayOneShotOnCamera(null);
					}
					Rect rect5 = new Rect(rect.center.x - 70f, rect.yMax - 30f, 140f, 30f);
					if (!complete)
					{
						if (Widgets.ButtonText(rect5, "MarkLearned".Translate(), true, true, true))
						{
							this.selectedConcept = null;
							SoundDefOf.PageChange.PlayOneShotOnCamera(null);
							PlayerKnowledgeDatabase.SetKnowledge(conc, 1f);
							return;
						}
					}
					else
					{
						GUI.color = new Color(1f, 1f, 1f, 0.5f);
						Text.Anchor = TextAnchor.MiddleCenter;
						Widgets.Label(rect5, "AlreadyLearned".Translate());
						Text.Anchor = TextAnchor.UpperLeft;
						GUI.color = Color.white;
					}
				}
			}, false, false, 1f);
			return outRect2;
		}

		// Token: 0x040031B1 RID: 12721
		private List<ConceptDef> activeConcepts = new List<ConceptDef>();

		// Token: 0x040031B2 RID: 12722
		private ConceptDef selectedConcept;

		// Token: 0x040031B3 RID: 12723
		private bool showAllMode;

		// Token: 0x040031B4 RID: 12724
		private float contentHeight;

		// Token: 0x040031B5 RID: 12725
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x040031B6 RID: 12726
		private string searchString = "";

		// Token: 0x040031B7 RID: 12727
		private float lastConceptActivateRealTime = -999f;

		// Token: 0x040031B8 RID: 12728
		private ConceptDef mouseoverConcept;

		// Token: 0x040031B9 RID: 12729
		private Rect windowRect;

		// Token: 0x040031BA RID: 12730
		private Action windowOnGUICached;

		// Token: 0x040031BB RID: 12731
		private const float OuterMargin = 8f;

		// Token: 0x040031BC RID: 12732
		private const float InnerMargin = 7f;

		// Token: 0x040031BD RID: 12733
		private const float ReadoutWidth = 200f;

		// Token: 0x040031BE RID: 12734
		private const float InfoPaneWidth = 310f;

		// Token: 0x040031BF RID: 12735
		private const float OpenButtonSize = 24f;

		// Token: 0x040031C0 RID: 12736
		public static readonly Texture2D ProgressBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.745098054f, 0.6039216f, 0.2f));

		// Token: 0x040031C1 RID: 12737
		public static readonly Texture2D ProgressBarBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.509803951f, 0.407843143f, 0.13333334f));

		// Token: 0x040031C2 RID: 12738
		private static List<ConceptDef> tmpConceptsToShow = new List<ConceptDef>();
	}
}
