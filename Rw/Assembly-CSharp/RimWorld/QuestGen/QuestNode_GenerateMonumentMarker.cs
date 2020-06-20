using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001115 RID: 4373
	public class QuestNode_GenerateMonumentMarker : QuestNode
	{
		// Token: 0x06006670 RID: 26224 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006671 RID: 26225 RVA: 0x0023DE80 File Offset: 0x0023C080
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MonumentMarker monumentMarker = (MonumentMarker)ThingMaker.MakeThing(ThingDefOf.MonumentMarker, null);
			monumentMarker.sketch = this.sketch.GetValue(slate);
			slate.Set<MonumentMarker>(this.storeAs.GetValue(slate), monumentMarker, false);
		}

		// Token: 0x04003E83 RID: 16003
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003E84 RID: 16004
		public SlateRef<Sketch> sketch;
	}
}
