using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGenNew
{
	
	public static class QuestGen
	{
	
		public static QuestScriptDef Root
		{
			get
			{
				return QuestGen.root;
			}
		}

		
		
		public static bool Working
		{
			get
			{
				return QuestGen.working;
			}
		}

		
		
		public static List<Rule> QuestDescriptionRulesReadOnly
		{
			get
			{
				return QuestGen.questDescriptionRules;
			}
		}

		
		
		public static Dictionary<string, string> QuestDescriptionConstantsReadOnly
		{
			get
			{
				return QuestGen.questDescriptionConstants;
			}
		}

		
		
		public static List<Rule> QuestNameRulesReadOnly
		{
			get
			{
				return QuestGen.questNameRules;
			}
		}

		
		
		public static Dictionary<string, string> QuestNameConstantsReadOnly
		{
			get
			{
				return QuestGen.questNameConstants;
			}
		}

		
		
		public static List<QuestTextRequest> TextRequestsReadOnly
		{
			get
			{
				return QuestGen.textRequests;
			}
		}

		
		public static string GenerateNewSignal(string signalString, bool ensureUnique = true)
		{
			int num;
			if (!ensureUnique || !QuestGen.generatedSignals.TryGetValue(signalString, out num))
			{
				num = 0;
			}
			string result = string.Concat(new object[]
			{
				"Quest",
				QuestGen.quest.id,
				".",
				signalString,
				(num == 0) ? "" : (num + 1).ToString()
			});
			QuestGen.generatedSignals[signalString] = num + 1;
			return result;
		}

		
		public static string GenerateNewTargetQuestTag(string targetString, bool ensureUnique = true)
		{
			int num;
			if (!ensureUnique || !QuestGen.generatedTargetQuestTags.TryGetValue(targetString, out num))
			{
				num = 0;
			}
			string result = string.Concat(new object[]
			{
				"Quest",
				QuestGen.quest.id,
				".",
				targetString,
				(num == 0) ? "" : (num + 1).ToString()
			});
			QuestGen.generatedTargetQuestTags[targetString] = num + 1;
			return result;
		}

		
		private static void ResetIdCounters()
		{
			QuestGen.generatedSignals.Clear();
			QuestGen.generatedTargetQuestTags.Clear();
		}

		
		public static Quest Generate(QuestScriptDef root, Slate initialVars)
		{
			if (DeepProfiler.enabled)
			{
				DeepProfiler.Start("Generate quest");
			}
			Quest result = null;
			try
			{
				if (QuestGen.working)
				{
					throw new Exception("Called Generate() while already working.");
				}
				QuestGen.working = true;
				QuestGen.root = root;
				QuestGen.slate.Reset();
				QuestGen.slate.SetAll(initialVars);
				QuestGen.quest = Quest.MakeRaw();
				QuestGen.quest.ticksUntilAcceptanceExpiry = (int)(root.expireDaysRange.RandomInRange * 60000f);
				if (root.defaultChallengeRating > 0)
				{
					QuestGen.quest.challengeRating = root.defaultChallengeRating;
				}
				QuestGen.quest.root = root;
				QuestGen.slate.SetIfNone<string>("inSignal", QuestGen.quest.InitiateSignal, false);
				root.Run();
				try
				{
					QuestNode_ResolveQuestName.Resolve();
				}
				catch (Exception arg)
				{
					Log.Error("Error while generating quest name: " + arg, false);
				}
				try
				{
					QuestNode_ResolveQuestDescription.Resolve();
				}
				catch (Exception arg2)
				{
					Log.Error("Error while generating quest description: " + arg2, false);
				}
				try
				{
					QuestNode_ResolveTextRequests.Resolve();
				}
				catch (Exception arg3)
				{
					Log.Error("Error while resolving text requests: " + arg3, false);
				}
				QuestGen.AddSlateQuestTags();
				bool flag = root.autoAccept;
				if (flag)
				{
					List<QuestPart> partsListForReading = QuestGen.quest.PartsListForReading;
					for (int i = 0; i < partsListForReading.Count; i++)
					{
						if (partsListForReading[i].PreventsAutoAccept)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					QuestGen.quest.SetInitiallyAccepted();
				}
				result = QuestGen.quest;
			}
			catch (Exception arg4)
			{
				Log.Error("Error in QuestGen: " + arg4, false);
			}
			finally
			{
				if (DeepProfiler.enabled)
				{
					DeepProfiler.End();
				}
				QuestGen.quest = null;
				QuestGen.root = null;
				QuestGen.working = false;
				QuestGen.generatedPawns.Clear();
				QuestGen.textRequests.Clear();
				QuestGen.slate.Reset();
				QuestGen.questDescriptionRules.Clear();
				QuestGen.questDescriptionConstants.Clear();
				QuestGen.questNameRules.Clear();
				QuestGen.questNameConstants.Clear();
				QuestGen.slateQuestTagsToAddWhenFinished.Clear();
				QuestGen.ResetIdCounters();
			}
			return result;
		}

		
		public static void AddToGeneratedPawns(Pawn pawn)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add a pawn to generated pawns while not resolving any quest.", false);
				return;
			}
			if (!QuestGen.generatedPawns.Contains(pawn))
			{
				QuestGen.generatedPawns.Add(pawn);
			}
		}

		
		public static bool WasGeneratedForQuestBeingGenerated(Pawn pawn)
		{
			return QuestGen.working && QuestGen.generatedPawns.Contains(pawn);
		}

		
		public static void AddQuestDescriptionRules(RulePack rulePack)
		{
			QuestGen.AddQuestDescriptionRules(rulePack.Rules);
		}

		
		public static void AddQuestDescriptionRules(List<Rule> rules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest description rules while not resolving any quest.", false);
				return;
			}
			QuestGen.questDescriptionRules.AddRange(QuestGenUtility.AppendCurrentPrefix(rules));
		}

		
		public static void AddQuestDescriptionConstants(Dictionary<string, string> constants)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest description constants while not resolving any quest.", false);
				return;
			}
			foreach (KeyValuePair<string, string> keyValuePair in QuestGenUtility.AppendCurrentPrefix(constants))
			{
				if (!QuestGen.questDescriptionConstants.ContainsKey(keyValuePair.Key))
				{
					QuestGen.questDescriptionConstants.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		
		public static void AddQuestNameRules(RulePack rulePack)
		{
			QuestGen.AddQuestNameRules(rulePack.Rules);
		}

		
		public static void AddQuestNameRules(List<Rule> rules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest name rules while not resolving any quest.", false);
				return;
			}
			QuestGen.questNameRules.AddRange(QuestGenUtility.AppendCurrentPrefix(rules));
		}

		
		public static void AddQuestNameConstants(Dictionary<string, string> constants)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest name constants while not resolving any quest.", false);
				return;
			}
			foreach (KeyValuePair<string, string> keyValuePair in QuestGenUtility.AppendCurrentPrefix(constants))
			{
				if (!QuestGen.questNameConstants.ContainsKey(keyValuePair.Key))
				{
					QuestGen.questNameConstants.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		
		public static void AddSlateQuestTagToAddWhenFinished(string slateVarNameWithPrefix)
		{
			if (!QuestGen.slateQuestTagsToAddWhenFinished.Contains(slateVarNameWithPrefix))
			{
				QuestGen.slateQuestTagsToAddWhenFinished.Add(slateVarNameWithPrefix);
			}
		}

		
		public static void AddTextRequest(string localKeyword, Action<string> setter, RulePack extraLocalRules = null)
		{
			QuestGen.AddTextRequest(localKeyword, setter, (extraLocalRules != null) ? extraLocalRules.Rules : null);
		}

		
		public static void AddTextRequest(string localKeyword, Action<string> setter, List<Rule> extraLocalRules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add a text request while not resolving any quest.", false);
				return;
			}
			QuestTextRequest questTextRequest = new QuestTextRequest();
			questTextRequest.keyword = localKeyword;
			if (!QuestGen.slate.CurrentPrefix.NullOrEmpty())
			{
				questTextRequest.keyword = QuestGen.slate.CurrentPrefix + "/" + questTextRequest.keyword;
			}
			questTextRequest.keyword = QuestGenUtility.NormalizeVarPath(questTextRequest.keyword);
			questTextRequest.setter = setter;
			if (extraLocalRules != null)
			{
				questTextRequest.extraRules = QuestGenUtility.AppendCurrentPrefix(extraLocalRules);
			}
			QuestGen.textRequests.Add(questTextRequest);
		}

		
		private static void AddSlateQuestTags()
		{
			for (int i = 0; i < QuestGen.slateQuestTagsToAddWhenFinished.Count; i++)
			{
				object obj;
				if (QuestGen.slate.TryGet<object>(QuestGen.slateQuestTagsToAddWhenFinished[i], out obj, true))
				{
					string questTagToAdd = QuestGen.GenerateNewTargetQuestTag(QuestGen.slateQuestTagsToAddWhenFinished[i], false);
					QuestUtility.AddQuestTag(obj, questTagToAdd);
				}
			}
			QuestGen.slateQuestTagsToAddWhenFinished.Clear();
		}

		
		public static Quest quest;

		
		public static Slate slate = new Slate();

		
		private static QuestScriptDef root;

		
		private static bool working;

		
		private static List<QuestTextRequest> textRequests = new List<QuestTextRequest>();

		
		private static List<Pawn> generatedPawns = new List<Pawn>();

		
		private static Dictionary<string, int> generatedSignals = new Dictionary<string, int>();

		
		private static Dictionary<string, int> generatedTargetQuestTags = new Dictionary<string, int>();

		
		private static List<Rule> questDescriptionRules = new List<Rule>();

		
		private static Dictionary<string, string> questDescriptionConstants = new Dictionary<string, string>();

		
		private static List<Rule> questNameRules = new List<Rule>();

		
		private static Dictionary<string, string> questNameConstants = new Dictionary<string, string>();

		
		private static List<string> slateQuestTagsToAddWhenFinished = new List<string>();
	}
}
