using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001154 RID: 4436
	public class QuestNode_ViolentQuestsAllowed : QuestNode
	{
		// Token: 0x06006772 RID: 26482 RVA: 0x00243270 File Offset: 0x00241470
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate, (QuestNode n) => n.TestRun(slate));
		}

		// Token: 0x06006773 RID: 26483 RVA: 0x002432A2 File Offset: 0x002414A2
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate, delegate(QuestNode n)
			{
				n.Run();
				return true;
			});
		}

		// Token: 0x06006774 RID: 26484 RVA: 0x002432D0 File Offset: 0x002414D0
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

		// Token: 0x04003F9C RID: 16284
		public QuestNode node;

		// Token: 0x04003F9D RID: 16285
		public QuestNode elseNode;
	}
}
