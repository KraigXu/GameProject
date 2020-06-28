using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000F26 RID: 3878
	public abstract class Lesson_Instruction : Lesson
	{
		// Token: 0x1700110D RID: 4365
		// (get) Token: 0x06005EF6 RID: 24310 RVA: 0x0020C6AF File Offset: 0x0020A8AF
		protected Map Map
		{
			get
			{
				return Find.AnyPlayerHomeMap;
			}
		}

		// Token: 0x1700110E RID: 4366
		// (get) Token: 0x06005EF7 RID: 24311 RVA: 0x0004E475 File Offset: 0x0004C675
		protected virtual float ProgressPercent
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x1700110F RID: 4367
		// (get) Token: 0x06005EF8 RID: 24312 RVA: 0x0020C6B6 File Offset: 0x0020A8B6
		protected virtual bool ShowProgressBar
		{
			get
			{
				return this.ProgressPercent >= 0f;
			}
		}

		// Token: 0x17001110 RID: 4368
		// (get) Token: 0x06005EF9 RID: 24313 RVA: 0x0020C6C8 File Offset: 0x0020A8C8
		public override string DefaultRejectInputMessage
		{
			get
			{
				return this.def.rejectInputMessage;
			}
		}

		// Token: 0x17001111 RID: 4369
		// (get) Token: 0x06005EFA RID: 24314 RVA: 0x0020C6D5 File Offset: 0x0020A8D5
		public override InstructionDef Instruction
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x06005EFB RID: 24315 RVA: 0x0020C6DD File Offset: 0x0020A8DD
		public override void ExposeData()
		{
			Scribe_Defs.Look<InstructionDef>(ref this.def, "def");
			base.ExposeData();
		}

		// Token: 0x06005EFC RID: 24316 RVA: 0x0020C6F8 File Offset: 0x0020A8F8
		public override void OnActivated()
		{
			base.OnActivated();
			if (this.def.giveOnActivateCount > 0)
			{
				Thing thing = ThingMaker.MakeThing(this.def.giveOnActivateDef, null);
				thing.stackCount = this.def.giveOnActivateCount;
				GenSpawn.Spawn(thing, TutorUtility.FindUsableRect(2, 2, this.Map, 0f, false).CenterCell, this.Map, WipeMode.Vanish);
			}
			if (this.def.resetBuildDesignatorStuffs)
			{
				foreach (DesignationCategoryDef designationCategoryDef in DefDatabase<DesignationCategoryDef>.AllDefs)
				{
					foreach (Designator designator in designationCategoryDef.ResolvedAllowedDesignators)
					{
						Designator_Build designator_Build = designator as Designator_Build;
						if (designator_Build != null)
						{
							designator_Build.ResetStuffToDefault();
						}
					}
				}
			}
		}

		// Token: 0x06005EFD RID: 24317 RVA: 0x0020C7EC File Offset: 0x0020A9EC
		public override void LessonOnGUI()
		{
			Text.Font = GameFont.Small;
			string textAdj = this.def.text.AdjustedForKeys(null, true);
			float num = Text.CalcHeight(textAdj, 290f) + 20f;
			if (this.ShowProgressBar)
			{
				num += 47f;
			}
			Vector2 vector = new Vector2((float)UI.screenWidth - 17f - 155f, 17f + num / 2f);
			if (!Find.TutorialState.introDone)
			{
				float screenOverlayAlpha = 0f;
				if (this.def.startCentered)
				{
					Vector2 vector2 = new Vector2((float)(UI.screenWidth / 2), (float)(UI.screenHeight / 2));
					if (base.AgeSeconds < 4f)
					{
						vector = vector2;
						screenOverlayAlpha = 0.9f;
					}
					else if (base.AgeSeconds < 5f)
					{
						float t = (base.AgeSeconds - 4f) / 1f;
						vector = Vector2.Lerp(vector2, vector, t);
						screenOverlayAlpha = Mathf.Lerp(0.9f, 0f, t);
					}
				}
				if (screenOverlayAlpha > 0f)
				{
					Rect fullScreenRect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
					Find.WindowStack.ImmediateWindow(972651, fullScreenRect, WindowLayer.SubSuper, delegate
					{
						GUI.color = new Color(1f, 1f, 1f, screenOverlayAlpha);
						GUI.DrawTexture(fullScreenRect, BaseContent.BlackTex);
						GUI.color = Color.white;
					}, false, true, 0f);
				}
				else
				{
					Find.TutorialState.introDone = true;
				}
			}
			Rect mainRect = new Rect(vector.x - 155f, vector.y - num / 2f - 10f, 310f, num);
			if (Find.TutorialState.introDone && Find.WindowStack.IsOpen<Page_ConfigureStartingPawns>())
			{
				Rect mainRect2 = mainRect;
				mainRect2.x = 17f;
				if ((mainRect.Contains(Event.current.mousePosition) || (this.def == InstructionDefOf.RandomizeCharacter && UI.screenHeight <= 768)) && !mainRect2.Contains(Event.current.mousePosition))
				{
					mainRect.x = 17f;
				}
			}
			Find.WindowStack.ImmediateWindow(177706, mainRect, WindowLayer.Super, delegate
			{
				Rect rect = mainRect.AtZero();
				Widgets.DrawWindowBackgroundTutor(rect);
				Rect rect2 = rect.ContractedBy(10f);
				Text.Font = GameFont.Small;
				Rect rect3 = rect2;
				if (this.ShowProgressBar)
				{
					rect3.height -= 47f;
				}
				Widgets.Label(rect3, textAdj);
				if (this.ShowProgressBar)
				{
					Widgets.FillableBar(new Rect(rect2.x, rect2.yMax - 30f, rect2.width, 30f), this.ProgressPercent, LearningReadout.ProgressBarFillTex);
				}
				if (this.AgeSeconds < 0.5f)
				{
					GUI.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, this.AgeSeconds / 0.5f));
					GUI.DrawTexture(rect, BaseContent.WhiteTex);
					GUI.color = Color.white;
				}
			}, false, false, 1f);
			if (this.def.highlightTags != null)
			{
				for (int i = 0; i < this.def.highlightTags.Count; i++)
				{
					UIHighlighter.HighlightTag(this.def.highlightTags[i]);
				}
			}
		}

		// Token: 0x06005EFE RID: 24318 RVA: 0x0020CAA7 File Offset: 0x0020ACA7
		public override void Notify_Event(EventPack ep)
		{
			if (this.def.eventTagsEnd != null && this.def.eventTagsEnd.Contains(ep.Tag))
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x06005EFF RID: 24319 RVA: 0x0020CAD9 File Offset: 0x0020ACD9
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			return this.def.actionTagsAllowed != null && this.def.actionTagsAllowed.Contains(ep.Tag);
		}

		// Token: 0x06005F00 RID: 24320 RVA: 0x0020CB08 File Offset: 0x0020AD08
		public override void PostDeactivated()
		{
			SoundDefOf.CommsWindow_Close.PlayOneShotOnCamera(null);
			TutorSystem.Notify_Event("InstructionDeactivated-" + this.def.defName);
			if (this.def.endTutorial)
			{
				Find.ActiveLesson.Deactivate();
				Find.TutorialState.Notify_TutorialEnding();
				LessonAutoActivator.Notify_TutorialEnding();
			}
		}

		// Token: 0x04003378 RID: 13176
		public InstructionDef def;

		// Token: 0x04003379 RID: 13177
		private const float RectWidth = 310f;

		// Token: 0x0400337A RID: 13178
		private const float BarHeight = 30f;
	}
}
