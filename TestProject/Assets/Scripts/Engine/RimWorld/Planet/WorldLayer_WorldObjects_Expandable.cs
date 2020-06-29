using System;

namespace RimWorld.Planet
{
	
	public class WorldLayer_WorldObjects_Expandable : WorldLayer_WorldObjects
	{
		
		// (get) Token: 0x06006A52 RID: 27218 RVA: 0x0025109A File Offset: 0x0024F29A
		protected override float Alpha
		{
			get
			{
				return 1f - ExpandableWorldObjectsUtility.TransitionPct;
			}
		}

		
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return !worldObject.def.expandingIcon;
		}
	}
}
