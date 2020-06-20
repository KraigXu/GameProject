using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001121 RID: 4385
	public class QuestNode_GetFreeColonistsCount : QuestNode
	{
		// Token: 0x06006699 RID: 26265 RVA: 0x0023EC70 File Offset: 0x0023CE70
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600669A RID: 26266 RVA: 0x0023EC7A File Offset: 0x0023CE7A
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600669B RID: 26267 RVA: 0x0023EC88 File Offset: 0x0023CE88
		private void SetVars(Slate slate)
		{
			int var;
			if (this.onlyThisMap.GetValue(slate) != null)
			{
				var = this.onlyThisMap.GetValue(slate).mapPawns.FreeColonistsCount;
			}
			else
			{
				var = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count;
			}
			slate.Set<int>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x04003EBE RID: 16062
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003EBF RID: 16063
		public SlateRef<Map> onlyThisMap;
	}
}
