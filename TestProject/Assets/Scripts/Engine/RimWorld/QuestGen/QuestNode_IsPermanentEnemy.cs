using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001152 RID: 4434
	public class QuestNode_IsPermanentEnemy : QuestNode
	{
		// Token: 0x0600676B RID: 26475 RVA: 0x002431B6 File Offset: 0x002413B6
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsPermanentEnemy(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600676C RID: 26476 RVA: 0x002431EE File Offset: 0x002413EE
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

		// Token: 0x0600676D RID: 26477 RVA: 0x00243224 File Offset: 0x00241424
		private bool IsPermanentEnemy(Slate slate)
		{
			Thing value = this.thing.GetValue(slate);
			return value != null && value.Faction != null && value.Faction.def.permanentEnemy;
		}

		// Token: 0x04003F98 RID: 16280
		public SlateRef<Thing> thing;

		// Token: 0x04003F99 RID: 16281
		public QuestNode node;

		// Token: 0x04003F9A RID: 16282
		public QuestNode elseNode;
	}
}
