using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011FF RID: 4607
	public class Tile
	{
		// Token: 0x170011DA RID: 4570
		// (get) Token: 0x06006A81 RID: 27265 RVA: 0x002523D7 File Offset: 0x002505D7
		public bool WaterCovered
		{
			get
			{
				return this.elevation <= 0f;
			}
		}

		// Token: 0x170011DB RID: 4571
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

		// Token: 0x170011DC RID: 4572
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

		// Token: 0x06006A84 RID: 27268 RVA: 0x00252418 File Offset: 0x00250618
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

		// Token: 0x04004277 RID: 17015
		public const int Invalid = -1;

		// Token: 0x04004278 RID: 17016
		public BiomeDef biome;

		// Token: 0x04004279 RID: 17017
		public float elevation = 100f;

		// Token: 0x0400427A RID: 17018
		public Hilliness hilliness;

		// Token: 0x0400427B RID: 17019
		public float temperature = 20f;

		// Token: 0x0400427C RID: 17020
		public float rainfall;

		// Token: 0x0400427D RID: 17021
		public float swampiness;

		// Token: 0x0400427E RID: 17022
		public WorldFeature feature;

		// Token: 0x0400427F RID: 17023
		public List<Tile.RoadLink> potentialRoads;

		// Token: 0x04004280 RID: 17024
		public List<Tile.RiverLink> potentialRivers;

		// Token: 0x02001F89 RID: 8073
		public struct RoadLink
		{
			// Token: 0x04007634 RID: 30260
			public int neighbor;

			// Token: 0x04007635 RID: 30261
			public RoadDef road;
		}

		// Token: 0x02001F8A RID: 8074
		public struct RiverLink
		{
			// Token: 0x04007636 RID: 30262
			public int neighbor;

			// Token: 0x04007637 RID: 30263
			public RiverDef river;
		}
	}
}
