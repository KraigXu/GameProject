using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000762 RID: 1890
	public class WorkGiver_TendSelf : WorkGiver_Tend
	{
		// Token: 0x06003169 RID: 12649 RVA: 0x001137DE File Offset: 0x001119DE
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			yield return pawn;
			yield break;
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x0600316A RID: 12650 RVA: 0x0010E01A File Offset: 0x0010C21A
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Undefined);
			}
		}

		// Token: 0x0600316B RID: 12651 RVA: 0x001137F0 File Offset: 0x001119F0
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool flag = pawn == t && pawn.playerSettings != null && base.HasJobOnThing(pawn, t, forced);
			if (flag && !pawn.playerSettings.selfTend)
			{
				JobFailReason.Is("SelfTendDisabled".Translate(), null);
			}
			return flag && pawn.playerSettings.selfTend;
		}
	}
}
