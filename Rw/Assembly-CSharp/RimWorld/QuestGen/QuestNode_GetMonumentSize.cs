using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200112F RID: 4399
	public class QuestNode_GetMonumentSize : QuestNode
	{
		// Token: 0x060066D7 RID: 26327 RVA: 0x00240058 File Offset: 0x0023E258
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x060066D8 RID: 26328 RVA: 0x00240062 File Offset: 0x0023E262
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060066D9 RID: 26329 RVA: 0x0024006F File Offset: 0x0023E26F
		private void DoWork(Slate slate)
		{
			if (this.monumentMarker.GetValue(slate) == null)
			{
				return;
			}
			slate.Set<IntVec2>(this.storeAs.GetValue(slate), this.monumentMarker.GetValue(slate).Size, false);
		}

		// Token: 0x04003EF0 RID: 16112
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003EF1 RID: 16113
		public SlateRef<MonumentMarker> monumentMarker;
	}
}
