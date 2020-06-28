using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200105A RID: 4186
	public class PlaceWorker_ShowTurretRadius : PlaceWorker
	{
		// Token: 0x060063D0 RID: 25552 RVA: 0x00229AA0 File Offset: 0x00227CA0
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			VerbProperties verbProperties = ((ThingDef)checkingDef).building.turretGunDef.Verbs.Find((VerbProperties v) => v.verbClass == typeof(Verb_Shoot));
			if (verbProperties.range > 0f)
			{
				GenDraw.DrawRadiusRing(loc, verbProperties.range);
			}
			if (verbProperties.minRange > 0f)
			{
				GenDraw.DrawRadiusRing(loc, verbProperties.minRange);
			}
			return true;
		}
	}
}
