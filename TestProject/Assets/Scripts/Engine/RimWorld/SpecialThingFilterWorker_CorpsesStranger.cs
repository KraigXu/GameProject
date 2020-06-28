using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE0 RID: 4064
	public class SpecialThingFilterWorker_CorpsesStranger : SpecialThingFilterWorker
	{
		// Token: 0x060061A4 RID: 24996 RVA: 0x0021FB10 File Offset: 0x0021DD10
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && corpse.InnerPawn.Faction != Faction.OfPlayer;
		}
	}
}
