using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001055 RID: 4181
	public class PlaceWorker_Conduit : PlaceWorker
	{
		// Token: 0x060063C6 RID: 25542 RVA: 0x0022993C File Offset: 0x00227B3C
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			List<Thing> thingList = loc.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.EverTransmitsPower)
				{
					return false;
				}
				if (thingList[i].def.entityDefToBuild != null)
				{
					ThingDef thingDef = thingList[i].def.entityDefToBuild as ThingDef;
					if (thingDef != null && thingDef.EverTransmitsPower)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
