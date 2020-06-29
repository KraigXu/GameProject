using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	
	public class Tile
	{
		
		// (get) Token: 0x06006A81 RID: 27265 RVA: 0x002523D7 File Offset: 0x002505D7
		public bool WaterCovered
		{
			get
			{
				return this.elevation <= 0f;
			}
		}

		
		// (get) Token: 0x06006A82 RID: 27266 RVA: 0x002523E9 File Offset: 0x002505E9
		public List<Tile.RoadLink> Roads
		{
			get
			{
				if (!this.biome.allowRoads)
				{
					return null;
				}
				return this.potentialRoads;
			}
		}

		
		// (get) Token: 0x06006A83 RID: 27267 RVA: 0x00252400 File Offset: 0x00250600
		public List<Tile.RiverLink> Rivers
		{
			get
			{
				if (!this.biome.allowRivers)
				{
					return null;
				}
				return this.potentialRivers;
			}
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.biome,
				" elev=",
				this.elevation,
				"m hill=",
				this.hilliness,
				" temp=",
				this.temperature,
				"°C rain=",
				this.rainfall,
				"mm swampiness=",
				this.swampiness.ToStringPercent(),
				" potentialRoads=",
				(this.potentialRoads == null) ? 0 : this.potentialRoads.Count,
				" (allowed=",
				this.biome.allowRoads.ToString(),
				") potentialRivers=",
				(this.potentialRivers == null) ? 0 : this.potentialRivers.Count,
				" (allowed=",
				this.biome.allowRivers.ToString(),
				"))"
			});
		}

		
		public const int Invalid = -1;

		
		public BiomeDef biome;

		
		public float elevation = 100f;

		
		public Hilliness hilliness;

		
		public float temperature = 20f;

		
		public float rainfall;

		
		public float swampiness;

		
		public WorldFeature feature;

		
		public List<Tile.RoadLink> potentialRoads;

		
		public List<Tile.RiverLink> potentialRivers;

		
		public struct RoadLink
		{
			
			public int neighbor;

			
			public RoadDef road;
		}

		
		public struct RiverLink
		{
			
			public int neighbor;

			
			public RiverDef river;
		}
	}
}
