using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class MechClusterSketch : IExposable
	{
		
		public MechClusterSketch()
		{
		}

		
		public MechClusterSketch(Sketch buildingsSketch, List<MechClusterSketch.Mech> pawns, bool startDormant)
		{
			this.buildingsSketch = buildingsSketch;
			this.pawns = pawns;
			this.startDormant = startDormant;
		}

		
		public void ExposeData()
		{
			Scribe_Deep.Look<Sketch>(ref this.buildingsSketch, "buildingsSketch", Array.Empty<object>());
			Scribe_Collections.Look<MechClusterSketch.Mech>(ref this.pawns, "pawns", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.startDormant, "startDormant", false, false);
		}

		
		public Sketch buildingsSketch;

		
		public List<MechClusterSketch.Mech> pawns;

		
		public bool startDormant;

		
		public struct Mech : IExposable
		{
			
			public Mech(PawnKindDef kindDef)
			{
				this.kindDef = kindDef;
				this.position = IntVec3.Invalid;
			}

			
			public void ExposeData()
			{
				Scribe_Defs.Look<PawnKindDef>(ref this.kindDef, "kindDef");
				Scribe_Values.Look<IntVec3>(ref this.position, "position", default(IntVec3), false);
			}

			
			public PawnKindDef kindDef;

			
			public IntVec3 position;
		}
	}
}
