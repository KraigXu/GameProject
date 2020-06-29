﻿using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class RoadInfo : MapComponent
	{
		
		public RoadInfo(Map map) : base(map)
		{
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.roadEdgeTiles, "roadEdgeTiles", LookMode.Undefined, Array.Empty<object>());
		}

		
		public List<IntVec3> roadEdgeTiles = new List<IntVec3>();
	}
}
