using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020010E8 RID: 4328
	public static class QuestGen
	{
		// Token: 0x17001164 RID: 4452
		// (get) Token: 0x060065BD RID: 26045 RVA: 0x0023A42B File Offset: 0x0023862B
		public static QuestScriptDef Root
		{
			get
			{
				return QuestGen.root;
			}
		}

		// Token: 0x17001165 RID: 4453
		// (get) Token: 0x060065BE RID: 26046 RVA: 0x0023A432 File Offset: 0x00238632
		public static bool Working
		{
			get
			{
				return QuestGen.working;
			}
		}

		// Token: 0x17001166 RID: 4454
		// (get) Token: 0x060065BF RID: 26047 RVA: 0x0023A439 File Offset: 0x00238639
		public static List<Rule> QuestDescriptionRulesReadOnly
		{
			get
			{
				return QuestGen.questDescriptionRules;
			}
		}

		// Token: 0x17001167 RID: 4455
		// (get) Token: 0x060065C0 RID: 26048 RVA: 0x0023A440 File Offset: 0x00238640
		public static Dictionary<string, string> QuestDescriptionConstantsReadOnly
		{
			get
			{
				return QuestGen.questDescriptionConstants;
			}
		}

		// Token: 0x17001168 RID: 4456
		// (get) Token: 0x060065C1 RID: 26049 RVA: 0x0023A447 File Offset: 0x00238647
		public static List<Rule> QuestNameRulesReadOnly
		{
			get
			{
				return QuestGen.questNameRules;
			}
		}

		// Token: 0x17001169 RID: 4457
		// (get) Token: 0x060065C2 RID: 26050 RVA: 0x0023A44E File Offset: 0x0023864E
		public static Dictionary<string, string> QuestNameConstantsReadOnly
		{
			get
			{
				return QuestGen.questNameConstants;
			}
		}

		// Token: 0x1700116A RID: 4458
		// (get) Token: 0x060065C3 RID: 26051 RVA: 0x0023A455 File Offset: 0x00238655
		public static List<QuestTextRequest> TextRequestsReadOnly
		{
			get
			{
				return QuestGen.textRequests;
			}
		}

		// Token: 0x060065C4 RID: 26052 RVA: 0x0023A45C File Offset: 0x0023865C
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

		// Token: 0x060065C5 RID: 26053 RVA: 0x0023A4D4 File Offset: 0x002386D4
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

		// Token: 0x060065C6 RID: 26054 RVA: 0x0023A54C File Offset: 0x0023874C
		private static void ResetIdCounters()
		{
			QuestGen.generatedSignals.Clear();
			QuestGen.generatedTargetQuestTags.Clear();
		}

		// Token: 0x060065C7 RID: 26055 RVA: 0x0023A564 File Offset: 0x00238764
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

		// Token: 0x060065C8 RID: 26056 RVA: 0x0023A7E0 File Offset: 0x002389E0
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

		// Token: 0x060065C9 RID: 26057 RVA: 0x0023A80D File Offset: 0x00238A0D
		public static bool WasGeneratedForQuestBeingGenerated(Pawn pawn)
		{
			return QuestGen.working && QuestGen.generatedPawns.Contains(pawn);
		}

		// Token: 0x060065CA RID: 26058 RVA: 0x0023A823 File Offset: 0x00238A23
		public static void AddQuestDescriptionRules(RulePack rulePack)
		{
			QuestGen.AddQuestDescriptionRules(rulePack.Rules);
		}

		// Token: 0x060065CB RID: 26059 RVA: 0x0023A830 File Offset: 0x00238A30
		public static void AddQuestDescriptionRules(List<Rule> rules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest description rules while not resolving any quest.", false);
				return;
			}
			QuestGen.questDescriptionRules.AddRange(QuestGenUtility.AppendCurrentPrefix(rules));
		}

		// Token: 0x060065CC RID: 26060 RVA: 0x0023A858 File Offset: 0x00238A58
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

		// Token: 0x060065CD RID: 26061 RVA: 0x0023A8E4 File Offset: 0x00238AE4
		public static void AddQuestNameRules(RulePack rulePack)
		{
			QuestGen.AddQuestNameRules(rulePack.Rules);
		}

		// Token: 0x060065CE RID: 26062 RVA: 0x0023A8F1 File Offset: 0x00238AF1
		public static void AddQuestNameRules(List<Rule> rules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest name rules while not resolving any quest.", false);
				return;
			}
			QuestGen.questNameRules.AddRange(QuestGenUtility.AppendCurrentPrefix(rules));
		}

		// Token: 0x060065CF RID: 26063 RVA: 0x0023A918 File Offset: 0x00238B18
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

		// Token: 0x060065D0 RID: 26064 RVA: 0x0023A9A4 File Offset: 0x00238BA4
		public static void AddSlateQuestTagToAddWhenFinished(string slateVarNameWithPrefix)
		{
			if (!QuestGen.slateQuestTagsToAddWhenFinished.Contains(slateVarNameWithPrefix))
			{
				QuestGen.slateQuestTagsToAddWhenFinished.Add(slateVarNameWithPrefix);
			}
		}

		// Token: 0x060065D1 RID: 26065 RVA: 0x0023A9BE File Offset: 0x00238BBE
		public static void AddTextRequest(string localKeyword, Action<string> setter, RulePack extraLocalRules = null)
		{
			QuestGen.AddTextRequest(localKeyword, setter, (extraLocalRules != null) ? extraLocalRules.Rules : null);
		}

		// Token: 0x060065D2 RID: 26066 RVA: 0x0023A9D4 File Offset: 0x00238BD4
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

		// Token: 0x060065D3 RID: 26067 RVA: 0x0023AA64 File Offset: 0x00238C64
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

		// Token: 0x04003DE1 RID: 15841
		public static Quest quest;

		// Token: 0x04003DE2 RID: 15842
		public static Slate slate = new Slate();

		// Token: 0x04003DE3 RID: 15843
		private static QuestScriptDef root;

		// Token: 0x04003DE4 RID: 15844
		private static bool working;

		// Token: 0x04003DE5 RID: 15845
		private static List<QuestTextRequest> textRequests = new List<QuestTextRequest>();

		// Token: 0x04003DE6 RID: 15846
		private static List<Pawn> generatedPawns = new List<Pawn>();

		// Token: 0x04003DE7 RID: 15847
		private static Dictionary<string, int> generatedSignals = new Dictionary<string, int>();

		// Token: 0x04003DE8 RID: 15848
		private static Dictionary<string, int> generatedTargetQuestTags = new Dictionary<string, int>();

		// Token: 0x04003DE9 RID: 15849
		private static List<Rule> questDescriptionRules = new List<Rule>();

		// Token: 0x04003DEA RID: 15850
		private static Dictionary<string, string> questDescriptionConstants = new Dictionary<string, string>();

		// Token: 0x04003DEB RID: 15851
		private static List<Rule> questNameRules = new List<Rule>();

		// Token: 0x04003DEC RID: 15852
		private static Dictionary<string, string> questNameConstants = new Dictionary<string, string>();

		// Token: 0x04003DED RID: 15853
		private static List<string> slateQuestTagsToAddWhenFinished = new List<string>();
	}
}
