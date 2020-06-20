using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001116 RID: 4374
	public class QuestNode_GenerateNonBuildableMonumentRequiredResources : QuestNode
	{
		// Token: 0x06006673 RID: 26227 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006674 RID: 26228 RVA: 0x0023DECC File Offset: 0x0023C0CC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MonumentMarker value = this.monumentMarker.GetValue(slate);
			for (int i = 0; i < value.sketch.Things.Count; i++)
			{
				ThingDef def = value.sketch.Things[i].def;
				ThingDef stuff = value.sketch.Things[i].stuff;
				if (def.category == ThingCategory.Building && !def.BuildableByPlayer)
				{
					MinifiedThing obj = ThingMaker.MakeThing(def, stuff).MakeMinified();
					QuestGenUtility.AddToOrMakeList(QuestGen.slate, this.addToList.GetValue(slate), obj);
				}
			}
		}

		// Token: 0x04003E85 RID: 16005
		[NoTranslate]
		public SlateRef<string> addToList;

		// Token: 0x04003E86 RID: 16006
		public SlateRef<MonumentMarker> monumentMarker;
	}
}
