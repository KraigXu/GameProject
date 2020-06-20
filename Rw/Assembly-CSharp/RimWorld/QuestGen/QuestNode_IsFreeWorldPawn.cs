using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200114F RID: 4431
	public class QuestNode_IsFreeWorldPawn : QuestNode
	{
		// Token: 0x0600675F RID: 26463 RVA: 0x00242FD0 File Offset: 0x002411D0
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsFreeWorldPawn(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06006760 RID: 26464 RVA: 0x00243008 File Offset: 0x00241208
		protected override void RunInt()
		{
			if (this.IsFreeWorldPawn(QuestGen.slate))
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

		// Token: 0x06006761 RID: 26465 RVA: 0x0024303E File Offset: 0x0024123E
		private bool IsFreeWorldPawn(Slate slate)
		{
			return this.pawn.GetValue(slate) != null && Find.WorldPawns.GetSituation(this.pawn.GetValue(slate)) == WorldPawnSituation.Free;
		}

		// Token: 0x04003F8E RID: 16270
		public SlateRef<Pawn> pawn;

		// Token: 0x04003F8F RID: 16271
		public QuestNode node;

		// Token: 0x04003F90 RID: 16272
		public QuestNode elseNode;
	}
}
