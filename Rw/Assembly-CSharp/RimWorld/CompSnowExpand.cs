using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02000D56 RID: 3414
	public class CompSnowExpand : ThingComp
	{
		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x0600531F RID: 21279 RVA: 0x001BCF0D File Offset: 0x001BB10D
		public CompProperties_SnowExpand Props
		{
			get
			{
				return (CompProperties_SnowExpand)this.props;
			}
		}

		// Token: 0x06005320 RID: 21280 RVA: 0x001BCF1A File Offset: 0x001BB11A
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.snowRadius, "snowRadius", 0f, false);
		}

		// Token: 0x06005321 RID: 21281 RVA: 0x001BCF32 File Offset: 0x001BB132
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.parent.IsHashIntervalTick(this.Props.expandInterval))
			{
				this.TryExpandSnow();
			}
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x001BCF60 File Offset: 0x001BB160
		private void TryExpandSnow()
		{
			if (this.parent.Map.mapTemperature.OutdoorTemp > 10f)
			{
				this.snowRadius = 0f;
				return;
			}
			if (this.snowNoise == null)
			{
				this.snowNoise = new Perlin(0.054999999701976776, 2.0, 0.5, 5, Rand.Range(0, 651431), QualityMode.Medium);
			}
			if (this.snowRadius < 8f)
			{
				this.snowRadius += 1.3f;
			}
			else if (this.snowRadius < 17f)
			{
				this.snowRadius += 0.7f;
			}
			else if (this.snowRadius < 30f)
			{
				this.snowRadius += 0.4f;
			}
			else
			{
				this.snowRadius += 0.1f;
			}
			this.snowRadius = Mathf.Min(this.snowRadius, this.Props.maxRadius);
			CellRect occupiedRect = this.parent.OccupiedRect();
			CompSnowExpand.reachableCells.Clear();
			this.parent.Map.floodFiller.FloodFill(this.parent.Position, (IntVec3 x) => (float)x.DistanceToSquared(this.parent.Position) <= this.snowRadius * this.snowRadius && (occupiedRect.Contains(x) || !x.Filled(this.parent.Map)), delegate(IntVec3 x)
			{
				CompSnowExpand.reachableCells.Add(x);
			}, int.MaxValue, false, null);
			int num = GenRadial.NumCellsInRadius(this.snowRadius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = this.parent.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.parent.Map) && CompSnowExpand.reachableCells.Contains(intVec))
				{
					float num2 = this.snowNoise.GetValue(intVec);
					num2 += 1f;
					num2 *= 0.5f;
					if (num2 < 0.1f)
					{
						num2 = 0.1f;
					}
					if (this.parent.Map.snowGrid.GetDepth(intVec) <= num2)
					{
						float lengthHorizontal = (intVec - this.parent.Position).LengthHorizontal;
						float num3 = 1f - lengthHorizontal / this.snowRadius;
						this.parent.Map.snowGrid.AddDepth(intVec, num3 * this.Props.addAmount * num2);
					}
				}
			}
		}

		// Token: 0x04002DE2 RID: 11746
		private float snowRadius;

		// Token: 0x04002DE3 RID: 11747
		private ModuleBase snowNoise;

		// Token: 0x04002DE4 RID: 11748
		private const float MaxOutdoorTemp = 10f;

		// Token: 0x04002DE5 RID: 11749
		private static HashSet<IntVec3> reachableCells = new HashSet<IntVec3>();
	}
}
