using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FDF RID: 4063
	public class SpecialThingFilterWorker_CorpsesColonist : SpecialThingFilterWorker
	{
		// Token: 0x060061A2 RID: 24994 RVA: 0x0021FAC4 File Offset: 0x0021DCC4
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && corpse.InnerPawn.Faction == Faction.OfPlayer;
		}
	}
}
