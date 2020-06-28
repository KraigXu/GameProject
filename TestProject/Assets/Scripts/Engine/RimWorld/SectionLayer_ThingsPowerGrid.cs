using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A43 RID: 2627
	public class SectionLayer_ThingsPowerGrid : SectionLayer_Things
	{
		// Token: 0x06003E22 RID: 15906 RVA: 0x001474AD File Offset: 0x001456AD
		public SectionLayer_ThingsPowerGrid(Section section) : base(section)
		{
			this.requireAddToMapMesh = false;
			this.relevantChangeTypes = MapMeshFlag.PowerGrid;
		}

		// Token: 0x06003E23 RID: 15907 RVA: 0x001474C8 File Offset: 0x001456C8
		public override void DrawLayer()
		{
			if (OverlayDrawHandler.ShouldDrawPowerGrid)
			{
				base.DrawLayer();
			}
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x001474D8 File Offset: 0x001456D8
		protected override void TakePrintFrom(Thing t)
		{
			if (t.Faction != null && t.Faction != Faction.OfPlayer)
			{
				return;
			}
			Building building = t as Building;
			if (building != null)
			{
				building.PrintForPowerGrid(this);
			}
		}
	}
}
