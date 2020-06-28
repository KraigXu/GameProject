using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001022 RID: 4130
	public class StatWorker_SurgerySuccessChanceFactor : StatWorker
	{
		// Token: 0x060062E3 RID: 25315 RVA: 0x0022590C File Offset: 0x00223B0C
		public override bool ShouldShowFor(StatRequest req)
		{
			if (!base.ShouldShowFor(req))
			{
				return false;
			}
			Def def = req.Def;
			if (!(def is ThingDef))
			{
				return false;
			}
			ThingDef thingDef = def as ThingDef;
			return typeof(Building_Bed).IsAssignableFrom(thingDef.thingClass);
		}
	}
}
