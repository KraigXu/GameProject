using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D37 RID: 3383
	public class CompPlantHarmRadius : ThingComp
	{
		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x0600522A RID: 21034 RVA: 0x001B748E File Offset: 0x001B568E
		protected CompProperties_PlantHarmRadius PropsPlantHarmRadius
		{
			get
			{
				return (CompProperties_PlantHarmRadius)this.props;
			}
		}

		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x0600522B RID: 21035 RVA: 0x001B749B File Offset: 0x001B569B
		public float AgeDays
		{
			get
			{
				return (float)this.plantHarmAge / 60000f;
			}
		}

		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x0600522C RID: 21036 RVA: 0x001B74AA File Offset: 0x001B56AA
		public float CurrentRadius
		{
			get
			{
				return this.PropsPlantHarmRadius.radiusPerDayCurve.Evaluate(this.AgeDays);
			}
		}

		// Token: 0x0600522D RID: 21037 RVA: 0x001B74C2 File Offset: 0x001B56C2
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.plantHarmAge, "plantHarmAge", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToPlantHarm, "ticksToPlantHarm", 0, false);
		}

		// Token: 0x0600522E RID: 21038 RVA: 0x001B74E8 File Offset: 0x001B56E8
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostPostMake();
			this.initiatableComp = this.parent.GetComp<CompInitiatable>();
		}

		// Token: 0x0600522F RID: 21039 RVA: 0x001B7504 File Offset: 0x001B5704
		public override string CompInspectStringExtra()
		{
			return "FoliageKillRadius".Translate() + ": " + this.CurrentRadius.ToString("0.0") + "\n" + "RadiusExpandRate".Translate() + ": " + Math.Round((double)(this.PropsPlantHarmRadius.radiusPerDayCurve.Evaluate(this.AgeDays + 1f) - this.PropsPlantHarmRadius.radiusPerDayCurve.Evaluate(this.AgeDays))) + "/" + "day".Translate();
		}

		// Token: 0x06005230 RID: 21040 RVA: 0x001B75C4 File Offset: 0x001B57C4
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

		// Token: 0x06005231 RID: 21041 RVA: 0x001B7684 File Offset: 0x001B5884
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

		// Token: 0x04002D56 RID: 11606
		private int plantHarmAge;

		// Token: 0x04002D57 RID: 11607
		private int ticksToPlantHarm;

		// Token: 0x04002D58 RID: 11608
		protected CompInitiatable initiatableComp;
	}
}
