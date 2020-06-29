using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_IsPermanentEnemy : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsPermanentEnemy(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			if (this.IsPermanentEnemy(QuestGen.slate))
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

		
		private bool IsPermanentEnemy(Slate slate)
		{
			Thing value = this.thing.GetValue(slate);
			return value != null && value.Faction != null && value.Faction.def.permanentEnemy;
		}

		
		public SlateRef<Thing> thing;

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
