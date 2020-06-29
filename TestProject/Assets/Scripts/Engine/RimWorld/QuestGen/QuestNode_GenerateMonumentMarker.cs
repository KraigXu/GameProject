using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GenerateMonumentMarker : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MonumentMarker monumentMarker = (MonumentMarker)ThingMaker.MakeThing(ThingDefOf.MonumentMarker, null);
			monumentMarker.sketch = this.sketch.GetValue(slate);
			slate.Set<MonumentMarker>(this.storeAs.GetValue(slate), monumentMarker, false);
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<Sketch> sketch;
	}
}
