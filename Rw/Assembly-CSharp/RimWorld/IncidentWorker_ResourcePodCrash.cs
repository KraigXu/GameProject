using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009EF RID: 2543
	public class IncidentWorker_ResourcePodCrash : IncidentWorker
	{
		// Token: 0x06003C83 RID: 15491 RVA: 0x0013FBCC File Offset: 0x0013DDCC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<Thing> things = ThingSetMakerDefOf.ResourcePod.root.Generate();
			IntVec3 intVec = DropCellFinder.RandomDropSpot(map);
			DropPodUtility.DropThingsNear(intVec, map, things, 110, false, true, true, true);
			base.SendStandardLetter("LetterLabelCargoPodCrash".Translate(), "CargoPodCrash".Translate(), LetterDefOf.PositiveEvent, parms, new TargetInfo(intVec, map, false), Array.Empty<NamedArgument>());
			return true;
		}
	}
}
