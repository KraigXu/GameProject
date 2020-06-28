using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001153 RID: 4435
	public class QuestNode_RequireRoyalFavorFromFaction : QuestNode
	{
		// Token: 0x0600676F RID: 26479 RVA: 0x0024325B File Offset: 0x0024145B
		protected override bool TestRunInt(Slate slate)
		{
			return this.faction.GetValue(slate).allowRoyalFavorRewards;
		}

		// Token: 0x06006770 RID: 26480 RVA: 0x00002681 File Offset: 0x00000881
		protected override void RunInt()
		{
		}

		// Token: 0x04003F9B RID: 16283
		public SlateRef<Faction> faction;
	}
}
