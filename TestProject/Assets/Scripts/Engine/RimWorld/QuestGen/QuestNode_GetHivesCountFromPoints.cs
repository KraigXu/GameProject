using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001128 RID: 4392
	public class QuestNode_GetHivesCountFromPoints : QuestNode
	{
		// Token: 0x060066B7 RID: 26295 RVA: 0x0023F35E File Offset: 0x0023D55E
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x060066B8 RID: 26296 RVA: 0x0023F368 File Offset: 0x0023D568
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060066B9 RID: 26297 RVA: 0x0023F378 File Offset: 0x0023D578
		private void SetVars(Slate slate)
		{
			int num = Mathf.RoundToInt(slate.Get<float>("points", 0f, false) / 220f);
			if (num < 1)
			{
				num = 1;
			}
			slate.Set<int>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x04003EDF RID: 16095
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
