using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200114E RID: 4430
	public class QuestNode_IsFactionLeader : QuestNode
	{
		// Token: 0x0600675B RID: 26459 RVA: 0x00242F0B File Offset: 0x0024110B
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsFactionLeader(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600675C RID: 26460 RVA: 0x00242F43 File Offset: 0x00241143
		protected override void RunInt()
		{
			if (this.IsFactionLeader(QuestGen.slate))
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

		// Token: 0x0600675D RID: 26461 RVA: 0x00242F7C File Offset: 0x0024117C
		private bool IsFactionLeader(Slate slate)
		{
			return this.pawn.GetValue(slate) != null && this.pawn.GetValue(slate).Faction != null && this.pawn.GetValue(slate).Faction.leader == this.pawn.GetValue(slate);
		}

		// Token: 0x04003F8B RID: 16267
		public SlateRef<Pawn> pawn;

		// Token: 0x04003F8C RID: 16268
		public QuestNode node;

		// Token: 0x04003F8D RID: 16269
		public QuestNode elseNode;
	}
}
