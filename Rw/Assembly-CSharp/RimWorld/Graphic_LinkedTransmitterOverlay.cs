using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C53 RID: 3155
	public class Graphic_LinkedTransmitterOverlay : Graphic_Linked
	{
		// Token: 0x06004B54 RID: 19284 RVA: 0x001967EF File Offset: 0x001949EF
		public Graphic_LinkedTransmitterOverlay()
		{
		}

		// Token: 0x06004B55 RID: 19285 RVA: 0x0007C4F7 File Offset: 0x0007A6F7
		public Graphic_LinkedTransmitterOverlay(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x06004B56 RID: 19286 RVA: 0x001967F7 File Offset: 0x001949F7
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			return c.InBounds(parent.Map) && parent.Map.powerNetGrid.TransmittedPowerNetAt(c) != null;
		}

		// Token: 0x06004B57 RID: 19287 RVA: 0x00196820 File Offset: 0x00194A20
		public override void Print(SectionLayer layer, Thing parent)
		{
			foreach (IntVec3 cell in parent.OccupiedRect())
			{
				Vector3 center = cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MapDataOverlay);
				Printer_Plane.PrintPlane(layer, center, new Vector2(1f, 1f), base.LinkedDrawMatFrom(parent, cell), 0f, false, null, null, 0.01f, 0f);
			}
		}
	}
}
