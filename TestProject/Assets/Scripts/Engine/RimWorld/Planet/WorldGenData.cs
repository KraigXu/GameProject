using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public class WorldGenData : WorldComponent
	{
		
		public WorldGenData(World world) : base(world)
		{
		}

		
		public override void ExposeData()
		{
			Scribe_Collections.Look<int>(ref this.roadNodes, "roadNodes", LookMode.Undefined, Array.Empty<object>());
		}

		
		public List<int> roadNodes = new List<int>();

		
		public List<int> ancientSites = new List<int>();
	}
}
