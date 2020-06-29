using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_NoWorldObject : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_NoWorldObject questPart_NoWorldObject = new QuestPart_NoWorldObject();
			questPart_NoWorldObject.worldObject = this.worldObject.GetValue(slate);
			questPart_NoWorldObject.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_NoWorldObject);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_NoWorldObject.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_NoWorldObject);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		
		public SlateRef<WorldObject> worldObject;

		
		public QuestNode node;
	}
}
