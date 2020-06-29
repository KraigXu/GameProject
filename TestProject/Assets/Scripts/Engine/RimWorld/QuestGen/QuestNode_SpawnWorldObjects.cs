using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_SpawnWorldObjects : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<IEnumerable<WorldObject>> worldObjects;

		
		public SlateRef<int?> tile;

		
		public SlateRef<List<ThingDef>> defsToExcludeFromHyperlinks;
	}
}
