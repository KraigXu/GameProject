using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000999 RID: 2457
	public static class QuestPartUtility
	{
		// Token: 0x06003A49 RID: 14921 RVA: 0x001348CC File Offset: 0x00132ACC
		public static T MakeAndAddEndCondition<T>(Quest quest, string inSignalActivate, QuestEndOutcome outcome, Letter letter = null) where T : QuestPartActivable, new()
		{
			T t = Activator.CreateInstance<T>();
			t.inSignalEnable = inSignalActivate;
			quest.AddPart(t);
			if (letter != null)
			{
				quest.AddPart(new QuestPart_Letter
				{
					letter = letter,
					inSignal = t.OutSignalCompleted
				});
			}
			quest.AddPart(new QuestPart_QuestEnd
			{
				inSignal = t.OutSignalCompleted,
				outcome = new QuestEndOutcome?(outcome)
			});
			return t;
		}

		// Token: 0x06003A4A RID: 14922 RVA: 0x0013494C File Offset: 0x00132B4C
		public static QuestPart_QuestEnd MakeAndAddEndNodeWithLetter(Quest quest, string inSignalActivate, QuestEndOutcome outcome, Letter letter)
		{
			quest.AddPart(new QuestPart_Letter
			{
				letter = letter,
				inSignal = inSignalActivate
			});
			QuestPart_QuestEnd questPart_QuestEnd = new QuestPart_QuestEnd();
			questPart_QuestEnd.inSignal = inSignalActivate;
			questPart_QuestEnd.outcome = new QuestEndOutcome?(outcome);
			quest.AddPart(questPart_QuestEnd);
			return questPart_QuestEnd;
		}

		// Token: 0x06003A4B RID: 14923 RVA: 0x00134998 File Offset: 0x00132B98
		public static QuestPart_Delay MakeAndAddQuestTimeoutDelay(Quest quest, int delayTicks, WorldObject worldObject)
		{
			QuestPart_WorldObjectTimeout questPart_WorldObjectTimeout = new QuestPart_WorldObjectTimeout();
			questPart_WorldObjectTimeout.delayTicks = delayTicks;
			questPart_WorldObjectTimeout.expiryInfoPart = "QuestExpiresIn".Translate();
			questPart_WorldObjectTimeout.expiryInfoPartTip = "QuestExpiresOn".Translate();
			questPart_WorldObjectTimeout.isBad = true;
			questPart_WorldObjectTimeout.outcomeCompletedSignalArg = QuestEndOutcome.Fail;
			questPart_WorldObjectTimeout.inSignalEnable = quest.InitiateSignal;
			quest.AddPart(questPart_WorldObjectTimeout);
			string text = "Quest" + quest.id + ".DelayingWorldObject";
			QuestUtility.AddQuestTag(ref worldObject.questTags, text);
			questPart_WorldObjectTimeout.inSignalDisable = text + ".MapGenerated";
			quest.AddPart(new QuestPart_QuestEnd
			{
				inSignal = questPart_WorldObjectTimeout.OutSignalCompleted
			});
			return questPart_WorldObjectTimeout;
		}

		// Token: 0x06003A4C RID: 14924 RVA: 0x00134A4F File Offset: 0x00132C4F
		public static IEnumerable<GenUI.AnonymousStackElement> GetRewardStackElementsForThings(IEnumerable<Thing> things, bool detailsHidden = false)
		{
			QuestPartUtility.tmpThings.Clear();
			foreach (Thing thing2 in things)
			{
				bool flag = false;
				for (int j = 0; j < QuestPartUtility.tmpThings.Count; j++)
				{
					if (QuestPartUtility.tmpThings[j].First.CanStackWith(thing2))
					{
						QuestPartUtility.tmpThings[j] = new Pair<Thing, int>(QuestPartUtility.tmpThings[j].First, QuestPartUtility.tmpThings[j].Second + thing2.stackCount);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					QuestPartUtility.tmpThings.Add(new Pair<Thing, int>(thing2, thing2.stackCount));
				}
			}
			int num;
			for (int i = 0; i < QuestPartUtility.tmpThings.Count; i = num + 1)
			{
				Thing thing = QuestPartUtility.tmpThings[i].First.GetInnerIfMinified();
				int second = QuestPartUtility.tmpThings[i].Second;
				Pawn value;
				string label;
				if ((value = (thing as Pawn)) != null)
				{
					label = "PawnQuestReward".Translate(value);
				}
				else
				{
					label = thing.LabelCapNoCount + ((second > 1) ? (" x" + second) : "");
				}
				yield return new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect rect)
					{
						Widgets.DrawHighlight(rect);
						Rect rect2 = new Rect(rect.x + 6f, rect.y + 1f, rect.width - 12f, rect.height - 2f);
						if (Mouse.IsOver(rect))
						{
							Widgets.DrawHighlight(rect);
							TaggedString t = detailsHidden ? "NoMoreInfoAvailable".Translate() : "ClickForMoreInfo".Translate();
							Pawn pawn;
							if ((pawn = (thing as Pawn)) != null && pawn.RaceProps.Humanlike)
							{
								TooltipHandler.TipRegion(rect, pawn.LabelCap + "\n\n" + t);
							}
							else
							{
								TooltipHandler.TipRegion(rect, thing.DescriptionDetailed + "\n\n" + t);
							}
						}
						Widgets.ThingIcon(new Rect(rect2.x, rect2.y, 22f, 22f), thing, 1f);
						Rect rect3 = rect2;
						rect3.xMin += 24f;
						Widgets.Label(rect3, label);
						if (Widgets.ButtonInvisible(rect, true))
						{
							if (detailsHidden)
							{
								Messages.Message("NoMoreInfoAvailable".Translate(), MessageTypeDefOf.RejectInput, false);
								return;
							}
							Find.WindowStack.Add(new Dialog_InfoCard(thing));
						}
					},
					width = Text.CalcSize(label).x + 12f + 22f + 2f
				};
				num = i;
			}
			yield break;
		}

		// Token: 0x06003A4D RID: 14925 RVA: 0x00134A66 File Offset: 0x00132C66
		public static IEnumerable<GenUI.AnonymousStackElement> GetRewardStackElementsForThings(IEnumerable<Reward_Items.RememberedItem> thingDefs)
		{
			QuestPartUtility.tmpThingDefs.Clear();
			foreach (Reward_Items.RememberedItem rememberedItem in thingDefs)
			{
				bool flag = false;
				for (int j = 0; j < QuestPartUtility.tmpThingDefs.Count; j++)
				{
					if (QuestPartUtility.tmpThingDefs[j].thing == rememberedItem.thing && QuestPartUtility.tmpThingDefs[j].label == rememberedItem.label)
					{
						QuestPartUtility.tmpThingDefs[j] = new Reward_Items.RememberedItem(QuestPartUtility.tmpThingDefs[j].thing, QuestPartUtility.tmpThingDefs[j].stackCount + rememberedItem.stackCount, QuestPartUtility.tmpThingDefs[j].label);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					QuestPartUtility.tmpThingDefs.Add(rememberedItem);
				}
			}
			int num;
			for (int i = 0; i < QuestPartUtility.tmpThingDefs.Count; i = num + 1)
			{
				ThingStuffPairWithQuality thing = QuestPartUtility.tmpThingDefs[i].thing;
				int stackCount = QuestPartUtility.tmpThingDefs[i].stackCount;
				string label = QuestPartUtility.tmpThingDefs[i].label.CapitalizeFirst() + ((stackCount > 1) ? (" x" + stackCount) : "");
				yield return new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect rect)
					{
						Widgets.DrawHighlight(rect);
						Rect rect2 = new Rect(rect.x + 6f, rect.y + 1f, rect.width - 12f, rect.height - 2f);
						if (Mouse.IsOver(rect))
						{
							Widgets.DrawHighlight(rect);
							TooltipHandler.TipRegion(rect, thing.thing.DescriptionDetailed + "\n\n" + "ClickForMoreInfo".Translate());
						}
						Widgets.ThingIcon(new Rect(rect2.x, rect2.y, 22f, 22f), thing.thing, thing.stuff, 1f);
						Rect rect3 = rect2;
						rect3.xMin += 24f;
						Widgets.Label(rect3, label);
						if (Widgets.ButtonInvisible(rect, true))
						{
							Find.WindowStack.Add(new Dialog_InfoCard(thing.thing, thing.stuff));
						}
					},
					width = Text.CalcSize(label).x + 12f + 22f + 2f
				};
				num = i;
			}
			yield break;
		}

		// Token: 0x06003A4E RID: 14926 RVA: 0x00134A78 File Offset: 0x00132C78
		public static GenUI.AnonymousStackElement GetStandardRewardStackElement(string label, Texture2D icon, Func<string> tipGetter, Action onClick = null)
		{
			return QuestPartUtility.GetStandardRewardStackElement(label, delegate(Rect r)
			{
				GUI.DrawTexture(r, icon);
			}, tipGetter, onClick);
		}

		// Token: 0x06003A4F RID: 14927 RVA: 0x00134AA8 File Offset: 0x00132CA8
		public static GenUI.AnonymousStackElement GetStandardRewardStackElement(string label, Action<Rect> iconDrawer, Func<string> tipGetter, Action onClick = null)
		{
			return new GenUI.AnonymousStackElement
			{
				drawer = delegate(Rect rect)
				{
					Widgets.DrawHighlight(rect);
					Rect rect2 = new Rect(rect.x + 6f, rect.y + 1f, rect.width - 12f, rect.height - 2f);
					if (Mouse.IsOver(rect))
					{
						Widgets.DrawHighlight(rect);
						if (tipGetter != null)
						{
							TooltipHandler.TipRegion(rect, new TipSignal(tipGetter, 90876542 ^ (int)rect.x ^ (int)rect.y));
						}
					}
					Rect obj = new Rect(rect2.x, rect2.y, 22f, 22f);
					iconDrawer(obj);
					Rect rect3 = rect2;
					rect3.xMin += 24f;
					Widgets.Label(rect3, label);
					if (onClick != null && Widgets.ButtonInvisible(rect, true))
					{
						onClick();
					}
				},
				width = Text.CalcSize(label).x + 12f + 22f + 2f
			};
		}

		// Token: 0x04002254 RID: 8788
		public const int RewardStackElementIconSize = 22;

		// Token: 0x04002255 RID: 8789
		public const int RewardStackElementMarginHorizontal = 6;

		// Token: 0x04002256 RID: 8790
		public const int RewardStackElementMarginVertical = 1;

		// Token: 0x04002257 RID: 8791
		public const int RewardStackElementIconGap = 2;

		// Token: 0x04002258 RID: 8792
		private static List<Pair<Thing, int>> tmpThings = new List<Pair<Thing, int>>();

		// Token: 0x04002259 RID: 8793
		private static List<Reward_Items.RememberedItem> tmpThingDefs = new List<Reward_Items.RememberedItem>();
	}
}
