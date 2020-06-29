using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GenerateWorldObject : QuestNode
	{
		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			WorldObject worldObject = WorldObjectMaker.MakeWorldObject(this.def.GetValue(slate));
			worldObject.Tile = this.tile.GetValue(slate);
			if (this.faction.GetValue(slate) != null)
			{
				worldObject.SetFaction(this.faction.GetValue(slate));
			}
			if (this.storeAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<WorldObject>(this.storeAs.GetValue(slate), worldObject, false);
			}
		}

		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		public SlateRef<WorldObjectDef> def;

		
		public SlateRef<int> tile;

		
		public SlateRef<Faction> faction;

		
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
