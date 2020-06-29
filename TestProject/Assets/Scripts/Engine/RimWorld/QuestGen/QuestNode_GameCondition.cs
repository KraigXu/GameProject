using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GameCondition : QuestNode
	{
		
		private static Map GetMap(Slate slate)
		{
			Map randomPlayerHomeMap;
			if (!slate.TryGet<Map>("map", out randomPlayerHomeMap, false))
			{
				randomPlayerHomeMap = Find.RandomPlayerHomeMap;
			}
			return randomPlayerHomeMap;
		}

		
		protected override bool TestRunInt(Slate slate)
		{
			return this.targetWorld.GetValue(slate) || QuestNode_GameCondition.GetMap(slate) != null;
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<GameConditionDef> gameCondition;

		
		public SlateRef<bool> targetWorld;

		
		public SlateRef<int> duration;

		
		[NoTranslate]
		public SlateRef<string> storeGameConditionDescriptionFutureAs;
	}
}
