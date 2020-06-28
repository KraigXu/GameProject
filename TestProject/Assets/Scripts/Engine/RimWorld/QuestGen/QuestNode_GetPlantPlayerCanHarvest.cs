using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001137 RID: 4407
	public class QuestNode_GetPlantPlayerCanHarvest : QuestNode
	{
		// Token: 0x060066FC RID: 26364 RVA: 0x002411CA File Offset: 0x0023F3CA
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		// Token: 0x060066FD RID: 26365 RVA: 0x002411D3 File Offset: 0x0023F3D3
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060066FE RID: 26366 RVA: 0x002411E4 File Offset: 0x0023F3E4
		private bool DoWork(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return false;
			}
			float x = slate.Get<float>("points", 0f, false);
			float seasonalTemp = Find.World.tileTemperatures.GetSeasonalTemp(map.Tile);
			int ticksAbs = GenTicks.TicksAbs;
			for (int i = 0; i < 15; i++)
			{
				int absTick = ticksAbs + 60000 * i;
				float num = seasonalTemp + Find.World.tileTemperatures.OffsetFromDailyRandomVariation(map.Tile, absTick);
				if (num <= 5f || num >= 53f)
				{
					return false;
				}
			}
			ThingDef thingDef;
			if (!(from def in DefDatabase<ThingDef>.AllDefs
			where def.category == ThingCategory.Plant && !def.plant.cavePlant && def.plant.Sowable && def.plant.harvestedThingDef != null && def.plant.growDays <= (float)this.maxPlantGrowDays.GetValue(slate) && Command_SetPlantToGrow.IsPlantAvailable(def, map)
			select def).TryRandomElement(out thingDef))
			{
				return false;
			}
			SimpleCurve value = this.pointsToRequiredWorkCurve.GetValue(slate);
			float randomInRange = (this.workRandomFactorRange.GetValue(slate) ?? FloatRange.One).RandomInRange;
			float num2 = value.Evaluate(x) * randomInRange;
			float num3 = (thingDef.plant.sowWork + thingDef.plant.harvestWork) / thingDef.plant.harvestYield;
			int num4 = GenMath.RoundRandom(num2 / num3);
			num4 = Mathf.Max(num4, 1);
			slate.Set<ThingDef>(this.storeHarvestItemDefAs.GetValue(slate), thingDef.plant.harvestedThingDef, false);
			slate.Set<int>(this.storeHarvestItemCountAs.GetValue(slate), num4, false);
			if (this.storeGrowDaysAs.GetValue(slate) != null)
			{
				slate.Set<float>(this.storeGrowDaysAs.GetValue(slate), thingDef.plant.growDays, false);
			}
			return true;
		}

		// Token: 0x04003F24 RID: 16164
		[NoTranslate]
		public SlateRef<string> storeHarvestItemDefAs;

		// Token: 0x04003F25 RID: 16165
		[NoTranslate]
		public SlateRef<string> storeHarvestItemCountAs;

		// Token: 0x04003F26 RID: 16166
		[NoTranslate]
		public SlateRef<string> storeGrowDaysAs;

		// Token: 0x04003F27 RID: 16167
		public SlateRef<int> maxPlantGrowDays;

		// Token: 0x04003F28 RID: 16168
		public SlateRef<SimpleCurve> pointsToRequiredWorkCurve;

		// Token: 0x04003F29 RID: 16169
		public SlateRef<FloatRange?> workRandomFactorRange;

		// Token: 0x04003F2A RID: 16170
		private const float TemperatureBuffer = 5f;

		// Token: 0x04003F2B RID: 16171
		private const int TemperatureCheckDays = 15;
	}
}
