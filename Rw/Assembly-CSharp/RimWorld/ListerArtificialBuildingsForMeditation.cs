using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A3C RID: 2620
	public class ListerArtificialBuildingsForMeditation
	{
		// Token: 0x06003DE3 RID: 15843 RVA: 0x001465FE File Offset: 0x001447FE
		public ListerArtificialBuildingsForMeditation(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x00146623 File Offset: 0x00144823
		public void Notify_BuildingSpawned(Building b)
		{
			if (MeditationUtility.CountsAsArtificialBuilding(b))
			{
				this.artificialBuildings.Add(b);
				this.artificialBuildingsPerCell.Clear();
			}
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x00146644 File Offset: 0x00144844
		public void Notify_BuildingDeSpawned(Building b)
		{
			if (MeditationUtility.CountsAsArtificialBuilding(b))
			{
				this.artificialBuildings.Remove(b);
				this.artificialBuildingsPerCell.Clear();
			}
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x00146668 File Offset: 0x00144868
		public List<Thing> GetForCell(IntVec3 cell, float radius)
		{
			CellWithRadius key = new CellWithRadius(cell, radius);
			List<Thing> list;
			if (!this.artificialBuildingsPerCell.TryGetValue(key, out list))
			{
				list = new List<Thing>();
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(cell, this.map, radius, false))
				{
					if (MeditationUtility.CountsAsArtificialBuilding(thing))
					{
						list.Add(thing);
					}
				}
				this.artificialBuildingsPerCell[key] = list;
			}
			return list;
		}

		// Token: 0x0400241D RID: 9245
		private Map map;

		// Token: 0x0400241E RID: 9246
		private List<Thing> artificialBuildings = new List<Thing>();

		// Token: 0x0400241F RID: 9247
		private Dictionary<CellWithRadius, List<Thing>> artificialBuildingsPerCell = new Dictionary<CellWithRadius, List<Thing>>();
	}
}
