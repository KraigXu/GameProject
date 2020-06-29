using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_Delay : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Delay questPart_Delay;
			if (this.delayTicksRange.GetValue(slate) != null)
			{
				questPart_Delay = new QuestPart_DelayRandom();
				((QuestPart_DelayRandom)questPart_Delay).delayTicksRange = this.delayTicksRange.GetValue(slate).Value;
			}
			else
			{
				questPart_Delay = this.MakeDelayQuestPart();
				questPart_Delay.delayTicks = this.delayTicks.GetValue(slate);
			}
			questPart_Delay.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Delay.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_Delay.reactivatable = this.reactivatable.GetValue(slate);
			if (!this.inspectStringTargets.GetValue(slate).EnumerableNullOrEmpty<ISelectable>())
			{
				questPart_Delay.inspectString = this.inspectString.GetValue(slate);
				questPart_Delay.inspectStringTargets = new List<ISelectable>();
				questPart_Delay.inspectStringTargets.AddRange(this.inspectStringTargets.GetValue(slate));
			}
			if (this.isQuestTimeout.GetValue(slate))
			{
				questPart_Delay.isBad = true;
				questPart_Delay.expiryInfoPart = "QuestExpiresIn".Translate();
				questPart_Delay.expiryInfoPartTip = "QuestExpiresOn".Translate();
			}
			else
			{
				questPart_Delay.expiryInfoPart = this.expiryInfoPart.GetValue(slate);
				questPart_Delay.expiryInfoPartTip = this.expiryInfoPartTip.GetValue(slate);
			}
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_Delay);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_Delay.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_Delay);
		}

		
		protected virtual QuestPart_Delay MakeDelayQuestPart()
		{
			return new QuestPart_Delay();
		}

		
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		
		public SlateRef<string> expiryInfoPart;

		
		public SlateRef<string> expiryInfoPartTip;

		
		public SlateRef<string> inspectString;

		
		public SlateRef<IEnumerable<ISelectable>> inspectStringTargets;

		
		public SlateRef<int> delayTicks;

		
		public SlateRef<IntRange?> delayTicksRange;

		
		public SlateRef<bool> isQuestTimeout;

		
		public SlateRef<bool> reactivatable;

		
		public QuestNode node;
	}
}
