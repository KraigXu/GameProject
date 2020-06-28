using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020008A5 RID: 2213
	public class WorldObjectCompProperties_EscapeShip : WorldObjectCompProperties
	{
		// Token: 0x06003597 RID: 13719 RVA: 0x00123D51 File Offset: 0x00121F51
		public WorldObjectCompProperties_EscapeShip()
		{
			this.compClass = typeof(EscapeShipComp);
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x00123D69 File Offset: 0x00121F69
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			foreach (string text in this.<>n__0(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(MapParent).IsAssignableFrom(parentDef.worldObjectClass))
			{
				yield return parentDef.defName + " has WorldObjectCompProperties_EscapeShip but it's not MapParent.";
			}
			yield break;
			yield break;
		}
	}
}
