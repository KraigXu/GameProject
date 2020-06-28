using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EC8 RID: 3784
	[StaticConstructorOnStartup]
	public class MainTabWindow_Quests : MainTabWindow
	{
		// Token: 0x170010C1 RID: 4289
		// (get) Token: 0x06005C8A RID: 23690 RVA: 0x001FDEDF File Offset: 0x001FC0DF
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 640f);
			}
		}

		// Token: 0x06005C8B RID: 23691 RVA: 0x001FF3F8 File Offset: 0x001FD5F8
		public override void PreOpen()
		{
			base.PreOpen();
			if (MainTabWindow_Quests.RatingIcon == null)
			{
				MainTabWindow_Quests.RatingIcon = ContentFinder<Texture2D>.Get("UI/Icons/ChallengeRatingIcon", true);
			}
			this.tabs.Clear();
			this.tabs.Add(new TabRecord("AvailableQuests".Translate(), delegate
			{
				this.curTab = MainTabWindow_Quests.QuestsTab.Available;
				this.selected = null;
			}, () => this.curTab == MainTabWindow_Quests.QuestsTab.Available));
			this.tabs.Add(new TabRecord("ActiveQuests".Translate(), delegate
			{
				this.curTab = MainTabWindow_Quests.QuestsTab.Active;
				this.selected = null;
			}, () => this.curTab == MainTabWindow_Quests.QuestsTab.Active));
			this.tabs.Add(new TabRecord("HistoricalQuests".Translate(), delegate
			{
				this.curTab = MainTabWindow_Quests.QuestsTab.Historical;
				this.selected = null;
			}, () => this.curTab == MainTabWindow_Quests.QuestsTab.Historical));
			this.Select(this.selected);
		}

		// Token: 0x06005C8C RID: 23692 RVA: 0x001FF4E4 File Offset: 0x001FD6E4
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			Rect rect2 = rect;
			rect2.yMin += 4f;
			rect2.xMax = rect2.width * 0.36f;
			rect2.yMax -= this.DoRewardsPrefsButton(rect2).height + 4f;
			this.DoQuestsList(rect2);
			Rect rect3 = rect;
			rect3.yMin += 4f;
			rect3.xMin = rect2.xMax + 17f;
			this.DoSelectedQuestInfo(rect3);
		}

		// Token: 0x06005C8D RID: 23693 RVA: 0x001FF57C File Offset: 0x001FD77C
		[Obsolete]
		public Rect DoFavorCheckBoxes(Rect rect)
		{
			return default(Rect);
		}

		// Token: 0x06005C8E RID: 23694 RVA: 0x001FF594 File Offset: 0x001FD794
		public Rect DoRewardsPrefsButton(Rect rect)
		{
			rect.yMin = rect.yMax - 40f;
			Text.Font = GameFont.Small;
			if (Widgets.ButtonText(rect, "ChooseRewards".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_RewardPrefsConfig());
			}
			return rect;
		}

		// Token: 0x06005C8F RID: 23695 RVA: 0x001FF5E8 File Offset: 0x001FD7E8
		public void Select(Quest quest)
		{
			if (quest != this.selected)
			{
				this.selected = quest;
				this.selectedQuestScrollPosition = default(Vector2);
				this.selectedQuestLastHeight = 300f;
			}
			if (quest != null)
			{
				if (quest.dismissed)
				{
					this.curTab = MainTabWindow_Quests.QuestsTab.Historical;
					return;
				}
				if (quest.State == QuestState.NotYetAccepted)
				{
					this.curTab = MainTabWindow_Quests.QuestsTab.Available;
					return;
				}
				if (quest.State == QuestState.Ongoing)
				{
					this.curTab = MainTabWindow_Quests.QuestsTab.Active;
					return;
				}
				this.curTab = MainTabWindow_Quests.QuestsTab.Historical;
			}
		}

		// Token: 0x06005C90 RID: 23696 RVA: 0x001FF658 File Offset: 0x001FD858
		private void DoQuestsList(Rect rect)
		{
			Rect rect2 = rect;
			rect2.yMin += 32f;
			Widgets.DrawMenuSection(rect2);
			TabDrawer.DrawTabs(rect2, this.tabs, 200f);
			this.SortQuestsByTab();
			if (MainTabWindow_Quests.tmpQuestsToShow.Count != 0)
			{
				Rect rect3 = rect2;
				rect3 = rect3.ContractedBy(10f);
				rect3.xMax += 6f;
				Rect viewRect = new Rect(0f, 0f, rect3.width - 16f, (float)MainTabWindow_Quests.tmpQuestsToShow.Count * 32f);
				Vector2 vector = default(Vector2);
				switch (this.curTab)
				{
				case MainTabWindow_Quests.QuestsTab.Available:
					Widgets.BeginScrollView(rect3, ref this.scrollPosition_available, viewRect, true);
					vector = this.scrollPosition_available;
					break;
				case MainTabWindow_Quests.QuestsTab.Active:
					Widgets.BeginScrollView(rect3, ref this.scrollPosition_active, viewRect, true);
					vector = this.scrollPosition_active;
					break;
				case MainTabWindow_Quests.QuestsTab.Historical:
					Widgets.BeginScrollView(rect3, ref this.scrollPosition_historical, viewRect, true);
					vector = this.scrollPosition_historical;
					break;
				}
				float num = 0f;
				for (int i = 0; i < MainTabWindow_Quests.tmpQuestsToShow.Count; i++)
				{
					Quest quest = MainTabWindow_Quests.tmpQuestsToShow[i];
					float num2 = vector.y - 32f;
					float num3 = vector.y + rect3.height;
					if (num > num2 && num < num3)
					{
						this.DoRow(new Rect(0f, num, viewRect.width - 4f, 32f), quest);
					}
					num += 32f;
				}
				MainTabWindow_Quests.tmpQuestsToShow.Clear();
				Widgets.EndScrollView();
				return;
			}
			Widgets.NoneLabel(rect2.y + 17f, rect2.width, null);
		}

		// Token: 0x06005C91 RID: 23697 RVA: 0x001FF814 File Offset: 0x001FDA14
		private void SortQuestsByTab()
		{
			List<Quest> questsInDisplayOrder = Find.QuestManager.questsInDisplayOrder;
			MainTabWindow_Quests.tmpQuestsToShow.Clear();
			for (int i = 0; i < questsInDisplayOrder.Count; i++)
			{
				if (this.ShouldListNow(questsInDisplayOrder[i]))
				{
					MainTabWindow_Quests.tmpQuestsToShow.Add(questsInDisplayOrder[i]);
				}
			}
			switch (this.curTab)
			{
			case MainTabWindow_Quests.QuestsTab.Available:
				MainTabWindow_Quests.tmpQuestsToShow.SortBy((Quest q) => q.ticksUntilAcceptanceExpiry);
				return;
			case MainTabWindow_Quests.QuestsTab.Active:
				MainTabWindow_Quests.tmpQuestsToShow.SortBy((Quest q) => q.TicksSinceAccepted);
				return;
			case MainTabWindow_Quests.QuestsTab.Historical:
				MainTabWindow_Quests.tmpQuestsToShow.SortBy((Quest q) => q.TicksSinceCleanup);
				return;
			default:
				return;
			}
		}

		// Token: 0x06005C92 RID: 23698 RVA: 0x001FF900 File Offset: 0x001FDB00
		private void DoRow(Rect rect, Quest quest)
		{
			Rect rect2 = rect;
			rect2.width -= 95f;
			Rect rect3 = rect;
			rect3.xMax -= 4f;
			rect3.xMin = rect3.xMax - 35f;
			Rect rect4 = rect;
			rect4.xMax = rect3.xMin;
			rect4.xMin = rect4.xMax - 60f;
			if (this.selected == quest)
			{
				Widgets.DrawHighlightSelected(rect);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect5 = new Rect(rect2.x + 4f, rect2.y, rect2.width - 4f, rect2.height);
			Widgets.Label(rect5, quest.name.Truncate(rect5.width, null));
			string timeTip;
			Color color;
			string shortTimeInfo = this.GetShortTimeInfo(quest, out timeTip, out color);
			if (!shortTimeInfo.NullOrEmpty())
			{
				GUI.color = color;
				Text.Anchor = TextAnchor.MiddleRight;
				Widgets.Label(rect3, shortTimeInfo);
				GUI.color = Color.white;
				if (Mouse.IsOver(rect3))
				{
					TooltipHandler.TipRegion(rect3, () => quest.name + (timeTip.NullOrEmpty() ? "" : ("\n" + timeTip)), quest.id ^ 875632098);
					Widgets.DrawHighlight(rect3);
				}
			}
			if (quest.dismissed && !quest.Historical)
			{
				rect4.x -= 25f;
				Rect rect6 = new Rect(rect4.xMax + 5f, rect4.y + rect4.height / 2f - 7f, 15f, 15f);
				GUI.DrawTexture(rect6, MainTabWindow_Quests.QuestDismissedIcon);
				rect6.height = rect5.height;
				rect6.y = rect5.y;
				if (Mouse.IsOver(rect6))
				{
					TooltipHandler.TipRegion(rect6, "QuestDismissed".Translate());
					Widgets.DrawHighlight(rect6);
				}
			}
			for (int i = 0; i < quest.challengeRating; i++)
			{
				GUI.DrawTexture(new Rect(rect4.xMax - (float)(15 * (i + 1)), rect4.y + rect4.height / 2f - 7f, 15f, 15f), MainTabWindow_Quests.RatingIcon);
			}
			if (Mouse.IsOver(rect4))
			{
				TooltipHandler.TipRegion(rect4, "QuestChallengeRatingTip".Translate());
				Widgets.DrawHighlight(rect4);
			}
			GenUI.ResetLabelAlign();
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.Select(quest);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06005C93 RID: 23699 RVA: 0x001FFBB4 File Offset: 0x001FDDB4
		private string GetShortTimeInfo(Quest quest, out string tip, out Color color)
		{
			color = Color.gray;
			if (quest.State == QuestState.NotYetAccepted)
			{
				if (quest.ticksUntilAcceptanceExpiry >= 0)
				{
					color = ColoredText.RedReadable;
					tip = "QuestExpiresIn".Translate(quest.ticksUntilAcceptanceExpiry.ToStringTicksToPeriod(true, false, true, true));
					return quest.ticksUntilAcceptanceExpiry.ToStringTicksToPeriod(true, true, true, true);
				}
			}
			else
			{
				if (quest.Historical)
				{
					tip = "QuestFinishedAgo".Translate(quest.TicksSinceCleanup.ToStringTicksToPeriod(true, false, true, true));
					return quest.TicksSinceCleanup.ToStringTicksToPeriod(false, true, true, true);
				}
				if (quest.EverAccepted)
				{
					foreach (QuestPart questPart in quest.PartsListForReading)
					{
						QuestPart_Delay questPart_Delay = questPart as QuestPart_Delay;
						if (questPart_Delay != null && questPart_Delay.State == QuestPartState.Enabled && questPart_Delay.isBad && !questPart_Delay.expiryInfoPart.NullOrEmpty())
						{
							color = ColoredText.RedReadable;
							tip = "QuestExpiresIn".Translate(questPart_Delay.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
							return questPart_Delay.TicksLeft.ToStringTicksToPeriod(false, true, false, true);
						}
					}
					tip = this.GetAcceptedAgoByString(quest);
					return quest.TicksSinceAccepted.ToStringTicksToPeriod(false, true, true, true);
				}
			}
			tip = null;
			return null;
		}

		// Token: 0x06005C94 RID: 23700 RVA: 0x001FFD30 File Offset: 0x001FDF30
		private void DoSelectedQuestInfo(Rect rect)
		{
			Widgets.DrawMenuSection(rect);
			if (this.selected == null)
			{
				Widgets.NoneLabelCenteredVertically(rect, "(" + "NoQuestSelected".Translate() + ")");
				return;
			}
			Rect rect2 = rect.ContractedBy(17f);
			Rect outRect = rect2;
			Rect innerRect = new Rect(0f, 0f, outRect.width, this.selectedQuestLastHeight);
			Rect rect3 = new Rect(0f, 0f, outRect.width - 16f, this.selectedQuestLastHeight);
			Rect rect4 = rect3;
			bool flag = rect3.height > rect2.height;
			if (flag)
			{
				rect3.width -= 4f;
				rect4.width -= 16f;
			}
			Widgets.BeginScrollView(outRect, ref this.selectedQuestScrollPosition, rect3, true);
			float num = 0f;
			this.DoTitle(rect3, ref num);
			this.DoDismissButton(rect3, ref num);
			if (this.selected != null)
			{
				float curYBeforeAcceptButton = num;
				this.DoAcceptButton(rect3, ref num);
				this.DoRightAlignedInfo(rect3, ref num, curYBeforeAcceptButton);
				this.DoOutcomeInfo(rect3, ref num);
				this.DoDescription(rect3, ref num);
				this.DoAcceptanceRequirementInfo(innerRect, flag, ref num);
				this.DoRewards(rect3, ref num);
				this.DoLookTargets(rect3, ref num);
				this.DoSelectTargets(rect3, ref num);
				float num2 = num;
				this.DoDefHyperlinks(rect3, ref num);
				float num3 = num;
				num = num2;
				this.DoFactionInfo(rect4, ref num);
				this.DoDebugInfoToggle(rect3, ref num);
				if (num3 > num)
				{
					num = num3;
				}
				this.DoDebugInfo(rect3, ref num);
				this.selectedQuestLastHeight = num;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06005C95 RID: 23701 RVA: 0x001FFECC File Offset: 0x001FE0CC
		private void DoTitle(Rect innerRect, ref float curY)
		{
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(innerRect.x, curY, innerRect.width, 100f);
			Widgets.Label(rect, this.selected.name.Truncate(rect.width, null));
			Text.Font = GameFont.Small;
			curY += Text.LineHeight;
			curY += 17f;
		}

		// Token: 0x06005C96 RID: 23702 RVA: 0x001FFF34 File Offset: 0x001FE134
		private void DoDismissButton(Rect innerRect, ref float curY)
		{
			Rect rect = new Rect(innerRect.xMax - 32f - 4f, innerRect.y, 32f, 32f);
			Texture2D tex = (!this.selected.Historical && this.selected.dismissed) ? MainTabWindow_Quests.ResumeQuestIcon : MainTabWindow_Quests.DismissIcon;
			if (Widgets.ButtonImage(rect, tex, true))
			{
				if (this.selected.Historical)
				{
					this.selected.hiddenInUI = true;
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.Select(null);
					return;
				}
				this.selected.dismissed = !this.selected.dismissed;
				if (this.selected.dismissed)
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					this.SortQuestsByTab();
					this.selected = MainTabWindow_Quests.tmpQuestsToShow.FirstOrDefault((Quest x) => this.ShouldListNow(x));
					MainTabWindow_Quests.tmpQuestsToShow.Clear();
					return;
				}
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				this.Select(this.selected);
			}
			if (Mouse.IsOver(rect))
			{
				string key = this.selected.Historical ? "DeleteQuest" : (this.selected.dismissed ? "UnDismissQuest" : "DismissQuest");
				TooltipHandler.TipRegion(rect, key.Translate());
			}
		}

		// Token: 0x06005C97 RID: 23703 RVA: 0x00200088 File Offset: 0x001FE288
		private void DoDebugInfoToggle(Rect innerRect, ref float curY)
		{
			if (!Prefs.DevMode)
			{
				this.showDebugInfo = false;
				return;
			}
			Widgets.CheckboxLabeled(new Rect(innerRect.xMax - 110f, curY, 110f, 30f), "Dev: Debug", ref this.showDebugInfo, false, null, null, false);
			curY += 30f;
		}

		// Token: 0x06005C98 RID: 23704 RVA: 0x002000E0 File Offset: 0x001FE2E0
		private void DoAcceptButton(Rect innerRect, ref float curY)
		{
			QuestPart_Choice questPart_Choice = null;
			List<QuestPart> partsListForReading = this.selected.PartsListForReading;
			for (int i = 0; i < partsListForReading.Count; i++)
			{
				questPart_Choice = (partsListForReading[i] as QuestPart_Choice);
				if (questPart_Choice != null)
				{
					break;
				}
			}
			if (questPart_Choice != null && !Prefs.DevMode)
			{
				return;
			}
			curY += 17f;
			if (this.selected.State == QuestState.NotYetAccepted)
			{
				float num = innerRect.x;
				if (questPart_Choice == null)
				{
					Rect rect = new Rect(num, curY, 180f, 40f);
					if (!QuestUtility.CanAcceptQuest(this.selected))
					{
						GUI.color = Color.grey;
					}
					if (Widgets.ButtonText(rect, "AcceptQuest".Translate(), true, true, true))
					{
						this.AcceptQuestByInterface(null, this.selected.RequiresAccepter);
					}
					num += rect.width + 10f;
					GUI.color = Color.white;
				}
				if (Prefs.DevMode && Widgets.ButtonText(new Rect(num, curY, 180f, 40f), "Dev: Accept instantly", true, true, true))
				{
					SoundDefOf.Quest_Accepted.PlayOneShotOnCamera(null);
					if (questPart_Choice.choices.Any<QuestPart_Choice.Choice>())
					{
						questPart_Choice.Choose(questPart_Choice.choices.RandomElement<QuestPart_Choice.Choice>());
					}
					this.selected.Accept((from p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep
					where QuestUtility.CanPawnAcceptQuest(p, this.selected)
					select p).RandomElementWithFallback(null));
					this.selected.dismissed = false;
					this.Select(this.selected);
				}
				curY += 44f;
			}
		}

		// Token: 0x06005C99 RID: 23705 RVA: 0x0020025C File Offset: 0x001FE45C
		private void DoRightAlignedInfo(Rect innerRect, ref float curY, float curYBeforeAcceptButton)
		{
			bool flag = false;
			Vector2 locForDates = QuestUtility.GetLocForDates();
			float num = curYBeforeAcceptButton;
			if (!this.selected.initiallyAccepted && this.selected.EverAccepted)
			{
				if (!flag)
				{
					num += 17f;
					flag = true;
				}
				Rect rect = new Rect(innerRect.x, num, innerRect.width, 25f);
				GUI.color = MainTabWindow_Quests.TimeLimitColor;
				Text.Anchor = TextAnchor.MiddleRight;
				string text = this.selected.Historical ? this.GetAcceptedOnByString(this.selected) : this.GetAcceptedAgoByString(this.selected);
				Widgets.Label(rect, text);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				rect.xMin = rect.xMax - Text.CalcSize(text).x;
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, this.selected.Historical ? this.GetAcceptedAgoByString(this.selected) : this.GetAcceptedOnByString(this.selected));
				}
				if (this.selected.AccepterPawn != null && CameraJumper.CanJump(this.selected.AccepterPawn))
				{
					Widgets.DrawHighlightIfMouseover(rect);
					if (Widgets.ButtonInvisible(rect, true))
					{
						CameraJumper.TryJumpAndSelect(this.selected.AccepterPawn);
						Find.MainTabsRoot.EscapeCurrentTab(true);
					}
				}
				num += Text.LineHeight;
			}
			else if (this.selected.Historical)
			{
				if (!flag)
				{
					num += 17f;
					flag = true;
				}
				Rect rect2 = new Rect(innerRect.x, num, innerRect.width, 25f);
				GUI.color = MainTabWindow_Quests.TimeLimitColor;
				Text.Anchor = TextAnchor.MiddleRight;
				TaggedString taggedString = "AppearedOn".Translate(GenDate.DateFullStringWithHourAt((long)GenDate.TickGameToAbs(this.selected.appearanceTick), locForDates));
				Widgets.Label(rect2, taggedString);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				rect2.xMin = rect2.xMax - Text.CalcSize(taggedString).x;
				if (Mouse.IsOver(rect2))
				{
					TooltipHandler.TipRegion(rect2, "AppearedDaysAgo".Translate(((float)this.selected.TicksSinceAppeared / 60000f).ToString("0.#")));
				}
				num += Text.LineHeight;
			}
			if (this.selected.State == QuestState.NotYetAccepted && this.selected.ticksUntilAcceptanceExpiry > 0)
			{
				if (!flag)
				{
					num += 17f;
					flag = true;
				}
				Rect rect3 = new Rect(innerRect.x, num, innerRect.width, 25f);
				GUI.color = MainTabWindow_Quests.TimeLimitColor;
				Text.Anchor = TextAnchor.MiddleRight;
				string text2 = "QuestExpiresIn".Translate(this.selected.ticksUntilAcceptanceExpiry.ToStringTicksToPeriod(true, false, true, true));
				Widgets.Label(rect3, text2);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				rect3.xMin = rect3.xMax - Text.CalcSize(text2).x;
				if (Mouse.IsOver(rect3))
				{
					TooltipHandler.TipRegion(rect3, "QuestExpiresOn".Translate(GenDate.DateFullStringWithHourAt((long)(Find.TickManager.TicksAbs + this.selected.ticksUntilAcceptanceExpiry), locForDates)));
				}
				num += Text.LineHeight;
			}
			if (this.selected.State == QuestState.Ongoing)
			{
				this.tmpQuestParts.Clear();
				this.tmpQuestParts.AddRange(this.selected.PartsListForReading);
				this.tmpQuestParts.SortBy(delegate(QuestPart x)
				{
					if (!(x is QuestPartActivable))
					{
						return 0;
					}
					return ((QuestPartActivable)x).EnableTick;
				});
				for (int i = 0; i < this.tmpQuestParts.Count; i++)
				{
					QuestPartActivable questPartActivable = this.tmpQuestParts[i] as QuestPartActivable;
					if (questPartActivable != null && questPartActivable.State == QuestPartState.Enabled)
					{
						string expiryInfoPart = questPartActivable.ExpiryInfoPart;
						if (!expiryInfoPart.NullOrEmpty())
						{
							if (!flag)
							{
								num += 17f;
								flag = true;
							}
							Rect rect4 = new Rect(innerRect.x, num, innerRect.width, 25f);
							GUI.color = MainTabWindow_Quests.TimeLimitColor;
							Text.Anchor = TextAnchor.MiddleRight;
							Widgets.Label(rect4, expiryInfoPart);
							GUI.color = Color.white;
							Text.Anchor = TextAnchor.UpperLeft;
							rect4.xMin = rect4.xMax - Text.CalcSize(expiryInfoPart).x;
							if (Mouse.IsOver(rect4))
							{
								string expiryInfoPartTip = questPartActivable.ExpiryInfoPartTip;
								if (!expiryInfoPartTip.NullOrEmpty())
								{
									TooltipHandler.TipRegion(rect4, expiryInfoPartTip);
								}
							}
							num += Text.LineHeight;
						}
					}
				}
				this.tmpQuestParts.Clear();
			}
			curY = Mathf.Max(curY, num);
		}

		// Token: 0x06005C9A RID: 23706 RVA: 0x00200724 File Offset: 0x001FE924
		private void DoAcceptanceRequirementInfo(Rect innerRect, bool scrollBarVisible, ref float curY)
		{
			if (this.selected.EverAccepted)
			{
				return;
			}
			IEnumerable<string> enumerable = this.ListUnmetAcceptRequirements();
			int num = enumerable.Count<string>();
			if (num == 0)
			{
				return;
			}
			bool flag = num > 1;
			string text = "QuestAcceptanceRequirementsDescription".Translate() + (flag ? ": " : " ") + (flag ? ("\n" + enumerable.ToLineList("  - ", true)) : enumerable.First<string>());
			curY += 17f;
			float num2 = 0f;
			float x = innerRect.x + 8f;
			float num3 = innerRect.width - 16f;
			if (scrollBarVisible)
			{
				num3 -= 31f;
			}
			Rect rect = new Rect(x, curY, num3, 10000f);
			num2 += Text.CalcHeight(text, rect.width);
			Rect rect2 = new Rect(x, curY, num3, num2).ExpandedBy(8f);
			Widgets.DrawBoxSolid(rect2, MainTabWindow_Quests.acceptanceRequirementsBoxBgColor);
			GUI.color = MainTabWindow_Quests.AcceptanceRequirementsColor;
			Widgets.Label(rect, text);
			GUI.color = MainTabWindow_Quests.AcceptanceRequirementsBoxColor;
			Widgets.DrawBox(rect2, 2);
			curY += num2;
			GUI.color = Color.white;
		}

		// Token: 0x06005C9B RID: 23707 RVA: 0x00200855 File Offset: 0x001FEA55
		private IEnumerable<string> ListUnmetAcceptRequirements()
		{
			int num;
			for (int i = 0; i < this.selected.PartsListForReading.Count; i = num + 1)
			{
				QuestPart_RequirementsToAccept questPart_RequirementsToAccept = this.selected.PartsListForReading[i] as QuestPart_RequirementsToAccept;
				if (questPart_RequirementsToAccept != null)
				{
					AcceptanceReport acceptanceReport = questPart_RequirementsToAccept.CanAccept();
					if (!acceptanceReport.Accepted)
					{
						yield return acceptanceReport.Reason;
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06005C9C RID: 23708 RVA: 0x00200868 File Offset: 0x001FEA68
		private void DoOutcomeInfo(Rect innerRect, ref float curY)
		{
			if (!this.selected.Historical)
			{
				return;
			}
			string text;
			if (this.selected.State == QuestState.EndedOfferExpired)
			{
				text = "QuestOutcomeInfo_OfferExpired".Translate();
			}
			else if (this.selected.State == QuestState.EndedUnknownOutcome || this.selected.State == QuestState.EndedSuccess)
			{
				text = "QuestOutcomeInfo_UnknownOrSuccess".Translate();
			}
			else if (this.selected.State == QuestState.EndedFailed)
			{
				text = "QuestOutcomeInfo_Failed".Translate();
			}
			else if (this.selected.State == QuestState.EndedInvalid)
			{
				text = "QuestOutcomeInfo_Invalid".Translate();
			}
			else
			{
				text = null;
			}
			if (!text.NullOrEmpty())
			{
				curY += 17f;
				Widgets.Label(new Rect(innerRect.x, curY, innerRect.width, 25f), text);
				curY += Text.LineHeight;
			}
		}

		// Token: 0x06005C9D RID: 23709 RVA: 0x00200950 File Offset: 0x001FEB50
		private void DoDescription(Rect innerRect, ref float curY)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!this.selected.description.RawText.NullOrEmpty())
			{
				string value = this.selected.description.Resolve();
				stringBuilder.Append(value);
			}
			this.tmpQuestParts.Clear();
			this.tmpQuestParts.AddRange(this.selected.PartsListForReading);
			this.tmpQuestParts.SortBy(delegate(QuestPart x)
			{
				if (!(x is QuestPartActivable))
				{
					return 0;
				}
				return ((QuestPartActivable)x).EnableTick;
			});
			for (int i = 0; i < this.tmpQuestParts.Count; i++)
			{
				QuestPartActivable questPartActivable = this.tmpQuestParts[i] as QuestPartActivable;
				if (questPartActivable == null || questPartActivable.State == QuestPartState.Enabled)
				{
					string descriptionPart = this.tmpQuestParts[i].DescriptionPart;
					if (!descriptionPart.NullOrEmpty())
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.AppendLine();
							stringBuilder.AppendLine();
						}
						stringBuilder.Append(descriptionPart);
					}
				}
			}
			this.tmpQuestParts.Clear();
			if (stringBuilder.Length == 0)
			{
				return;
			}
			curY += 17f;
			Rect rect = new Rect(innerRect.x, curY, innerRect.width, 10000f);
			Widgets.Label(rect, stringBuilder.ToString());
			curY += Text.CalcHeight(stringBuilder.ToString(), rect.width);
		}

		// Token: 0x06005C9E RID: 23710 RVA: 0x00200AB0 File Offset: 0x001FECB0
		private void DoRewards(Rect innerRect, ref float curY)
		{
			QuestPart_Choice choice = null;
			List<QuestPart> partsListForReading = this.selected.PartsListForReading;
			for (int i = 0; i < partsListForReading.Count; i++)
			{
				choice = (partsListForReading[i] as QuestPart_Choice);
				if (choice != null)
				{
					break;
				}
			}
			if (choice == null)
			{
				return;
			}
			bool flag = this.selected.State == QuestState.NotYetAccepted;
			bool flag2 = true;
			if (Event.current.type == EventType.Layout)
			{
				MainTabWindow_Quests.layoutRewardsRects.Clear();
			}
			for (int j = 0; j < choice.choices.Count; j++)
			{
				MainTabWindow_Quests.tmpStackElements.Clear();
				float num = 0f;
				for (int k = 0; k < choice.choices[j].rewards.Count; k++)
				{
					MainTabWindow_Quests.tmpStackElements.AddRange(choice.choices[j].rewards[k].StackElements);
					num += choice.choices[j].rewards[k].TotalMarketValue;
				}
				if (MainTabWindow_Quests.tmpStackElements.Any<GenUI.AnonymousStackElement>())
				{
					if (num > 0f)
					{
						TaggedString totalValueStr = "TotalValue".Translate(num.ToStringMoney("F0"));
						MainTabWindow_Quests.tmpStackElements.Add(new GenUI.AnonymousStackElement
						{
							drawer = delegate(Rect r)
							{
								GUI.color = new Color(0.7f, 0.7f, 0.7f);
								Widgets.Label(new Rect(r.x + 5f, r.y, r.width - 10f, r.height), totalValueStr);
								GUI.color = Color.white;
							},
							width = Text.CalcSize(totalValueStr).x + 10f
						});
					}
					if (flag2)
					{
						curY += 17f;
						flag2 = false;
					}
					else
					{
						curY += 10f;
					}
					Rect rect = new Rect(innerRect.x, curY, innerRect.width, 10000f);
					Rect rect2 = rect.ContractedBy(10f);
					if (flag)
					{
						rect2.xMin += 100f;
					}
					if (j < MainTabWindow_Quests.layoutRewardsRects.Count)
					{
						Widgets.DrawBoxSolid(MainTabWindow_Quests.layoutRewardsRects[j], new Color(0.13f, 0.13f, 0.13f));
						GUI.color = new Color(1f, 1f, 1f, 0.3f);
						Widgets.DrawHighlightIfMouseover(MainTabWindow_Quests.layoutRewardsRects[j]);
						GUI.color = Color.white;
					}
					rect.height = GenUI.DrawElementStack<GenUI.AnonymousStackElement>(rect2, 24f, MainTabWindow_Quests.tmpStackElements, delegate(Rect r, GenUI.AnonymousStackElement obj)
					{
						obj.drawer(r);
					}, (GenUI.AnonymousStackElement obj) => obj.width, 4f, 5f, false).height + 20f;
					if (Event.current.type == EventType.Layout)
					{
						MainTabWindow_Quests.layoutRewardsRects.Add(rect);
					}
					if (flag)
					{
						if (!QuestUtility.CanAcceptQuest(this.selected))
						{
							GUI.color = Color.grey;
						}
						Rect rect3 = new Rect(rect.x, rect.y, 100f, rect.height);
						if (Widgets.ButtonText(rect3, "AcceptQuestFor".Translate() + ":", true, true, true))
						{
							MainTabWindow_Quests.tmpRemainingQuestParts.Clear();
							MainTabWindow_Quests.tmpRemainingQuestParts.AddRange(this.selected.PartsListForReading);
							for (int l = 0; l < choice.choices.Count; l++)
							{
								if (j != l)
								{
									for (int m = 0; m < choice.choices[l].questParts.Count; m++)
									{
										QuestPart item = choice.choices[l].questParts[m];
										if (!choice.choices[j].questParts.Contains(item))
										{
											MainTabWindow_Quests.tmpRemainingQuestParts.Remove(item);
										}
									}
								}
							}
							bool requiresAccepter = false;
							for (int n = 0; n < MainTabWindow_Quests.tmpRemainingQuestParts.Count; n++)
							{
								if (MainTabWindow_Quests.tmpRemainingQuestParts[n].RequiresAccepter)
								{
									requiresAccepter = true;
									break;
								}
							}
							MainTabWindow_Quests.tmpRemainingQuestParts.Clear();
							QuestPart_Choice.Choice localChoice = choice.choices[j];
							this.AcceptQuestByInterface(delegate
							{
								choice.Choose(localChoice);
							}, requiresAccepter);
						}
						TooltipHandler.TipRegionByKey(rect3, "AcceptQuestForTip");
						GUI.color = Color.white;
					}
					curY += rect.height;
				}
			}
			if (Event.current.type == EventType.Repaint)
			{
				MainTabWindow_Quests.layoutRewardsRects.Clear();
			}
			MainTabWindow_Quests.tmpStackElements.Clear();
		}

		// Token: 0x06005C9F RID: 23711 RVA: 0x00200FCC File Offset: 0x001FF1CC
		private void DoLookTargets(Rect innerRect, ref float curY)
		{
			List<Map> maps = Find.Maps;
			int num = 0;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num++;
				}
			}
			MainTabWindow_Quests.tmpLookTargets.Clear();
			MainTabWindow_Quests.tmpLookTargets.AddRange(this.selected.QuestLookTargets);
			MainTabWindow_Quests.tmpLookTargets.SortBy(delegate(GlobalTargetInfo x)
			{
				if (x.Thing is Pawn)
				{
					return 0;
				}
				if (x.HasThing)
				{
					return 1;
				}
				if (!x.IsWorldTarget)
				{
					return 2;
				}
				if (!(x.WorldObject is Settlement) || ((Settlement)x.WorldObject).Faction != Faction.OfPlayer)
				{
					return 3;
				}
				return 4;
			}, (GlobalTargetInfo x) => x.Label);
			bool flag = false;
			for (int j = 0; j < MainTabWindow_Quests.tmpLookTargets.Count; j++)
			{
				GlobalTargetInfo globalTargetInfo = MainTabWindow_Quests.tmpLookTargets[j];
				if (globalTargetInfo.HasWorldObject)
				{
					MapParent mapParent = globalTargetInfo.WorldObject as MapParent;
					if (mapParent != null && (!mapParent.HasMap || !mapParent.Map.IsPlayerHome))
					{
						flag = true;
						break;
					}
				}
			}
			bool flag2 = false;
			for (int k = 0; k < MainTabWindow_Quests.tmpLookTargets.Count; k++)
			{
				GlobalTargetInfo globalTargetInfo2 = MainTabWindow_Quests.tmpLookTargets[k];
				if (CameraJumper.CanJump(globalTargetInfo2) && (num != 1 || !(globalTargetInfo2 == Find.AnyPlayerHomeMap.Parent) || flag))
				{
					if (!flag2)
					{
						flag2 = true;
						curY += 17f;
					}
					if (Widgets.ButtonText(new Rect(innerRect.x, curY, innerRect.width, 25f), "JumpToTargetCustom".Translate(globalTargetInfo2.Label), false, true, true))
					{
						CameraJumper.TryJumpAndSelect(globalTargetInfo2);
						Find.MainTabsRoot.EscapeCurrentTab(true);
					}
					curY += 25f;
				}
			}
		}

		// Token: 0x06005CA0 RID: 23712 RVA: 0x00201194 File Offset: 0x001FF394
		private void DoSelectTargets(Rect innerRect, ref float curY)
		{
			bool flag = false;
			for (int i = 0; i < this.selected.PartsListForReading.Count; i++)
			{
				QuestPart questPart = this.selected.PartsListForReading[i];
				MainTabWindow_Quests.tmpSelectTargets.Clear();
				MainTabWindow_Quests.tmpSelectTargets.AddRange(questPart.QuestSelectTargets);
				if (MainTabWindow_Quests.tmpSelectTargets.Count != 0)
				{
					if (!flag)
					{
						flag = true;
						curY += 4f;
					}
					if (Widgets.ButtonText(new Rect(innerRect.x, curY, innerRect.width, 25f), questPart.QuestSelectTargetsLabel, false, true, true))
					{
						Map map = null;
						int num = 0;
						Vector3 a = Vector3.zero;
						Find.Selector.ClearSelection();
						for (int j = 0; j < MainTabWindow_Quests.tmpSelectTargets.Count; j++)
						{
							GlobalTargetInfo target = MainTabWindow_Quests.tmpSelectTargets[j];
							if (CameraJumper.CanJump(target) && target.HasThing)
							{
								Find.Selector.Select(target.Thing, true, true);
								if (map == null)
								{
									map = target.Map;
								}
								else if (target.Map != map)
								{
									num = 0;
									break;
								}
								a += target.Cell.ToVector3();
								num++;
							}
						}
						if (num > 0)
						{
							CameraJumper.TryJump(new IntVec3(a / (float)num), map);
						}
						Find.MainTabsRoot.EscapeCurrentTab(true);
					}
					curY += 25f;
				}
			}
		}

		// Token: 0x06005CA1 RID: 23713 RVA: 0x0020130C File Offset: 0x001FF50C
		private void DoFactionInfo(Rect rect, ref float curY)
		{
			curY += 15f;
			foreach (Faction faction in this.selected.InvolvedFactions)
			{
				if (faction != null && !faction.def.hidden && !faction.IsPlayer)
				{
					FactionUIUtility.DrawRelatedFactionInfo(rect, faction, ref curY);
				}
			}
		}

		// Token: 0x06005CA2 RID: 23714 RVA: 0x00201384 File Offset: 0x001FF584
		private void DoDefHyperlinks(Rect rect, ref float curY)
		{
			curY += 25f;
			foreach (Dialog_InfoCard.Hyperlink hyperlink in this.selected.Hyperlinks)
			{
				float num = Text.CalcHeight(hyperlink.Label, rect.width);
				Widgets.HyperlinkWithIcon(new Rect(rect.x, curY, rect.width / 2f, num), hyperlink, "ViewHyperlink".Translate(hyperlink.Label), 2f, 6f);
				curY += num;
			}
		}

		// Token: 0x06005CA3 RID: 23715 RVA: 0x0020143C File Offset: 0x001FF63C
		private void DoDebugInfo(Rect innerRect, ref float curY)
		{
			if (!this.showDebugInfo)
			{
				return;
			}
			curY += 17f;
			List<QuestPart> partsListForReading = this.selected.PartsListForReading;
			if (this.selected.State == QuestState.Ongoing)
			{
				for (int i = 0; i < partsListForReading.Count; i++)
				{
					partsListForReading[i].DoDebugWindowContents(innerRect, ref curY);
				}
			}
			if (this.selected.State == QuestState.Ongoing)
			{
				Rect rect = new Rect(innerRect.x, curY, 210f, 25f);
				this.debugSendSignalTextField = Widgets.TextField(rect, this.debugSendSignalTextField);
				Rect rect2 = new Rect(innerRect.x + rect.width + 4f, curY, 117f, 25f);
				if (Widgets.ButtonText(rect2, "Send signal", true, true, true))
				{
					Find.SignalManager.SendSignal(new Signal(this.debugSendSignalTextField));
					this.debugSendSignalTextField = "";
				}
				if (Widgets.ButtonText(new Rect(rect2.xMax + 4f, curY, 165f, 25f), "Send defined signal...", true, true, true))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (string signalLocal2 in from x in this.DebugPossibleSignals(this.selected).Distinct<string>()
					orderby x
					select x)
					{
						string signalLocal = signalLocal2;
						list.Add(new FloatMenuOption(signalLocal, delegate
						{
							Find.SignalManager.SendSignal(new Signal(signalLocal));
							this.debugSendSignalTextField = "";
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				curY += rect.height + 4f;
			}
			string text = "-----------------";
			text = text + "\nId: " + this.selected.id;
			text = text + "\nState: " + this.selected.State;
			text += "\nData:";
			text = text + "\n" + Scribe.saver.DebugOutputFor(this.selected);
			text += "\n";
			text += "\nActive QuestParts:";
			bool flag = false;
			for (int j = 0; j < partsListForReading.Count; j++)
			{
				QuestPartActivable questPartActivable = partsListForReading[j] as QuestPartActivable;
				if (questPartActivable != null && questPartActivable.State == QuestPartState.Enabled)
				{
					text = text + "\n" + questPartActivable.ToString();
					flag = true;
				}
			}
			if (!flag)
			{
				text += "\nNone";
			}
			Rect rect3 = new Rect(innerRect.x, curY, 180f, 40f);
			if (Widgets.ButtonText(rect3, "Copy debug to clipboard", true, true, true))
			{
				GUIUtility.systemCopyBuffer = text;
			}
			curY += rect3.height + 4f;
			Widgets.LongLabel(innerRect.x, innerRect.width, text, ref curY, true);
		}

		// Token: 0x06005CA4 RID: 23716 RVA: 0x0020176C File Offset: 0x001FF96C
		private bool ShouldListNow(Quest quest)
		{
			switch (this.curTab)
			{
			case MainTabWindow_Quests.QuestsTab.Available:
				return quest.State == QuestState.NotYetAccepted && !quest.dismissed && !quest.hiddenInUI;
			case MainTabWindow_Quests.QuestsTab.Active:
				return quest.State == QuestState.Ongoing && !quest.dismissed && !quest.hiddenInUI;
			case MainTabWindow_Quests.QuestsTab.Historical:
				return !quest.hiddenInUI && (quest.Historical || quest.dismissed);
			default:
				return false;
			}
		}

		// Token: 0x06005CA5 RID: 23717 RVA: 0x002017E9 File Offset: 0x001FF9E9
		private IEnumerable<string> DebugPossibleSignals(Quest quest)
		{
			string input = Scribe.saver.DebugOutputFor(this.selected);
			foreach (object obj in Regex.Matches(input, ">(Quest" + quest.id + "\\.[a-zA-Z0-9/\\-\\.]*)<"))
			{
				string value = ((Match)obj).Groups[1].Value;
				yield return value;
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		private string GetAcceptedAgoByString(Quest quest)
		{
			string value = quest.TicksSinceAccepted.ToStringTicksToPeriod(true, false, true, true);
			if (!quest.AccepterPawnLabelCap.NullOrEmpty())
			{
				return "AcceptedAgoBy".Translate(value, quest.AccepterPawnLabelCap);
			}
			return "AcceptedAgo".Translate(value);
		}


		private string GetAcceptedOnByString(Quest quest)
		{
			Vector2 locForDates = QuestUtility.GetLocForDates();
			string value = GenDate.DateFullStringWithHourAt((long)GenDate.TickGameToAbs(quest.acceptanceTick), locForDates);
			if (!quest.AccepterPawnLabelCap.NullOrEmpty())
			{
				return "AcceptedOnBy".Translate(value, quest.AccepterPawnLabelCap);
			}
			return "AcceptedOn".Translate(value);
		}


		private void AcceptQuestByInterface(Action preAcceptAction = null, bool requiresAccepter = false)
		{
			MainTabWindow_Quests.<>c__DisplayClass66_0 <>c__DisplayClass66_ = new MainTabWindow_Quests.<>c__DisplayClass66_0();
			<>c__DisplayClass66_.<>4__this = this;
			<>c__DisplayClass66_.preAcceptAction = preAcceptAction;
			if (!QuestUtility.CanAcceptQuest(this.selected))
			{
				Messages.Message("MessageCannotAcceptQuest".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (!requiresAccepter)
			{
				SoundDefOf.Quest_Accepted.PlayOneShotOnCamera(null);
				if (<>c__DisplayClass66_.preAcceptAction != null)
				{
					<>c__DisplayClass66_.preAcceptAction();
				}
				this.selected.Accept(null);
				this.Select(this.selected);
				return;
			}
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			using (List<Pawn>.Enumerator enumerator = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MainTabWindow_Quests.<>c__DisplayClass66_1 <>c__DisplayClass66_2 = new MainTabWindow_Quests.<>c__DisplayClass66_1();
					<>c__DisplayClass66_2.CS$<>8__locals1 = <>c__DisplayClass66_;
					<>c__DisplayClass66_2.p = enumerator.Current;
					MainTabWindow_Quests.<>c__DisplayClass66_2 <>c__DisplayClass66_3 = new MainTabWindow_Quests.<>c__DisplayClass66_2();
					<>c__DisplayClass66_3.CS$<>8__locals2 = <>c__DisplayClass66_2;
					if (QuestUtility.CanPawnAcceptQuest(<>c__DisplayClass66_3.CS$<>8__locals2.p, this.selected))
					{
						<>c__DisplayClass66_3.pLocal = <>c__DisplayClass66_3.CS$<>8__locals2.p;
						string text = "AcceptWith".Translate(<>c__DisplayClass66_3.CS$<>8__locals2.p);
						if (<>c__DisplayClass66_3.CS$<>8__locals2.p.royalty != null && <>c__DisplayClass66_3.CS$<>8__locals2.p.royalty.AllTitlesInEffectForReading.Any<RoyalTitle>())
						{
							text = text + " (" + <>c__DisplayClass66_3.CS$<>8__locals2.p.royalty.MostSeniorTitle.def.GetLabelFor(<>c__DisplayClass66_3.pLocal) + ")";
						}
						list.Add(new FloatMenuOption(text, delegate
						{
							if (!QuestUtility.CanPawnAcceptQuest(<>c__DisplayClass66_3.pLocal, <>c__DisplayClass66_3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.selected))
							{
								return;
							}
							QuestPart_GiveRoyalFavor questPart_GiveRoyalFavor = <>c__DisplayClass66_3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.selected.PartsListForReading.OfType<QuestPart_GiveRoyalFavor>().FirstOrDefault<QuestPart_GiveRoyalFavor>();
							if (questPart_GiveRoyalFavor == null || !questPart_GiveRoyalFavor.giveToAccepter)
							{
								<>c__DisplayClass66_3.<AcceptQuestByInterface>g__AcceptAction|1();
								return;
							}
							IEnumerable<Trait> conceitedTraits = RoyalTitleUtility.GetConceitedTraits(<>c__DisplayClass66_3.CS$<>8__locals2.p);
							IEnumerable<Trait> traitsAffectingPsylinkNegatively = RoyalTitleUtility.GetTraitsAffectingPsylinkNegatively(<>c__DisplayClass66_3.CS$<>8__locals2.p);
							bool totallyDisabled = <>c__DisplayClass66_3.CS$<>8__locals2.p.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
							bool flag = conceitedTraits.Any<Trait>();
							bool flag2 = !<>c__DisplayClass66_3.CS$<>8__locals2.p.HasPsylink && traitsAffectingPsylinkNegatively.Any<Trait>();
							if (totallyDisabled || flag || flag2)
							{
								NamedArgument arg = <>c__DisplayClass66_3.CS$<>8__locals2.p.Named("PAWN");
								NamedArgument arg2 = questPart_GiveRoyalFavor.faction.Named("FACTION");
								TaggedString t4 = null;
								if (totallyDisabled)
								{
									t4 = "RoyalIncapableOfSocial".Translate(arg, arg2);
								}
								TaggedString t2 = null;
								if (flag)
								{
									t2 = "RoyalWithConceitedTrait".Translate(arg, arg2, (from t in conceitedTraits
									select t.CurrentData.label).ToCommaList(true));
								}
								TaggedString t3 = null;
								if (flag2)
								{
									t3 = "RoyalWithTraitAffectingPsylinkNegatively".Translate(arg, arg2, (from t in traitsAffectingPsylinkNegatively
									select t.Label).ToCommaList(true));
								}
								TaggedString taggedString = "QuestGivesRoyalFavor".Translate(arg, arg2);
								if (totallyDisabled)
								{
									taggedString += "\n\n" + t4;
								}
								if (flag)
								{
									taggedString += "\n\n" + t2;
								}
								if (flag2)
								{
									taggedString += "\n\n" + t3;
								}
								taggedString += "\n\n" + "WantToContinue".Translate();
								Find.WindowStack.Add(new Dialog_MessageBox(taggedString, "Confirm".Translate(), delegate
								{
									SoundDefOf.Quest_Accepted.PlayOneShotOnCamera(null);
									if (<>c__DisplayClass66_3.CS$<>8__locals2.CS$<>8__locals1.preAcceptAction != null)
									{
										<>c__DisplayClass66_3.CS$<>8__locals2.CS$<>8__locals1.preAcceptAction();
									}
									<>c__DisplayClass66_3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.selected.Accept(<>c__DisplayClass66_3.pLocal);
									<>c__DisplayClass66_3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.Select(<>c__DisplayClass66_3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.selected);
									Messages.Message("MessageQuestAccepted".Translate(<>c__DisplayClass66_3.pLocal, <>c__DisplayClass66_3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.selected.name), <>c__DisplayClass66_3.pLocal, MessageTypeDefOf.TaskCompletion, false);
								}, "GoBack".Translate(), null, null, false, null, null));
								return;
							}
							<>c__DisplayClass66_3.<AcceptQuestByInterface>g__AcceptAction|1();
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
			}
			if (list.Count > 0)
			{
				Find.WindowStack.Add(new FloatMenu(list));
				return;
			}
			Messages.Message("MessageNoColonistCanAcceptQuest".Translate(Faction.OfPlayer.def.pawnsPlural), MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x0400326B RID: 12907
		private Quest selected;

		// Token: 0x0400326C RID: 12908
		private MainTabWindow_Quests.QuestsTab curTab;

		// Token: 0x0400326D RID: 12909
		private List<TabRecord> tabs = new List<TabRecord>();

		// Token: 0x0400326E RID: 12910
		private Vector2 scrollPosition_available;

		// Token: 0x0400326F RID: 12911
		private Vector2 scrollPosition_active;

		// Token: 0x04003270 RID: 12912
		private Vector2 scrollPosition_historical;

		// Token: 0x04003271 RID: 12913
		private Vector2 selectedQuestScrollPosition;

		// Token: 0x04003272 RID: 12914
		private float selectedQuestLastHeight;

		// Token: 0x04003273 RID: 12915
		private bool showDebugInfo;

		// Token: 0x04003274 RID: 12916
		private List<QuestPart> tmpQuestParts = new List<QuestPart>();

		// Token: 0x04003275 RID: 12917
		private string debugSendSignalTextField;

		// Token: 0x04003276 RID: 12918
		private const float LeftRectWidthFraction = 0.36f;

		// Token: 0x04003277 RID: 12919
		private const float RowHeight = 32f;

		// Token: 0x04003278 RID: 12920
		private const float CheckBoxHeight = 24f;

		// Token: 0x04003279 RID: 12921
		private const float ShowDebugInfoToggleWidth = 110f;

		// Token: 0x0400327A RID: 12922
		private const float DismisIconWidth = 32f;

		// Token: 0x0400327B RID: 12923
		private const float TimeInfoWidth = 35f;

		// Token: 0x0400327C RID: 12924
		private static readonly Color TimeLimitColor = new Color(1f, 1f, 1f, 0.7f);

		// Token: 0x0400327D RID: 12925
		private static readonly Color AcceptanceRequirementsColor = new Color(1f, 0.25f, 0.25f);

		// Token: 0x0400327E RID: 12926
		private static readonly Color AcceptanceRequirementsBoxColor = new Color(0.62f, 0.18f, 0.18f);

		// Token: 0x0400327F RID: 12927
		private static readonly Color acceptanceRequirementsBoxBgColor = new Color(0.13f, 0.13f, 0.13f);

		// Token: 0x04003280 RID: 12928
		private const int RowIconSize = 15;

		// Token: 0x04003281 RID: 12929
		private const float RatingWidth = 60f;

		// Token: 0x04003282 RID: 12930
		private const float RewardsConfigButtonHeight = 40f;

		// Token: 0x04003283 RID: 12931
		private static Texture2D RatingIcon = null;

		// Token: 0x04003284 RID: 12932
		private static readonly Texture2D DismissIcon = ContentFinder<Texture2D>.Get("UI/Buttons/Dismiss", true);

		// Token: 0x04003285 RID: 12933
		private static readonly Texture2D ResumeQuestIcon = ContentFinder<Texture2D>.Get("UI/Buttons/UnDismiss", true);

		// Token: 0x04003286 RID: 12934
		private static readonly Texture2D QuestDismissedIcon = ContentFinder<Texture2D>.Get("UI/Icons/DismissedQuestIcon", true);

		// Token: 0x04003287 RID: 12935
		private static List<Quest> tmpQuestsToShow = new List<Quest>();

		// Token: 0x04003288 RID: 12936
		private static List<GenUI.AnonymousStackElement> tmpStackElements = new List<GenUI.AnonymousStackElement>();

		// Token: 0x04003289 RID: 12937
		private static List<Rect> layoutRewardsRects = new List<Rect>();

		// Token: 0x0400328A RID: 12938
		private static List<QuestPart> tmpRemainingQuestParts = new List<QuestPart>();

		// Token: 0x0400328B RID: 12939
		private static List<GlobalTargetInfo> tmpLookTargets = new List<GlobalTargetInfo>();

		// Token: 0x0400328C RID: 12940
		private static List<GlobalTargetInfo> tmpSelectTargets = new List<GlobalTargetInfo>();

		// Token: 0x02001DE9 RID: 7657
		private enum QuestsTab : byte
		{
			// Token: 0x040070CC RID: 28876
			Available,
			// Token: 0x040070CD RID: 28877
			Active,
			// Token: 0x040070CE RID: 28878
			Historical
		}
	}
}
