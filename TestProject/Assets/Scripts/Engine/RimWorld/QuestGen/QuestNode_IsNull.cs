﻿using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_IsNull : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate) == null)
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate) == null)
			{
				if (this.node != null)
				{
					this.node.Run();
					return;
				}
			}
			else if (this.elseNode != null)
			{
				this.elseNode.Run();
			}
		}

		
		[NoTranslate]
		public SlateRef<object> value;

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
