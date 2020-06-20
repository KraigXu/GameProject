using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000924 RID: 2340
	public class GameRules : IExposable
	{
		// Token: 0x06003792 RID: 14226 RVA: 0x0012A9A0 File Offset: 0x00128BA0
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

		// Token: 0x06003793 RID: 14227 RVA: 0x0012A9F3 File Offset: 0x00128BF3
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

		// Token: 0x06003794 RID: 14228 RVA: 0x0012AA34 File Offset: 0x00128C34
		public bool DesignatorAllowed(Designator d)
		{
			Designator_Place designator_Place = d as Designator_Place;
			if (designator_Place != null)
			{
				return !this.disallowedBuildings.Contains(designator_Place.PlacingDef);
			}
			return !this.disallowedDesignatorTypes.Contains(d.GetType());
		}

		// Token: 0x06003795 RID: 14229 RVA: 0x0012AA74 File Offset: 0x00128C74
		public void ExposeData()
		{
			Scribe_Collections.Look<ThingDef>(ref this.disallowedBuildings, "disallowedBuildings", LookMode.Undefined);
			Scribe_Collections.Look<Type>(ref this.disallowedDesignatorTypes, "disallowedDesignatorTypes", LookMode.Undefined);
		}

		// Token: 0x040020EE RID: 8430
		private HashSet<Type> disallowedDesignatorTypes = new HashSet<Type>();

		// Token: 0x040020EF RID: 8431
		private HashSet<ThingDef> disallowedBuildings = new HashSet<ThingDef>();
	}
}
