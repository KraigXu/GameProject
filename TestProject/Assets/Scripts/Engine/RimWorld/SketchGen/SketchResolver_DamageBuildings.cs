using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x0200108C RID: 4236
	public class SketchResolver_DamageBuildings : SketchResolver
	{
		// Token: 0x0600647A RID: 25722 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		// Token: 0x0600647B RID: 25723 RVA: 0x0022E238 File Offset: 0x0022C438
		protected override void ResolveInt(ResolveParams parms)
		{
			CellRect occupiedRect = parms.sketch.OccupiedRect;
			Rot4 random = Rot4.Random;
			int num = 0;
			int num2 = parms.sketch.Buildables.Count<SketchBuildable>();
			foreach (SketchBuildable buildable in parms.sketch.Buildables.InRandomOrder(null).ToList<SketchBuildable>())
			{
				bool flag;
				this.Damage(buildable, occupiedRect, random, parms.sketch, out flag);
				if (flag)
				{
					num++;
					if ((float)num > (float)num2 * 0.65f)
					{
						break;
					}
				}
			}
		}

		// Token: 0x0600647C RID: 25724 RVA: 0x0022E2E4 File Offset: 0x0022C4E4
		private void Damage(SketchBuildable buildable, CellRect rect, Rot4 dir, Sketch sketch, out bool destroyed)
		{
			float num;
			if (dir.IsHorizontal)
			{
				num = (float)(buildable.pos.x - rect.minX) / (float)rect.Width;
			}
			else
			{
				num = (float)(buildable.pos.z - rect.minZ) / (float)rect.Height;
			}
			if (dir == Rot4.East || dir == Rot4.South)
			{
				num = 1f - num;
			}
			if (Rand.Chance(Mathf.Pow(num, 1.32f)))
			{
				sketch.Remove(buildable);
				destroyed = true;
				SketchTerrain sketchTerrain = buildable as SketchTerrain;
				if (sketchTerrain != null && sketchTerrain.def.burnedDef != null)
				{
					sketch.AddTerrain(sketchTerrain.def.burnedDef, sketchTerrain.pos, true);
					return;
				}
			}
			else
			{
				destroyed = false;
				SketchThing sketchThing = buildable as SketchThing;
				if (sketchThing != null)
				{
					sketchThing.hitPoints = new int?(Mathf.Clamp(Mathf.RoundToInt((float)sketchThing.MaxHitPoints * (1f - num) * Rand.Range(1f, 1.2f)), 1, sketchThing.MaxHitPoints));
				}
			}
		}

		// Token: 0x04003D2E RID: 15662
		private const float MaxPctOfTotalDestroyed = 0.65f;

		// Token: 0x04003D2F RID: 15663
		private const float HpRandomFactor = 1.2f;

		// Token: 0x04003D30 RID: 15664
		private const float DestroyChanceExp = 1.32f;
	}
}
