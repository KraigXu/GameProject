using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001150 RID: 4432
	public class QuestNode_IsOfFaction : QuestNode
	{
		// Token: 0x06006763 RID: 26467 RVA: 0x00243069 File Offset: 0x00241269
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsOfFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06006764 RID: 26468 RVA: 0x002430A1 File Offset: 0x002412A1
		protected override void RunInt()
		{
			if (this.IsOfFaction(QuestGen.slate))
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

		// Token: 0x06006765 RID: 26469 RVA: 0x002430D7 File Offset: 0x002412D7
		private bool IsOfFaction(Slate slate)
		{
			return this.thing.GetValue(slate) != null && this.thing.GetValue(slate).Faction == this.faction.GetValue(slate);
		}

		// Token: 0x04003F91 RID: 16273
		public SlateRef<Thing> thing;

		// Token: 0x04003F92 RID: 16274
		public SlateRef<Faction> faction;

		// Token: 0x04003F93 RID: 16275
		public QuestNode node;

		// Token: 0x04003F94 RID: 16276
		public QuestNode elseNode;
	}
}
