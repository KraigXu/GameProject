using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200113B RID: 4411
	public class QuestNode_GetRandomElementByWeight : QuestNode
	{
		// Token: 0x0600670C RID: 26380 RVA: 0x00241533 File Offset: 0x0023F733
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600670D RID: 26381 RVA: 0x0024153D File Offset: 0x0023F73D
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600670E RID: 26382 RVA: 0x0024154C File Offset: 0x0023F74C
		private void SetVars(Slate slate)
		{
			QuestNode_GetRandomElementByWeight.Option option;
			if (this.options.TryRandomElementByWeight((QuestNode_GetRandomElementByWeight.Option x) => x.weight, out option))
			{
				slate.Set<object>(this.storeAs.GetValue(slate), option.element.GetValue(slate), false);
			}
		}

		// Token: 0x04003F34 RID: 16180
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003F35 RID: 16181
		public List<QuestNode_GetRandomElementByWeight.Option> options = new List<QuestNode_GetRandomElementByWeight.Option>();

		// Token: 0x02001F37 RID: 7991
		public class Option
		{
			// Token: 0x0400751E RID: 29982
			public SlateRef<object> element;

			// Token: 0x0400751F RID: 29983
			public float weight;
		}
	}
}
