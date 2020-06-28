using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001120 RID: 4384
	public class QuestNode_GetColonistCountFromColonyPercentage : QuestNode
	{
		// Token: 0x06006695 RID: 26261 RVA: 0x0023EB9C File Offset: 0x0023CD9C
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06006696 RID: 26262 RVA: 0x0023EBAC File Offset: 0x0023CDAC
		private void SetVars(Slate slate)
		{
			string value = this.storeAs.GetValue(slate);
			int num = PawnsFinder.AllMaps_FreeColonistsSpawned.Count((Pawn c) => !c.IsQuestLodger());
			int var = Mathf.Clamp((int)((float)num * this.colonyPercentage.GetValue(slate)), 1, num - 1);
			slate.Set<int>(value, var, false);
		}

		// Token: 0x06006697 RID: 26263 RVA: 0x0023EC14 File Offset: 0x0023CE14
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			float num = (float)this.mustHaveFreeColonistsAvailableCount.GetValue(slate);
			if (num > 0f)
			{
				return (float)PawnsFinder.AllMaps_FreeColonistsSpawned.Count((Pawn c) => !c.IsQuestLodger()) >= num;
			}
			return true;
		}

		// Token: 0x04003EBB RID: 16059
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003EBC RID: 16060
		public SlateRef<float> colonyPercentage;

		// Token: 0x04003EBD RID: 16061
		public SlateRef<int> mustHaveFreeColonistsAvailableCount;
	}
}
