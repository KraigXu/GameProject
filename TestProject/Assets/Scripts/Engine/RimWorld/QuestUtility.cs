using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using RimWorld.QuestGenNew;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public static class QuestUtility
	{
		
		public static Quest GenerateQuestAndMakeAvailable(QuestScriptDef root, float points)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", points, false);
			return QuestUtility.GenerateQuestAndMakeAvailable(root, slate);
		}

		
		public static Quest GenerateQuestAndMakeAvailable(QuestScriptDef root, Slate vars)
		{
			Quest quest = QuestGen.Generate(root, vars);
			Find.QuestManager.Add(quest);
			return quest;
		}

		
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

		
		public static void GenerateBackCompatibilityNameFor(Quest quest)
		{
			quest.name = NameGenerator.GenerateName(RulePackDefOf.NamerQuestDefault, from x in Find.QuestManager.QuestsListForReading
			select x.name, false, "defaultQuestName");
		}

		
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

		
		public static Vector2 GetLocForDates()
		{
			if (Find.AnyPlayerHomeMap != null)
			{
				return Find.WorldGrid.LongLatOf(Find.AnyPlayerHomeMap.Tile);
			}
			return default(Vector2);
		}

		
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, default(SignalArgs));
		}

		
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1));
		}

		
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2));
		}

		
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2, arg3));
		}

		
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2, arg3, arg4));
		}

		
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument[] args)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(args));
		}

		
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

		
		public static bool IsReservedByQuestOrQuestBeingGenerated(Pawn pawn)
		{
			return Find.QuestManager.IsReservedByAnyQuest(pawn) || (QuestGen.quest != null && QuestGen.quest.QuestReserves(pawn)) || QuestGen.WasGeneratedForQuestBeingGenerated(pawn);
		}

		
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

		
		public static bool IsQuestLodger(this Pawn p)
		{
			return p.HasExtraHomeFaction(null);
		}

		
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

		
		//public static bool HasExtraHomeFaction(this Pawn p, Quest forQuest)
		//{
		//	return p.GetExtraHomeFaction(forQuest) != null;
		//}
		public static bool HasExtraHomeQuest(this Pawn p, Quest forQuest)
		{
			return p.GetExtraHomeFaction(forQuest) != null;
		}


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

		
		public static Faction GetExtraHomeFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraFaction(ExtraFactionType.HomeFaction, forQuest);
		}

		
		public static Faction GetExtraHostFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraFaction(ExtraFactionType.HostFaction, forQuest);
		}

		
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

		
		public static void AppendInspectStringsFromQuestParts(StringBuilder sb, ISelectable target)
		{
			int num;
			QuestUtility.AppendInspectStringsFromQuestParts(sb, target, out num);
		}

		
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

		
		public const string QuestTargetSignalPart_MapGenerated = "MapGenerated";

		
		public const string QuestTargetSignalPart_MapRemoved = "MapRemoved";

		
		public const string QuestTargetSignalPart_Spawned = "Spawned";

		
		public const string QuestTargetSignalPart_Despawned = "Despawned";

		
		public const string QuestTargetSignalPart_Destroyed = "Destroyed";

		
		public const string QuestTargetSignalPart_Killed = "Killed";

		
		public const string QuestTargetSignalPart_ChangedFaction = "ChangedFaction";

		
		public const string QuestTargetSignalPart_LeftMap = "LeftMap";

		
		public const string QuestTargetSignalPart_SurgeryViolation = "SurgeryViolation";

		
		public const string QuestTargetSignalPart_Arrested = "Arrested";

		
		public const string QuestTargetSignalPart_Recruited = "Recruited";

		
		public const string QuestTargetSignalPart_Kidnapped = "Kidnapped";

		
		public const string QuestTargetSignalPart_ChangedHostFaction = "ChangedHostFaction";

		
		public const string QuestTargetSignalPart_NoLongerFactionLeader = "NoLongerFactionLeader";

		
		public const string QuestTargetSignalPart_TitleChanged = "TitleChanged";

		
		public const string QuestTargetSignalPart_ShuttleSentSatisfied = "SentSatisfied";

		
		public const string QuestTargetSignalPart_ShuttleSentUnsatisfied = "SentUnsatisfied";

		
		public const string QuestTargetSignalPart_ShuttleSentWithExtraColonists = "SentWithExtraColonists";

		
		public const string QuestTargetSignalPart_AllEnemiesDefeated = "AllEnemiesDefeated";

		
		public const string QuestTargetSignalPart_TradeRequestFulfilled = "TradeRequestFulfilled";

		
		public const string QuestTargetSignalPart_PeaceTalksResolved = "Resolved";

		
		public const string QuestTargetSignalPart_LaunchedShip = "LaunchedShip";

		
		public const string QuestTargetSignalPart_ReactorDestroyed = "ReactorDestroyed";

		
		public const string QuestTargetSignalPart_MonumentCompleted = "MonumentCompleted";

		
		public const string QuestTargetSignalPart_MonumentDestroyed = "MonumentDestroyed";

		
		public const string QuestTargetSignalPart_MonumentCancelled = "MonumentCancelled";

		
		public const string QuestTargetSignalPart_AllHivesDestroyed = "AllHivesDestroyed";

		
		public const string QuestTargetSignalPart_ExitMentalState = "ExitMentalState";

		
		public const string QuestTargetSignalPart_FactionBecameHostileToPlayer = "BecameHostileToPlayer";

		
		private static List<ExtraFaction> tmpExtraFactions = new List<ExtraFaction>();

		
		private static List<QuestPart> tmpQuestParts = new List<QuestPart>();
	}
}
