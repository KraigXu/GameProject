using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetHivesCountFromPoints : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		
		private void SetVars(Slate slate)
		{
			int num = Mathf.RoundToInt(slate.Get<float>("points", 0f, false) / 220f);
			if (num < 1)
			{
				num = 1;
			}
			slate.Set<int>(this.storeAs.GetValue(slate), num, false);
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
