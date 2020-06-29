using System;
using Verse;

namespace RimWorld
{
	
	public class CompPlantHarmRadius : ThingComp
	{
		
		// (get) Token: 0x0600522A RID: 21034 RVA: 0x001B748E File Offset: 0x001B568E
		protected CompProperties_PlantHarmRadius PropsPlantHarmRadius
		{
			get
			{
				return (CompProperties_PlantHarmRadius)this.props;
			}
		}

		
		// (get) Token: 0x0600522B RID: 21035 RVA: 0x001B749B File Offset: 0x001B569B
		public float AgeDays
		{
			get
			{
				return (float)this.plantHarmAge / 60000f;
			}
		}

		
		// (get) Token: 0x0600522C RID: 21036 RVA: 0x001B74AA File Offset: 0x001B56AA
		public float CurrentRadius
		{
			get
			{
				return this.PropsPlantHarmRadius.radiusPerDayCurve.Evaluate(this.AgeDays);
			}
		}

		
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.plantHarmAge, "plantHarmAge", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToPlantHarm, "ticksToPlantHarm", 0, false);
		}

		
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostPostMake();
			this.initiatableComp = this.parent.GetComp<CompInitiatable>();
		}

		
		public override string CompInspectStringExtra()
		{
			return "FoliageKillRadius".Translate() + ": " + this.CurrentRadius.ToString("0.0") + "\n" + "RadiusExpandRate".Translate() + ": " + Math.Round((double)(this.PropsPlantHarmRadius.radiusPerDayCurve.Evaluate(this.AgeDays + 1f) - this.PropsPlantHarmRadius.radiusPerDayCurve.Evaluate(this.AgeDays))) + "/" + "day".Translate();
		}

		
		public override void CompTick()
		{
			if (!this.parent.Spawned || (this.initiatableComp != null && !this.initiatableComp.Initiated))
			{
				return;
			}
			this.plantHarmAge++;
			this.ticksToPlantHarm--;
			if (this.ticksToPlantHarm <= 0)
			{
				float currentRadius = this.CurrentRadius;
				float num = 3.14159274f * currentRadius * currentRadius * this.PropsPlantHarmRadius.harmFrequencyPerArea;
				float num2 = 60f / num;
				int num3;
				if (num2 >= 1f)
				{
					this.ticksToPlantHarm = GenMath.RoundRandom(num2);
					num3 = 1;
				}
				else
				{
					this.ticksToPlantHarm = 1;
					num3 = GenMath.RoundRandom(1f / num2);
				}
				for (int i = 0; i < num3; i++)
				{
					this.HarmRandomPlantInRadius(currentRadius);
				}
			}
		}

		
		private void HarmRandomPlantInRadius(float radius)
		{
			IntVec3 c = this.parent.Position + (Rand.InsideUnitCircleVec3 * radius).ToIntVec3();
			if (!c.InBounds(this.parent.Map))
			{
				return;
			}
			Plant plant = c.GetPlant(this.parent.Map);
			if (plant != null)
			{
				if (plant.LeaflessNow)
				{
					if (Rand.Value < this.PropsPlantHarmRadius.leaflessPlantKillChance)
					{
						plant.Kill(null, null);
						return;
					}
				}
				else
				{
					plant.MakeLeafless(Plant.LeaflessCause.Poison);
				}
			}
		}

		
		private int plantHarmAge;

		
		private int ticksToPlantHarm;

		
		protected CompInitiatable initiatableComp;
	}
}
