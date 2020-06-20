using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200118D RID: 4493
	public class QuestNode_SetRoyalTitle : QuestNode
	{
		// Token: 0x0600682C RID: 26668 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600682D RID: 26669 RVA: 0x00246610 File Offset: 0x00244810
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Pawn value = this.pawn.GetValue(slate);
			if (value.royalty != null)
			{
				value.royalty.SetTitle(this.faction.GetValue(slate), this.royalTitle.GetValue(slate), false, false, true);
			}
		}

		// Token: 0x04004075 RID: 16501
		public SlateRef<Pawn> pawn;

		// Token: 0x04004076 RID: 16502
		public SlateRef<RoyalTitleDef> royalTitle;

		// Token: 0x04004077 RID: 16503
		public SlateRef<Faction> faction;
	}
}
