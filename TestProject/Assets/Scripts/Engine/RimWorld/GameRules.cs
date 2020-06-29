using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class GameRules : IExposable
	{
		
		public void SetAllowDesignator(Type type, bool allowed)
		{
			if (allowed && this.disallowedDesignatorTypes.Contains(type))
			{
				this.disallowedDesignatorTypes.Remove(type);
			}
			if (!allowed && !this.disallowedDesignatorTypes.Contains(type))
			{
				this.disallowedDesignatorTypes.Add(type);
			}
			Find.ReverseDesignatorDatabase.Reinit();
		}

		
		public void SetAllowBuilding(ThingDef building, bool allowed)
		{
			if (allowed && this.disallowedBuildings.Contains(building))
			{
				this.disallowedBuildings.Remove(building);
			}
			if (!allowed && !this.disallowedBuildings.Contains(building))
			{
				this.disallowedBuildings.Add(building);
			}
		}

		
		public bool DesignatorAllowed(Designator d)
		{
			Designator_Place designator_Place = d as Designator_Place;
			if (designator_Place != null)
			{
				return !this.disallowedBuildings.Contains(designator_Place.PlacingDef);
			}
			return !this.disallowedDesignatorTypes.Contains(d.GetType());
		}

		
		public void ExposeData()
		{
			Scribe_Collections.Look<ThingDef>(ref this.disallowedBuildings, "disallowedBuildings", LookMode.Undefined);
			Scribe_Collections.Look<Type>(ref this.disallowedDesignatorTypes, "disallowedDesignatorTypes", LookMode.Undefined);
		}

		
		private HashSet<Type> disallowedDesignatorTypes = new HashSet<Type>();

		
		private HashSet<ThingDef> disallowedBuildings = new HashSet<ThingDef>();
	}
}
