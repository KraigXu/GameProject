using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200113A RID: 4410
	public class QuestNode_GetRandomElement : QuestNode
	{
		// Token: 0x06006708 RID: 26376 RVA: 0x002414E3 File Offset: 0x0023F6E3
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06006709 RID: 26377 RVA: 0x002414ED File Offset: 0x0023F6ED
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600670A RID: 26378 RVA: 0x002414FC File Offset: 0x0023F6FC
		private void SetVars(Slate slate)
		{
			SlateRef<object> slateRef;
			if (this.options.TryRandomElement(out slateRef))
			{
				slate.Set<object>(this.storeAs.GetValue(slate), slateRef.GetValue(slate), false);
			}
		}

		// Token: 0x04003F32 RID: 16178
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003F33 RID: 16179
		public List<SlateRef<object>> options;
	}
}
