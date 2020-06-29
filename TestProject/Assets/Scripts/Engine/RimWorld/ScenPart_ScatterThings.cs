﻿using System;
using Verse;

namespace RimWorld
{
	
	public abstract class ScenPart_ScatterThings : ScenPart_ThingCount
	{
		
		// (get) Token: 0x06004A30 RID: 18992
		protected abstract bool NearPlayerStart { get; }

		
		public override void GenerateIntoMap(Map map)
		{
			if (Find.GameInitData == null)
			{
				return;
			}
			new GenStep_ScatterThings
			{
				nearPlayerStart = this.NearPlayerStart,
				allowFoggedPositions = !this.NearPlayerStart,
				thingDef = this.thingDef,
				stuff = this.stuff,
				count = this.count,
				spotMustBeStandable = true,
				minSpacing = 5f,
				clusterSize = ((this.thingDef.category == ThingCategory.Building) ? 1 : 4)
			}.Generate(map, default(GenStepParams));
		}
	}
}
