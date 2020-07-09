using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_JoinPlayer : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_JoinPlayer questPart_JoinPlayer = new QuestPart_JoinPlayer();
			questPart_JoinPlayer.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_JoinPlayer.joinPlayer = this.joinPlayer.GetValue(slate);
			questPart_JoinPlayer.makePrisoners = this.makePrisoners.GetValue(slate);
			questPart_JoinPlayer.mapParent = QuestGen.slate.Get<Map>("map", null, false).Parent;
			questPart_JoinPlayer.pawns.AddRange(this.pawns.GetValue(slate));
			QuestGen.quest.AddPart(questPart_JoinPlayer);
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<IEnumerable<Pawn>> pawns;

		
		public SlateRef<bool> joinPlayer;

		
		public SlateRef<bool> makePrisoners;
	}
}
