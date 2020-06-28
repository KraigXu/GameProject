using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020011A8 RID: 4520
	public class QuestNode_GameCondition : QuestNode
	{
		// Token: 0x0600688B RID: 26763 RVA: 0x00247F2C File Offset: 0x0024612C
		private static Map GetMap(Slate slate)
		{
			Map randomPlayerHomeMap;
			if (!slate.TryGet<Map>("map", out randomPlayerHomeMap, false))
			{
				randomPlayerHomeMap = Find.RandomPlayerHomeMap;
			}
			return randomPlayerHomeMap;
		}

		// Token: 0x0600688C RID: 26764 RVA: 0x00247F50 File Offset: 0x00246150
		protected override bool TestRunInt(Slate slate)
		{
			return this.targetWorld.GetValue(slate) || QuestNode_GameCondition.GetMap(slate) != null;
		}

		// Token: 0x0600688D RID: 26765 RVA: 0x00247F6C File Offset: 0x0024616C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			float points = QuestGen.slate.Get<float>("points", 0f, false);
			GameCondition gameCondition = GameConditionMaker.MakeCondition(this.gameCondition.GetValue(slate), this.duration.GetValue(slate));
			QuestPart_GameCondition questPart_GameCondition = new QuestPart_GameCondition();
			questPart_GameCondition.gameCondition = gameCondition;
			List<Rule> list = new List<Rule>();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (this.targetWorld.GetValue(slate))
			{
				questPart_GameCondition.targetWorld = true;
				gameCondition.RandomizeSettings(points, null, list, dictionary);
			}
			else
			{
				Map map = QuestNode_GameCondition.GetMap(QuestGen.slate);
				questPart_GameCondition.mapParent = map.Parent;
				gameCondition.RandomizeSettings(points, map, list, dictionary);
			}
			questPart_GameCondition.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_GameCondition);
			if (!this.storeGameConditionDescriptionFutureAs.GetValue(slate).NullOrEmpty())
			{
				slate.Set<string>(this.storeGameConditionDescriptionFutureAs.GetValue(slate), gameCondition.def.descriptionFuture, false);
			}
			QuestGen.AddQuestNameRules(list);
			QuestGen.AddQuestDescriptionRules(list);
			QuestGen.AddQuestDescriptionConstants(dictionary);
		}

		// Token: 0x040040DC RID: 16604
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040040DD RID: 16605
		public SlateRef<GameConditionDef> gameCondition;

		// Token: 0x040040DE RID: 16606
		public SlateRef<bool> targetWorld;

		// Token: 0x040040DF RID: 16607
		public SlateRef<int> duration;

		// Token: 0x040040E0 RID: 16608
		[NoTranslate]
		public SlateRef<string> storeGameConditionDescriptionFutureAs;
	}
}
