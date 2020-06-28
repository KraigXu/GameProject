using System;

namespace RimWorld.Planet
{
	// Token: 0x020011F5 RID: 4597
	public class WorldLayer_WorldObjects_Expandable : WorldLayer_WorldObjects
	{
		// Token: 0x170011D2 RID: 4562
		// (get) Token: 0x06006A52 RID: 27218 RVA: 0x0025109A File Offset: 0x0024F29A
		protected override float Alpha
		{
			get
			{
				return 1f - ExpandableWorldObjectsUtility.TransitionPct;
			}
		}

		// Token: 0x06006A53 RID: 27219 RVA: 0x002510A7 File Offset: 0x0024F2A7
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return !worldObject.def.expandingIcon;
		}
	}
}
