using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200111B RID: 4379
	public class QuestNode_GenerateWorldObject : QuestNode
	{
		// Token: 0x06006682 RID: 26242 RVA: 0x0023E810 File Offset: 0x0023CA10
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

		// Token: 0x06006683 RID: 26243 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04003EAC RID: 16044
		public SlateRef<WorldObjectDef> def;

		// Token: 0x04003EAD RID: 16045
		public SlateRef<int> tile;

		// Token: 0x04003EAE RID: 16046
		public SlateRef<Faction> faction;

		// Token: 0x04003EAF RID: 16047
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
