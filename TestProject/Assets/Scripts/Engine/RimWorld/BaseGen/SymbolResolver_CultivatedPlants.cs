using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010AB RID: 4267
	public class SymbolResolver_CultivatedPlants : SymbolResolver
	{
		// Token: 0x060064F9 RID: 25849 RVA: 0x0023306C File Offset: 0x0023126C
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (rp.cultivatedPlantDef != null || SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect) != null);
		}

		// Token: 0x060064FA RID: 25850 RVA: 0x00233094 File Offset: 0x00231294
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef = rp.cultivatedPlantDef ?? SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect);
			if (thingDef == null)
			{
				return;
			}
			float growth = Rand.Range(0.2f, 1f);
			int age = thingDef.plant.LimitedLifespan ? Rand.Range(0, Mathf.Max(thingDef.plant.LifespanTicks - 2500, 0)) : 0;
			foreach (IntVec3 intVec in rp.rect)
			{
				if (map.fertilityGrid.FertilityAt(intVec) >= thingDef.plant.fertilityMin && this.TryDestroyBlockingThingsAt(intVec))
				{
					Plant plant = (Plant)GenSpawn.Spawn(thingDef, intVec, map, WipeMode.Vanish);
					plant.Growth = growth;
					if (plant.def.plant.LimitedLifespan)
					{
						plant.Age = age;
					}
				}
			}
		}

		// Token: 0x060064FB RID: 25851 RVA: 0x002331A0 File Offset: 0x002313A0
		public static ThingDef DeterminePlantDef(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			if (map.mapTemperature.OutdoorTemp < 0f || map.mapTemperature.OutdoorTemp > 58f)
			{
				return null;
			}
			float minFertility = float.MaxValue;
			bool flag = false;
			foreach (IntVec3 loc in rect)
			{
				float num = map.fertilityGrid.FertilityAt(loc);
				if (num > 0f)
				{
					flag = true;
					minFertility = Mathf.Min(minFertility, num);
				}
			}
			if (!flag)
			{
				return null;
			}
			ThingDef result;
			if ((from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Plant && x.plant.Sowable && !x.plant.IsTree && !x.plant.cavePlant && x.plant.fertilityMin <= minFertility && x.plant.Harvestable
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060064FC RID: 25852 RVA: 0x00233284 File Offset: 0x00231484
		private bool TryDestroyBlockingThingsAt(IntVec3 c)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_CultivatedPlants.tmpThings.Clear();
			SymbolResolver_CultivatedPlants.tmpThings.AddRange(c.GetThingList(map));
			for (int i = 0; i < SymbolResolver_CultivatedPlants.tmpThings.Count; i++)
			{
				if (!(SymbolResolver_CultivatedPlants.tmpThings[i] is Pawn) && !SymbolResolver_CultivatedPlants.tmpThings[i].def.destroyable)
				{
					SymbolResolver_CultivatedPlants.tmpThings.Clear();
					return false;
				}
			}
			for (int j = 0; j < SymbolResolver_CultivatedPlants.tmpThings.Count; j++)
			{
				if (!(SymbolResolver_CultivatedPlants.tmpThings[j] is Pawn))
				{
					SymbolResolver_CultivatedPlants.tmpThings[j].Destroy(DestroyMode.Vanish);
				}
			}
			SymbolResolver_CultivatedPlants.tmpThings.Clear();
			return true;
		}

		// Token: 0x04003DA7 RID: 15783
		private const float MinPlantGrowth = 0.2f;

		// Token: 0x04003DA8 RID: 15784
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
