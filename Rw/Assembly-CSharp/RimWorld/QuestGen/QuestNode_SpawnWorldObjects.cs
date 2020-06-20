using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001194 RID: 4500
	public class QuestNode_SpawnWorldObjects : QuestNode
	{
		// Token: 0x06006842 RID: 26690 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006843 RID: 26691 RVA: 0x00246CD4 File Offset: 0x00244ED4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.worldObjects.GetValue(slate) == null)
			{
				return;
			}
			string text = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false);
			foreach (WorldObject worldObject in this.worldObjects.GetValue(slate))
			{
				QuestPart_SpawnWorldObject questPart_SpawnWorldObject = new QuestPart_SpawnWorldObject();
				questPart_SpawnWorldObject.worldObject = worldObject;
				questPart_SpawnWorldObject.inSignal = text;
				questPart_SpawnWorldObject.defsToExcludeFromHyperlinks = this.defsToExcludeFromHyperlinks.GetValue(slate);
				if (this.tile.GetValue(slate) != null)
				{
					worldObject.Tile = this.tile.GetValue(slate).Value;
				}
				QuestGen.quest.AddPart(questPart_SpawnWorldObject);
			}
		}

		// Token: 0x04004093 RID: 16531
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04004094 RID: 16532
		public SlateRef<IEnumerable<WorldObject>> worldObjects;

		// Token: 0x04004095 RID: 16533
		public SlateRef<int?> tile;

		// Token: 0x04004096 RID: 16534
		public SlateRef<List<ThingDef>> defsToExcludeFromHyperlinks;
	}
}
