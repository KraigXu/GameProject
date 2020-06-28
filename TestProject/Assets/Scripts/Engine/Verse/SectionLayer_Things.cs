using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200019C RID: 412
	public abstract class SectionLayer_Things : SectionLayer
	{
		// Token: 0x06000BB2 RID: 2994 RVA: 0x0004264A File Offset: 0x0004084A
		public SectionLayer_Things(Section section) : base(section)
		{
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00042653 File Offset: 0x00040853
		public override void DrawLayer()
		{
			if (!DebugViewSettings.drawThingsPrinted)
			{
				return;
			}
			base.DrawLayer();
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x00042664 File Offset: 0x00040864
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			foreach (IntVec3 intVec in this.section.CellRect)
			{
				List<Thing> list = base.Map.thingGrid.ThingsListAt(intVec);
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					Thing thing = list[i];
					if ((thing.def.seeThroughFog || !base.Map.fogGrid.fogGrid[CellIndicesUtility.CellToIndex(thing.Position, base.Map.Size.x)]) && thing.def.drawerType != DrawerType.None && (thing.def.drawerType != DrawerType.RealtimeOnly || !this.requireAddToMapMesh) && (thing.def.hideAtSnowDepth >= 1f || base.Map.snowGrid.GetDepth(thing.Position) <= thing.def.hideAtSnowDepth) && thing.Position.x == intVec.x && thing.Position.z == intVec.z)
					{
						this.TakePrintFrom(thing);
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x06000BB5 RID: 2997
		protected abstract void TakePrintFrom(Thing t);

		// Token: 0x0400095C RID: 2396
		protected bool requireAddToMapMesh;
	}
}
