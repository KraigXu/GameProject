using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x020001CE RID: 462
	public class RoofCollapseBufferResolver
	{
		// Token: 0x06000D1D RID: 3357 RVA: 0x0004A9DE File Offset: 0x00048BDE
		public RoofCollapseBufferResolver(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0004AA04 File Offset: 0x00048C04
		public void CollapseRoofsMarkedToCollapse()
		{
			RoofCollapseBuffer roofCollapseBuffer = this.map.roofCollapseBuffer;
			if (roofCollapseBuffer.CellsMarkedToCollapse.Any<IntVec3>())
			{
				this.tmpCrushedThings.Clear();
				RoofCollapserImmediate.DropRoofInCells(roofCollapseBuffer.CellsMarkedToCollapse, this.map, this.tmpCrushedThings);
				if (this.tmpCrushedThings.Any<Thing>())
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("RoofCollapsed".Translate());
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("TheseThingsCrushed".Translate());
					this.tmpCrushedNames.Clear();
					for (int i = 0; i < this.tmpCrushedThings.Count; i++)
					{
						Thing thing = this.tmpCrushedThings[i];
						string item = thing.LabelShortCap;
						if (thing.def.category == ThingCategory.Pawn)
						{
							item = thing.LabelCap;
						}
						if (!this.tmpCrushedNames.Contains(item))
						{
							this.tmpCrushedNames.Add(item);
						}
					}
					foreach (string str in this.tmpCrushedNames)
					{
						stringBuilder.AppendLine("    -" + str);
					}
					Find.LetterStack.ReceiveLetter("LetterLabelRoofCollapsed".Translate(), stringBuilder.ToString().TrimEndNewlines(), LetterDefOf.NegativeEvent, new TargetInfo(roofCollapseBuffer.CellsMarkedToCollapse[0], this.map, false), null, null, null, null);
				}
				else
				{
					Messages.Message("RoofCollapsed".Translate(), new TargetInfo(roofCollapseBuffer.CellsMarkedToCollapse[0], this.map, false), MessageTypeDefOf.SilentInput, true);
				}
				this.tmpCrushedThings.Clear();
				roofCollapseBuffer.Clear();
			}
		}

		// Token: 0x04000A2E RID: 2606
		private Map map;

		// Token: 0x04000A2F RID: 2607
		private List<Thing> tmpCrushedThings = new List<Thing>();

		// Token: 0x04000A30 RID: 2608
		private HashSet<string> tmpCrushedNames = new HashSet<string>();
	}
}
