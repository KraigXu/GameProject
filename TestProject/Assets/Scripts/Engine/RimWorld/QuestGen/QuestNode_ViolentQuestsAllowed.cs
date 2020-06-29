using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_ViolentQuestsAllowed : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate, (QuestNode n) => n.TestRun(slate));
		}

		
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate, delegate(QuestNode n)
			{
				n.Run();
				return true;
			});
		}

		
		private bool DoWork(Slate slate, Func<QuestNode, bool> func)
		{
			bool allowViolentQuests = Find.Storyteller.difficulty.allowViolentQuests;
			slate.Set<bool>("allowViolentQuests", allowViolentQuests, false);
			if (allowViolentQuests)
			{
				if (this.node != null)
				{
					return func(this.node);
				}
			}
			else if (this.elseNode != null)
			{
				return func(this.elseNode);
			}
			return true;
		}

		
		public QuestNode node;

		
		public QuestNode elseNode;
	}
}
