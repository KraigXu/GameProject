using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200114C RID: 4428
	public class QuestNode_HasRoyalTitleInCurrentFaction : QuestNode
	{
		// Token: 0x06006753 RID: 26451 RVA: 0x00242D98 File Offset: 0x00240F98
		protected override bool TestRunInt(Slate slate)
		{
			if (this.HasRoyalTitleInCurrentFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06006754 RID: 26452 RVA: 0x00242DD0 File Offset: 0x00240FD0
		protected override void RunInt()
		{
			if (this.HasRoyalTitleInCurrentFaction(QuestGen.slate))
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

		// Token: 0x06006755 RID: 26453 RVA: 0x00242E08 File Offset: 0x00241008
		private bool HasRoyalTitleInCurrentFaction(Slate slate)
		{
			Pawn value = this.pawn.GetValue(slate);
			return value != null && value.Faction != null && value.royalty != null && value.royalty.HasAnyTitleIn(value.Faction);
		}

		// Token: 0x04003F84 RID: 16260
		public SlateRef<Pawn> pawn;

		// Token: 0x04003F85 RID: 16261
		public QuestNode node;

		// Token: 0x04003F86 RID: 16262
		public QuestNode elseNode;
	}
}
