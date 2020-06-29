using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ListerArtificialBuildingsForMeditation
	{
		
		public ListerArtificialBuildingsForMeditation(Map map)
		{
			this.map = map;
		}

		
		public void Notify_BuildingSpawned(Building b)
		{
			if (MeditationUtility.CountsAsArtificialBuilding(b))
			{
				this.artificialBuildings.Add(b);
				this.artificialBuildingsPerCell.Clear();
			}
		}

		
		public void Notify_BuildingDeSpawned(Building b)
		{
			if (MeditationUtility.CountsAsArtificialBuilding(b))
			{
				this.artificialBuildings.Remove(b);
				this.artificialBuildingsPerCell.Clear();
			}
		}

		
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

		
		private Map map;

		
		private List<Thing> artificialBuildings = new List<Thing>();

		
		private Dictionary<CellWithRadius, List<Thing>> artificialBuildingsPerCell = new Dictionary<CellWithRadius, List<Thing>>();
	}
}
