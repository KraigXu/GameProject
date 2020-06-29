using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetPlantPlayerCanHarvest : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> storeHarvestItemDefAs;

		
		[NoTranslate]
		public SlateRef<string> storeHarvestItemCountAs;

		
		[NoTranslate]
		public SlateRef<string> storeGrowDaysAs;

		
		public SlateRef<int> maxPlantGrowDays;

		
		public SlateRef<SimpleCurve> pointsToRequiredWorkCurve;

		
		public SlateRef<FloatRange?> workRandomFactorRange;

		
		private const float TemperatureBuffer = 5f;

		
		private const int TemperatureCheckDays = 15;
	}
}
