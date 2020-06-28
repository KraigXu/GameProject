using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001151 RID: 4433
	public class QuestNode_IsOfRoyalFaction : QuestNode
	{
		// Token: 0x06006767 RID: 26471 RVA: 0x00243108 File Offset: 0x00241308
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsOfRoyalFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06006768 RID: 26472 RVA: 0x00243140 File Offset: 0x00241340
		protected override void RunInt()
		{
			if (this.IsOfRoyalFaction(QuestGen.slate))
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

		// Token: 0x06006769 RID: 26473 RVA: 0x00243176 File Offset: 0x00241376
		private bool IsOfRoyalFaction(Slate slate)
		{
			return this.thing.GetValue(slate) != null && this.thing.GetValue(slate).Faction != null && this.thing.GetValue(slate).Faction.def.HasRoyalTitles;
		}

		// Token: 0x04003F95 RID: 16277
		public SlateRef<Thing> thing;

		// Token: 0x04003F96 RID: 16278
		public QuestNode node;

		// Token: 0x04003F97 RID: 16279
		public QuestNode elseNode;
	}
}
