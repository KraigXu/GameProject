﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000850 RID: 2128
	public abstract class ThoughtWorker_MusicalInstrumentListeningBase : ThoughtWorker
	{
		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x060034BD RID: 13501
		protected abstract ThingDef InstrumentDef { get; }

		// Token: 0x060034BE RID: 13502 RVA: 0x00120CF8 File Offset: 0x0011EEF8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThingDef def = this.InstrumentDef;
			return GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(def), PathEndMode.ClosestTouch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false), def.building.instrumentRange, delegate(Thing thing)
			{
				Building_MusicalInstrument building_MusicalInstrument = thing as Building_MusicalInstrument;
				return building_MusicalInstrument != null && building_MusicalInstrument.IsBeingPlayed && Building_MusicalInstrument.IsAffectedByInstrument(def, building_MusicalInstrument.Position, p.Position, p.Map);
			}, null, 0, -1, false, RegionType.Set_Passable, false) != null;
		}
	}
}
