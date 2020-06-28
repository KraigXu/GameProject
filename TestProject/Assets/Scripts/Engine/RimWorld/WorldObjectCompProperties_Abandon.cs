using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020008A1 RID: 2209
	public class WorldObjectCompProperties_Abandon : WorldObjectCompProperties
	{
		// Token: 0x0600358F RID: 13711 RVA: 0x00123CBA File Offset: 0x00121EBA
		public WorldObjectCompProperties_Abandon()
		{
			this.compClass = typeof(AbandonComp);
		}

		// Token: 0x06003590 RID: 13712 RVA: 0x00123CD2 File Offset: 0x00121ED2
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			foreach (string text in this.<>n__0(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(MapParent).IsAssignableFrom(parentDef.worldObjectClass))
			{
				yield return parentDef.defName + " has WorldObjectCompProperties_Abandon but it's not MapParent.";
			}
			yield break;
			yield break;
		}
	}
}
