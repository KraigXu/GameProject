using System;

namespace RimWorld.Planet
{
	// Token: 0x020011F7 RID: 4599
	public class WorldLayer_WorldObjects_NonExpandable : WorldLayer_WorldObjects
	{
		// Token: 0x06006A5E RID: 27230 RVA: 0x00251471 File Offset: 0x0024F671
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return worldObject.def.expandingIcon;
		}
	}
}
