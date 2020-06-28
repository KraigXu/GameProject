using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002EB RID: 747
	public class Graphic_Linked : Graphic
	{
		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001520 RID: 5408 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.Basic;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001521 RID: 5409 RVA: 0x0007C3A0 File Offset: 0x0007A5A0
		public override Material MatSingle
		{
			get
			{
				return MaterialAtlasPool.SubMaterialFromAtlas(this.subGraphic.MatSingle, LinkDirections.None);
			}
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x0007BCCC File Offset: 0x00079ECC
		public Graphic_Linked()
		{
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x0007C3B3 File Offset: 0x0007A5B3
		public Graphic_Linked(Graphic subGraphic)
		{
			this.subGraphic = subGraphic;
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x0007C3C2 File Offset: 0x0007A5C2
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_Linked(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
			{
				data = this.data
			};
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x0007C3E4 File Offset: 0x0007A5E4
		public override void Print(SectionLayer layer, Thing thing)
		{
			Material mat = this.LinkedDrawMatFrom(thing, thing.Position);
			Printer_Plane.PrintPlane(layer, thing.TrueCenter(), new Vector2(1f, 1f), mat, 0f, false, null, null, 0.01f, 0f);
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x0007C42D File Offset: 0x0007A62D
		public override Material MatSingleFor(Thing thing)
		{
			return this.LinkedDrawMatFrom(thing, thing.Position);
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x0007C43C File Offset: 0x0007A63C
		protected Material LinkedDrawMatFrom(Thing parent, IntVec3 cell)
		{
			int num = 0;
			int num2 = 1;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = cell + GenAdj.CardinalDirections[i];
				if (this.ShouldLinkWith(c, parent))
				{
					num += num2;
				}
				num2 *= 2;
			}
			LinkDirections linkSet = (LinkDirections)num;
			return MaterialAtlasPool.SubMaterialFromAtlas(this.subGraphic.MatSingleFor(parent), linkSet);
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x0007C494 File Offset: 0x0007A694
		public virtual bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			if (!parent.Spawned)
			{
				return false;
			}
			if (!c.InBounds(parent.Map))
			{
				return (parent.def.graphicData.linkFlags & LinkFlags.MapEdge) > LinkFlags.None;
			}
			return (parent.Map.linkGrid.LinkFlagsAt(c) & parent.def.graphicData.linkFlags) > LinkFlags.None;
		}

		// Token: 0x04000DF7 RID: 3575
		protected Graphic subGraphic;
	}
}
