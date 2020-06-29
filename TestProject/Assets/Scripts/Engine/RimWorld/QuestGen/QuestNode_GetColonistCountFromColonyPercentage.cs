using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetColonistCountFromColonyPercentage : QuestNode
	{
		
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		
		private void SetVars(Slate slate)
		{
			string value = this.storeAs.GetValue(slate);
			int num = PawnsFinder.AllMaps_FreeColonistsSpawned.Count((Pawn c) => !c.IsQuestLodger());
			int var = Mathf.Clamp((int)((float)num * this.colonyPercentage.GetValue(slate)), 1, num - 1);
			slate.Set<int>(value, var, false);
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<float> colonyPercentage;

		
		public SlateRef<int> mustHaveFreeColonistsAvailableCount;
	}
}
