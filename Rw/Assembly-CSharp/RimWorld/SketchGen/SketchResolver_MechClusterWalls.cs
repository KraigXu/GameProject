using System;
using UnityEngine;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x0200108F RID: 4239
	public class SketchResolver_MechClusterWalls : SketchResolver
	{
		// Token: 0x06006488 RID: 25736 RVA: 0x0022EA98 File Offset: 0x0022CC98
		protected override void ResolveInt(ResolveParams parms)
		{
			IntVec2 value = parms.mechClusterSize.Value;
			int num = GenMath.RoundRandom((float)GenMath.RoundRandom(SketchResolver_MechClusterWalls.WidthToMaxWallsCountCurve.Evaluate((float)Mathf.Min(value.x, value.z))) * SketchResolver_MechClusterWalls.WallCountRandomFactorRange.RandomInRange);
			num = Math.Max(1, num);
			for (int i = 0; i < num; i++)
			{
				this.TryAddWall(parms.sketch, value);
			}
			if (Rand.Bool)
			{
				ResolveParams parms2 = parms;
				parms2.symmetryVertical = new bool?(false);
				parms2.symmetryOrigin = new int?(value.x / 2);
				parms2.symmetryOriginIncluded = new bool?(value.x % 2 == 1);
				SketchResolverDefOf.Symmetry.Resolve(parms2);
				return;
			}
			if (Rand.Bool)
			{
				ResolveParams parms3 = parms;
				parms3.symmetryVertical = new bool?(true);
				parms3.symmetryOrigin = new int?(value.z / 2);
				parms3.symmetryOriginIncluded = new bool?(value.z % 2 == 1);
				SketchResolverDefOf.Symmetry.Resolve(parms3);
			}
		}

		// Token: 0x06006489 RID: 25737 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		// Token: 0x0600648A RID: 25738 RVA: 0x0022EBA8 File Offset: 0x0022CDA8
		private void TryAddWall(Sketch sketch, IntVec2 size)
		{
			SketchResolver_MechClusterWalls.<>c__DisplayClass9_0 <>c__DisplayClass9_;
			<>c__DisplayClass9_.sketch = sketch;
			for (int i = 0; i < 50; i++)
			{
				if (Rand.Chance(0.8f))
				{
					bool @bool = Rand.Bool;
					int num = @bool ? size.x : size.z;
					CellRect rect;
					if (@bool)
					{
						IntVec2 intVec = new IntVec2(1, Rand.Bool ? (size.z - 1) : 0);
						rect = new CellRect(intVec.x, intVec.z, num - 1, 1);
					}
					else
					{
						IntVec2 intVec2 = new IntVec2(Rand.Bool ? (size.x - 1) : 0, 0);
						rect = new CellRect(intVec2.x, intVec2.z, 1, num);
					}
					rect.ClipInsideRect(new CellRect(0, 0, size.x, size.z));
					if (rect.Area >= 3 && SketchResolver_MechClusterWalls.<TryAddWall>g__WallRectIsUsable|9_0(rect, false, ref <>c__DisplayClass9_))
					{
						SketchResolver_MechClusterWalls.<TryAddWall>g__GenerateWallInRect|9_1(rect, Rand.Bool, ref <>c__DisplayClass9_);
						return;
					}
				}
				else
				{
					IntVec3 intVec3 = new IntVec3(Rand.RangeInclusive(0, size.x - 1), 0, Rand.RangeInclusive(0, size.z - 1));
					int num2 = GenMath.RoundRandom(Rand.Range((float)size.x * 0.4f, (float)size.x));
					CellRect rect2;
					if (Rand.Bool)
					{
						rect2 = new CellRect(intVec3.x, intVec3.z, num2, 1);
					}
					else
					{
						rect2 = new CellRect(intVec3.x - num2 + 1, intVec3.z, num2, 1);
					}
					rect2.ClipInsideRect(new CellRect(0, 0, size.x, size.z));
					if (rect2.Area >= 2)
					{
						int num3 = GenMath.RoundRandom(Rand.Range((float)size.z * 0.4f, (float)size.z));
						CellRect rect3;
						if (Rand.Bool)
						{
							rect3 = new CellRect(intVec3.x, intVec3.z, 1, num3);
						}
						else
						{
							rect3 = new CellRect(intVec3.x, intVec3.z - num3 + 1, 1, num3);
						}
						rect3.ClipInsideRect(new CellRect(0, 0, size.x, size.z));
						if (rect3.Area >= 2 && SketchResolver_MechClusterWalls.<TryAddWall>g__WallRectIsUsable|9_0(rect2, true, ref <>c__DisplayClass9_) && SketchResolver_MechClusterWalls.<TryAddWall>g__WallRectIsUsable|9_0(rect3, true, ref <>c__DisplayClass9_))
						{
							SketchResolver_MechClusterWalls.<TryAddWall>g__GenerateWallInRect|9_1(rect2, false, ref <>c__DisplayClass9_);
							SketchResolver_MechClusterWalls.<TryAddWall>g__GenerateWallInRect|9_1(rect3, false, ref <>c__DisplayClass9_);
							return;
						}
					}
				}
			}
		}

		// Token: 0x04003D35 RID: 15669
		private static readonly FloatRange WallCountRandomFactorRange = new FloatRange(0.5f, 1f);

		// Token: 0x04003D36 RID: 15670
		private static readonly SimpleCurve WidthToMaxWallsCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(3f, 1f),
				true
			},
			{
				new CurvePoint(6f, 2f),
				true
			},
			{
				new CurvePoint(9f, 3f),
				true
			},
			{
				new CurvePoint(14f, 4f),
				true
			}
		};

		// Token: 0x04003D37 RID: 15671
		private const float Straight_LengthMinSizeFraction = 0.8f;

		// Token: 0x04003D38 RID: 15672
		private const float Corner_LengthMinSizeFraction = 0.4f;

		// Token: 0x04003D39 RID: 15673
		private const float EdgeWallChance = 0.8f;

		// Token: 0x04003D3A RID: 15674
		private const int MinWallLengthStraight = 3;

		// Token: 0x04003D3B RID: 15675
		private const int MinWallLengthCorner = 2;
	}
}
