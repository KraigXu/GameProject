using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020008A3 RID: 2211
	public class WorldObjectCompProperties_DefeatAllEnemiesQuest : WorldObjectCompProperties
	{
		// Token: 0x06003593 RID: 13715 RVA: 0x00123D0A File Offset: 0x00121F0A
		public WorldObjectCompProperties_DefeatAllEnemiesQuest()
		{
			this.compClass = typeof(DefeatAllEnemiesQuestComp);
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x00123D22 File Offset: 0x00121F22
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			foreach (string text in this.<>n__0(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(MapParent).IsAssignableFrom(parentDef.worldObjectClass))
			{
				yield return parentDef.defName + " has WorldObjectCompProperties_DefeatAllEnemiesQuest but it's not MapParent.";
			}
			yield break;
			yield break;
		}
	}
}
