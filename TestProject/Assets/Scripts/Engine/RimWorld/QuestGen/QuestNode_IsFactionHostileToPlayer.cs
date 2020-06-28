using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200114D RID: 4429
	public class QuestNode_IsFactionHostileToPlayer : QuestNode
	{
		// Token: 0x06006757 RID: 26455 RVA: 0x00242E48 File Offset: 0x00241048
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsHostile(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06006758 RID: 26456 RVA: 0x00242E80 File Offset: 0x00241080
		protected override void RunInt()
		{
			if (this.IsHostile(QuestGen.slate))
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

		// Token: 0x06006759 RID: 26457 RVA: 0x00242EB8 File Offset: 0x002410B8
		private bool IsHostile(Slate slate)
		{
			Faction value = this.faction.GetValue(slate);
			if (value != null)
			{
				return value.HostileTo(Faction.OfPlayer);
			}
			Thing value2 = this.factionOf.GetValue(slate);
			return value2 != null && value2.Faction != null && value2.Faction.HostileTo(Faction.OfPlayer);
		}

		// Token: 0x04003F87 RID: 16263
		public SlateRef<Faction> faction;

		// Token: 0x04003F88 RID: 16264
		public SlateRef<Thing> factionOf;

		// Token: 0x04003F89 RID: 16265
		public QuestNode node;

		// Token: 0x04003F8A RID: 16266
		public QuestNode elseNode;
	}
}
