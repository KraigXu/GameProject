using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001008 RID: 4104
	public class StatPart_PlantGrowthNutritionFactor : StatPart
	{
		// Token: 0x06006241 RID: 25153 RVA: 0x00221050 File Offset: 0x0021F250
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06006242 RID: 25154 RVA: 0x00221070 File Offset: 0x0021F270
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetFactor(req, out f))
			{
				Plant plant = (Plant)req.Thing;
				TaggedString taggedString = "StatsReport_PlantGrowth".Translate(plant.Growth.ToStringPercent()) + ": x" + f.ToStringPercent();
				if (!plant.def.plant.Sowable)
				{
					taggedString += " (" + "StatsReport_PlantGrowth_Wild".Translate() + ")";
				}
				return taggedString;
			}
			return null;
		}

		// Token: 0x06006243 RID: 25155 RVA: 0x00221104 File Offset: 0x0021F304
		private bool TryGetFactor(StatRequest req, out float factor)
		{
			if (!req.HasThing)
			{
				factor = 1f;
				return false;
			}
			Plant plant = req.Thing as Plant;
			if (plant == null)
			{
				factor = 1f;
				return false;
			}
			if (plant.def.plant.Sowable)
			{
				factor = plant.Growth;
				return true;
			}
			factor = Mathf.Lerp(0.5f, 1f, plant.Growth);
			return true;
		}
	}
}
