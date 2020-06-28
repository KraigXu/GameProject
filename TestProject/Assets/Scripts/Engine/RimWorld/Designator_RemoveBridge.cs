using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E30 RID: 3632
	public class Designator_RemoveBridge : Designator_RemoveFloor
	{
		// Token: 0x060057C9 RID: 22473 RVA: 0x001D2230 File Offset: 0x001D0430
		public Designator_RemoveBridge()
		{
			this.defaultLabel = "DesignatorRemoveBridge".Translate();
			this.defaultDesc = "DesignatorRemoveBridgeDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemoveBridge", true);
			this.hotKey = KeyBindingDefOf.Misc5;
		}

		// Token: 0x060057CA RID: 22474 RVA: 0x001D2289 File Offset: 0x001D0489
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (c.InBounds(base.Map) && c.GetTerrain(base.Map) != TerrainDefOf.Bridge)
			{
				return false;
			}
			return base.CanDesignateCell(c);
		}
	}
}
