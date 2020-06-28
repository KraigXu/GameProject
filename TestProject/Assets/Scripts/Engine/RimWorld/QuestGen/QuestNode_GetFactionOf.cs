using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001125 RID: 4389
	public class QuestNode_GetFactionOf : QuestNode
	{
		// Token: 0x060066AB RID: 26283 RVA: 0x0023F1A3 File Offset: 0x0023D3A3
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x060066AC RID: 26284 RVA: 0x0023F1AD File Offset: 0x0023D3AD
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060066AD RID: 26285 RVA: 0x0023F1BC File Offset: 0x0023D3BC
		private void DoWork(Slate slate)
		{
			Faction var = null;
			Thing value = this.thing.GetValue(slate);
			if (value != null)
			{
				var = value.Faction;
			}
			slate.Set<Faction>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x04003ED6 RID: 16086
		public SlateRef<Thing> thing;

		// Token: 0x04003ED7 RID: 16087
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
