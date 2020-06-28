using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020008A0 RID: 2208
	public class WorldObjectCompProperties
	{
		// Token: 0x0600358C RID: 13708 RVA: 0x00123C8B File Offset: 0x00121E8B
		public virtual IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has WorldObjectCompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x0600358D RID: 13709 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ResolveReferences(WorldObjectDef parentDef)
		{
		}

		// Token: 0x04001D51 RID: 7505
		[TranslationHandle]
		public Type compClass = typeof(WorldObjectComp);
	}
}
