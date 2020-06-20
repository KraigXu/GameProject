using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020009AA RID: 2474
	public static class QuestUtility
	{
		// Token: 0x06003ABE RID: 15038 RVA: 0x00137070 File Offset: 0x00135270
		public static Quest GenerateQuestAndMakeAvailable(QuestScriptDef root, float points)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", points, false);
			return QuestUtility.GenerateQuestAndMakeAvailable(root, slate);
		}

		// Token: 0x06003ABF RID: 15039 RVA: 0x00137098 File Offset: 0x00135298
		public static Quest GenerateQuestAndMakeAvailable(QuestScriptDef root, Slate vars)
		{
			Quest quest = QuestGen.Generate(root, vars);
			Find.QuestManager.Add(quest);
			return quest;
		}

		// Token: 0x06003AC0 RID: 15040 RVA: 0x001370BC File Offset: 0x001352BC
		public static void SendLetterQuestAvailable(Quest quest)
		{
			TaggedString label = IncidentDefOf.GiveQuest_Random.letterLabel + ": " + quest.name;
			TaggedString taggedString;
			if (quest.initiallyAccepted)
			{
				label = "LetterQuestAutomaticallyAcceptedTitle".Translate(quest.name);
				taggedString = "LetterQuestBecameActive".Translate(quest.name);
				int questTicksRemaining = QuestUtility.GetQuestTicksRemaining(quest);
				if (questTicksRemaining > 0)
				{
					taggedString += " " + "LetterQuestActiveNowTime".Translate(questTicksRemaining.ToStringTicksToPeriod(false, false, false, true));
				}
			}
			else
			{
				taggedString = "LetterQuestBecameAvailable".Translate(quest.name);
				if (quest.ticksUntilAcceptanceExpiry >= 0)
				{
					taggedString += "\n\n" + "LetterQuestRequiresAcceptance".Translate(quest.ticksUntilAcceptanceExpiry.ToStringTicksToPeriod(false, false, false, true));
				}
			}
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, taggedString, IncidentDefOf.GiveQuest_Random.letterDef, LookTargets.Invalid, null, quest, null);
			choiceLetter.title = quest.name;
			Find.LetterStack.ReceiveLetter(choiceLetter, null);
		}

		// Token: 0x06003AC1 RID: 15041 RVA: 0x001371D4 File Offset: 0x001353D4
		public static int GetQuestTicksRemaining(Quest quest)
		{
			foreach (QuestPart questPart in quest.PartsListForReading)
			{
				QuestPart_Delay questPart_Delay = questPart as QuestPart_Delay;
				if (questPart_Delay != null && questPart_Delay.State == QuestPartState.Enabled && questPart_Delay.isBad && !questPart_Delay.expiryInfoPart.NullOrEmpty())
				{
					return questPart_Delay.TicksLeft;
				}
			}
			return 0;
		}

		// Token: 0x06003AC2 RID: 15042 RVA: 0x00137254 File Offset: 0x00135454
		public static void GenerateBackCompatibilityNameFor(Quest quest)
		{
			quest.name = NameGenerator.GenerateName(RulePackDefOf.NamerQuestDefault, from x in Find.QuestManager.QuestsListForReading
			select x.name, false, "defaultQuestName");
		}

		// Token: 0x06003AC3 RID: 15043 RVA: 0x001372A8 File Offset: 0x001354A8
		public static bool CanPawnAcceptQuest(Pawn p, Quest quest)
		{
			for (int i = 0; i < quest.PartsListForReading.Count; i++)
			{
				QuestPart_RequirementsToAccept questPart_RequirementsToAccept = quest.PartsListForReading[i] as QuestPart_RequirementsToAccept;
				if (questPart_RequirementsToAccept != null && !questPart_RequirementsToAccept.CanPawnAccept(p))
				{
					return false;
				}
			}
			return !p.Destroyed && p.IsFreeColonist && !p.Downed && !p.Suspended && !p.IsQuestLodger();
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x00137318 File Offset: 0x00135518
		public static bool CanAcceptQuest(Quest quest)
		{
			for (int i = 0; i < quest.PartsListForReading.Count; i++)
			{
				QuestPart_RequirementsToAccept questPart_RequirementsToAccept = quest.PartsListForReading[i] as QuestPart_RequirementsToAccept;
				if (questPart_RequirementsToAccept != null && !questPart_RequirementsToAccept.CanAccept().Accepted)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x00137364 File Offset: 0x00135564
		public static Vector2 GetLocForDates()
		{
			if (Find.AnyPlayerHomeMap != null)
			{
				return Find.WorldGrid.LongLatOf(Find.AnyPlayerHomeMap.Tile);
			}
			return default(Vector2);
		}

		// Token: 0x06003AC6 RID: 15046 RVA: 0x00137398 File Offset: 0x00135598
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, default(SignalArgs));
		}

		// Token: 0x06003AC7 RID: 15047 RVA: 0x001373B5 File Offset: 0x001355B5
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1));
		}

		// Token: 0x06003AC8 RID: 15048 RVA: 0x001373C4 File Offset: 0x001355C4
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2));
		}

		// Token: 0x06003AC9 RID: 15049 RVA: 0x001373D4 File Offset: 0x001355D4
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2, arg3));
		}

		// Token: 0x06003ACA RID: 15050 RVA: 0x001373E6 File Offset: 0x001355E6
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2, arg3, arg4));
		}

		// Token: 0x06003ACB RID: 15051 RVA: 0x001373FA File Offset: 0x001355FA
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument[] args)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(args));
		}

		// Token: 0x06003ACC RID: 15052 RVA: 0x0013740C File Offset: 0x0013560C
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, SignalArgs args)
		{
			if (questTags == null)
			{
				return;
			}
			for (int i = 0; i < questTags.Count; i++)
			{
				Find.SignalManager.SendSignal(new Signal(questTags[i] + "." + signalPart, args));
			}
		}

		// Token: 0x06003ACD RID: 15053 RVA: 0x00137450 File Offset: 0x00135650
		public static void AddQuestTag(ref List<string> questTags, string questTagToAdd)
		{
			if (questTags == null)
			{
				questTags = new List<string>();
			}
			if (questTags.Contains(questTagToAdd))
			{
				return;
			}
			questTags.Add(questTagToAdd);
		}

		// Token: 0x06003ACE RID: 15054 RVA: 0x00137470 File Offset: 0x00135670
		public static void AddQuestTag(object obj, string questTagToAdd)
		{
			if (questTagToAdd.NullOrEmpty())
			{
				return;
			}
			if (obj is Thing)
			{
				QuestUtility.AddQuestTag(ref ((Thing)obj).questTags, questTagToAdd);
				return;
			}
			if (obj is WorldObject)
			{
				QuestUtility.AddQuestTag(ref ((WorldObject)obj).questTags, questTagToAdd);
				return;
			}
			if (obj is Map)
			{
				QuestUtility.AddQuestTag(ref ((Map)obj).Parent.questTags, questTagToAdd);
				return;
			}
			if (obj is Lord)
			{
				QuestUtility.AddQuestTag(ref ((Lord)obj).questTags, questTagToAdd);
				return;
			}
			if (obj is Faction)
			{
				QuestUtility.AddQuestTag(ref ((Faction)obj).questTags, questTagToAdd);
				return;
			}
			if (obj is IEnumerable)
			{
				foreach (object obj2 in ((IEnumerable)obj))
				{
					if (obj2 is Thing)
					{
						QuestUtility.AddQuestTag(ref ((Thing)obj2).questTags, questTagToAdd);
					}
					else if (obj2 is WorldObject)
					{
						QuestUtility.AddQuestTag(ref ((WorldObject)obj2).questTags, questTagToAdd);
					}
					else if (obj2 is Map)
					{
						QuestUtility.AddQuestTag(ref ((Map)obj2).Parent.questTags, questTagToAdd);
					}
					else if (obj2 is Faction)
					{
						QuestUtility.AddQuestTag(ref ((Faction)obj2).questTags, questTagToAdd);
					}
				}
			}
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x001375C8 File Offset: 0x001357C8
		public static bool AnyMatchingTags(List<string> first, List<string> second)
		{
			if (first.NullOrEmpty<string>() || second.NullOrEmpty<string>())
			{
				return false;
			}
			for (int i = 0; i < first.Count; i++)
			{
				for (int j = 0; j < second.Count; j++)
				{
					if (first[i] == second[j])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003AD0 RID: 15056 RVA: 0x00137621 File Offset: 0x00135821
		public static bool IsReservedByQuestOrQuestBeingGenerated(Pawn pawn)
		{
			return Find.QuestManager.IsReservedByAnyQuest(pawn) || (QuestGen.quest != null && QuestGen.quest.QuestReserves(pawn)) || QuestGen.WasGeneratedForQuestBeingGenerated(pawn);
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x0013764C File Offset: 0x0013584C
		public static IEnumerable<QuestPart_WorkDisabled> GetWorkDisabledQuestPart(Pawn p)
		{
			List<Quest> quests = Find.QuestManager.QuestsListForReading;
			int num;
			for (int i = 0; i < quests.Count; i = num + 1)
			{
				if (quests[i].State == QuestState.Ongoing)
				{
					Quest quest = quests[i];
					List<QuestPart> partList = quest.PartsListForReading;
					for (int j = 0; j < partList.Count; j = num + 1)
					{
						QuestPart_WorkDisabled questPart_WorkDisabled;
						if ((questPart_WorkDisabled = (partList[j] as QuestPart_WorkDisabled)) != null && questPart_WorkDisabled.pawns.Contains(p))
						{
							yield return questPart_WorkDisabled;
						}
						num = j;
					}
					partList = null;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06003AD2 RID: 15058 RVA: 0x0013765C File Offset: 0x0013585C
		public static bool IsQuestLodger(this Pawn p)
		{
			return p.HasExtraHomeFaction(null);
		}

		// Token: 0x06003AD3 RID: 15059 RVA: 0x00137668 File Offset: 0x00135868
		public static bool IsQuestHelper(this Pawn p)
		{
			if (!p.IsQuestLodger())
			{
				return false;
			}
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_ExtraFaction questPart_ExtraFaction;
						if ((questPart_ExtraFaction = (partsListForReading[j] as QuestPart_ExtraFaction)) != null && questPart_ExtraFaction.affectedPawns.Contains(p) && questPart_ExtraFaction.areHelpers)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06003AD4 RID: 15060 RVA: 0x001376F4 File Offset: 0x001358F4
		public static bool LodgerAllowedDecrees(this Pawn p)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_AllowDecreesForLodger questPart_AllowDecreesForLodger;
						if ((questPart_AllowDecreesForLodger = (partsListForReading[j] as QuestPart_AllowDecreesForLodger)) != null && questPart_AllowDecreesForLodger.lodger == p)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06003AD5 RID: 15061 RVA: 0x00137768 File Offset: 0x00135968
		public static bool HasExtraHomeFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraHomeFaction(forQuest) != null;
		}

		// Token: 0x06003AD6 RID: 15062 RVA: 0x00137774 File Offset: 0x00135974
		public static bool HasExtraHomeFaction(this Pawn p, Faction faction)
		{
			QuestUtility.tmpExtraFactions.Clear();
			QuestUtility.GetExtraFactionsFromQuestParts(p, QuestUtility.tmpExtraFactions, null);
			for (int i = 0; i < QuestUtility.tmpExtraFactions.Count; i++)
			{
				if (QuestUtility.tmpExtraFactions[i].factionType == ExtraFactionType.HomeFaction && QuestUtility.tmpExtraFactions[i].faction == faction)
				{
					QuestUtility.tmpExtraFactions.Clear();
					return true;
				}
			}
			QuestUtility.tmpExtraFactions.Clear();
			return false;
		}

		// Token: 0x06003AD7 RID: 15063 RVA: 0x001377E8 File Offset: 0x001359E8
		public static Faction GetExtraHomeFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraFaction(ExtraFactionType.HomeFaction, forQuest);
		}

		// Token: 0x06003AD8 RID: 15064 RVA: 0x001377F2 File Offset: 0x001359F2
		public static Faction GetExtraHostFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraFaction(ExtraFactionType.HostFaction, forQuest);
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x001377FC File Offset: 0x001359FC
		public static Faction GetExtraFaction(this Pawn p, ExtraFactionType extraFactionType, Quest forQuest = null)
		{
			QuestUtility.tmpExtraFactions.Clear();
			QuestUtility.GetExtraFactionsFromQuestParts(p, QuestUtility.tmpExtraFactions, forQuest);
			for (int i = 0; i < QuestUtility.tmpExtraFactions.Count; i++)
			{
				if (QuestUtility.tmpExtraFactions[i].factionType == extraFactionType)
				{
					Faction faction = QuestUtility.tmpExtraFactions[i].faction;
					QuestUtility.tmpExtraFactions.Clear();
					return faction;
				}
			}
			QuestUtility.tmpExtraFactions.Clear();
			return null;
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x00137870 File Offset: 0x00135A70
		public static void GetExtraFactionsFromQuestParts(Pawn pawn, List<ExtraFaction> outExtraFactions, Quest forQuest = null)
		{
			outExtraFactions.Clear();
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing || questsListForReading[i] == forQuest)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_ExtraFaction questPart_ExtraFaction = partsListForReading[j] as QuestPart_ExtraFaction;
						if (questPart_ExtraFaction != null && questPart_ExtraFaction.affectedPawns.Contains(pawn))
						{
							outExtraFactions.Add(questPart_ExtraFaction.extraFaction);
						}
					}
				}
			}
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x00137904 File Offset: 0x00135B04
		public static bool IsBorrowedByAnyFaction(this Pawn pawn)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction;
						if ((questPart_LendColonistsToFaction = (partsListForReading[j] as QuestPart_LendColonistsToFaction)) != null && questPart_LendColonistsToFaction.LentColonistsListForReading.Contains(pawn))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06003ADC RID: 15068 RVA: 0x00137980 File Offset: 0x00135B80
		public static int TotalBorrowedColonistCount()
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			int num = 0;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction;
						if ((questPart_LendColonistsToFaction = (partsListForReading[j] as QuestPart_LendColonistsToFaction)) != null)
						{
							num += questPart_LendColonistsToFaction.LentColonistsListForReading.Count;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x001379FE File Offset: 0x00135BFE
		public static IEnumerable<T> GetAllQuestPartsOfType<T>(bool ongoingOnly = true) where T : class
		{
			List<Quest> quests = Find.QuestManager.QuestsListForReading;
			int num;
			for (int i = 0; i < quests.Count; i = num + 1)
			{
				if (!ongoingOnly || quests[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partList = quests[i].PartsListForReading;
					for (int j = 0; j < partList.Count; j = num + 1)
					{
						T t = partList[j] as T;
						if (t != null)
						{
							yield return t;
						}
						num = j;
					}
					partList = null;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x00137A10 File Offset: 0x00135C10
		public static void AppendInspectStringsFromQuestParts(StringBuilder sb, ISelectable target)
		{
			int num;
			QuestUtility.AppendInspectStringsFromQuestParts(sb, target, out num);
		}

		// Token: 0x06003ADF RID: 15071 RVA: 0x00137A26 File Offset: 0x00135C26
		public static void AppendInspectStringsFromQuestParts(StringBuilder sb, ISelectable target, out int count)
		{
			QuestUtility.AppendInspectStringsFromQuestParts(delegate(string str, Quest quest)
			{
				if (sb.Length != 0)
				{
					sb.AppendLine();
				}
				sb.Append(str);
			}, target, out count);
		}

		// Token: 0x06003AE0 RID: 15072 RVA: 0x00137A48 File Offset: 0x00135C48
		public static void AppendInspectStringsFromQuestParts(Action<string, Quest> func, ISelectable target, out int count)
		{
			count = 0;
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					QuestState state = questsListForReading[i].State;
					QuestUtility.tmpQuestParts.Clear();
					QuestUtility.tmpQuestParts.AddRange(questsListForReading[i].PartsListForReading);
					QuestUtility.tmpQuestParts.SortBy(delegate(QuestPart x)
					{
						if (!(x is QuestPartActivable))
						{
							return 0;
						}
						return ((QuestPartActivable)x).EnableTick;
					});
					for (int j = 0; j < QuestUtility.tmpQuestParts.Count; j++)
					{
						QuestPartActivable questPartActivable = QuestUtility.tmpQuestParts[j] as QuestPartActivable;
						if (questPartActivable != null && questPartActivable.State == QuestPartState.Enabled)
						{
							string str = questPartActivable.ExtraInspectString(target);
							if (!str.NullOrEmpty())
							{
								func(str.Formatted(target.Named("TARGET")), questsListForReading[i]);
								count++;
							}
						}
					}
					QuestUtility.tmpQuestParts.Clear();
				}
			}
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x00137B5A File Offset: 0x00135D5A
		public static IEnumerable<Gizmo> GetQuestRelatedGizmos(Thing thing)
		{
			if (Find.Selector.SelectedObjects.Count == 1)
			{
				Quest linkedQuest = Find.QuestManager.QuestsListForReading.FirstOrDefault((Quest q) => !q.Historical && !q.dismissed && (q.QuestLookTargets.Contains(thing) || q.QuestSelectTargets.Contains(thing)));
				if (linkedQuest != null)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandOpenLinkedQuest".Translate(linkedQuest.name),
						defaultDesc = "CommandOpenLinkedQuestDesc".Translate(),
						icon = TexCommand.OpenLinkedQuestTex,
						action = delegate
						{
							Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
							((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(linkedQuest);
						}
					};
				}
			}
			yield break;
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x00137B6C File Offset: 0x00135D6C
		public static Gizmo GetSelectMonumentMarkerGizmo(Thing thing)
		{
			if (!thing.Spawned || !ModsConfig.RoyaltyActive)
			{
				return null;
			}
			List<Thing> list = thing.Map.listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
			for (int i = 0; i < list.Count; i++)
			{
				MonumentMarker m = (MonumentMarker)list[i];
				if (m.IsPart(thing))
				{
					return new Command_Action
					{
						defaultLabel = "CommandSelectMonumentMarker".Translate(),
						defaultDesc = "CommandSelectMonumentMarkerDesc".Translate(),
						icon = ThingDefOf.MonumentMarker.uiIcon,
						iconAngle = ThingDefOf.MonumentMarker.uiIconAngle,
						iconOffset = ThingDefOf.MonumentMarker.uiIconOffset,
						action = delegate
						{
							CameraJumper.TrySelect(m);
						}
					};
				}
			}
			return null;
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x00137C50 File Offset: 0x00135E50
		public static bool AnyQuestDisablesRandomMoodCausedMentalBreaksFor(Pawn p)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_DisableRandomMoodCausedMentalBreaks questPart_DisableRandomMoodCausedMentalBreaks;
						if ((questPart_DisableRandomMoodCausedMentalBreaks = (partsListForReading[j] as QuestPart_DisableRandomMoodCausedMentalBreaks)) != null && questPart_DisableRandomMoodCausedMentalBreaks.State == QuestPartState.Enabled && questPart_DisableRandomMoodCausedMentalBreaks.pawns.Contains(p))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x040022BF RID: 8895
		public const string QuestTargetSignalPart_MapGenerated = "MapGenerated";

		// Token: 0x040022C0 RID: 8896
		public const string QuestTargetSignalPart_MapRemoved = "MapRemoved";

		// Token: 0x040022C1 RID: 8897
		public const string QuestTargetSignalPart_Spawned = "Spawned";

		// Token: 0x040022C2 RID: 8898
		public const string QuestTargetSignalPart_Despawned = "Despawned";

		// Token: 0x040022C3 RID: 8899
		public const string QuestTargetSignalPart_Destroyed = "Destroyed";

		// Token: 0x040022C4 RID: 8900
		public const string QuestTargetSignalPart_Killed = "Killed";

		// Token: 0x040022C5 RID: 8901
		public const string QuestTargetSignalPart_ChangedFaction = "ChangedFaction";

		// Token: 0x040022C6 RID: 8902
		public const string QuestTargetSignalPart_LeftMap = "LeftMap";

		// Token: 0x040022C7 RID: 8903
		public const string QuestTargetSignalPart_SurgeryViolation = "SurgeryViolation";

		// Token: 0x040022C8 RID: 8904
		public const string QuestTargetSignalPart_Arrested = "Arrested";

		// Token: 0x040022C9 RID: 8905
		public const string QuestTargetSignalPart_Recruited = "Recruited";

		// Token: 0x040022CA RID: 8906
		public const string QuestTargetSignalPart_Kidnapped = "Kidnapped";

		// Token: 0x040022CB RID: 8907
		public const string QuestTargetSignalPart_ChangedHostFaction = "ChangedHostFaction";

		// Token: 0x040022CC RID: 8908
		public const string QuestTargetSignalPart_NoLongerFactionLeader = "NoLongerFactionLeader";

		// Token: 0x040022CD RID: 8909
		public const string QuestTargetSignalPart_TitleChanged = "TitleChanged";

		// Token: 0x040022CE RID: 8910
		public const string QuestTargetSignalPart_ShuttleSentSatisfied = "SentSatisfied";

		// Token: 0x040022CF RID: 8911
		public const string QuestTargetSignalPart_ShuttleSentUnsatisfied = "SentUnsatisfied";

		// Token: 0x040022D0 RID: 8912
		public const string QuestTargetSignalPart_ShuttleSentWithExtraColonists = "SentWithExtraColonists";

		// Token: 0x040022D1 RID: 8913
		public const string QuestTargetSignalPart_AllEnemiesDefeated = "AllEnemiesDefeated";

		// Token: 0x040022D2 RID: 8914
		public const string QuestTargetSignalPart_TradeRequestFulfilled = "TradeRequestFulfilled";

		// Token: 0x040022D3 RID: 8915
		public const string QuestTargetSignalPart_PeaceTalksResolved = "Resolved";

		// Token: 0x040022D4 RID: 8916
		public const string QuestTargetSignalPart_LaunchedShip = "LaunchedShip";

		// Token: 0x040022D5 RID: 8917
		public const string QuestTargetSignalPart_ReactorDestroyed = "ReactorDestroyed";

		// Token: 0x040022D6 RID: 8918
		public const string QuestTargetSignalPart_MonumentCompleted = "MonumentCompleted";

		// Token: 0x040022D7 RID: 8919
		public const string QuestTargetSignalPart_MonumentDestroyed = "MonumentDestroyed";

		// Token: 0x040022D8 RID: 8920
		public const string QuestTargetSignalPart_MonumentCancelled = "MonumentCancelled";

		// Token: 0x040022D9 RID: 8921
		public const string QuestTargetSignalPart_AllHivesDestroyed = "AllHivesDestroyed";

		// Token: 0x040022DA RID: 8922
		public const string QuestTargetSignalPart_ExitMentalState = "ExitMentalState";

		// Token: 0x040022DB RID: 8923
		public const string QuestTargetSignalPart_FactionBecameHostileToPlayer = "BecameHostileToPlayer";

		// Token: 0x040022DC RID: 8924
		private static List<ExtraFaction> tmpExtraFactions = new List<ExtraFaction>();

		// Token: 0x040022DD RID: 8925
		private static List<QuestPart> tmpQuestParts = new List<QuestPart>();
	}
}
