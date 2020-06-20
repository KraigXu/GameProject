using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D9 RID: 2521
	public class IncidentWorker_CropBlight : IncidentWorker
	{
		// Token: 0x06003C2C RID: 15404 RVA: 0x0013DBC8 File Offset: 0x0013BDC8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Plant plant;
			return this.TryFindRandomBlightablePlant((Map)parms.target, out plant);
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x0013DBE8 File Offset: 0x0013BDE8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			float num = IncidentWorker_CropBlight.RadiusFactorPerPointsCurve.Evaluate(parms.points);
			Plant plant;
			if (!this.TryFindRandomBlightablePlant(map, out plant))
			{
				return false;
			}
			Room room = plant.GetRoom(RegionType.Set_Passable);
			int i = 0;
			int num2 = GenRadial.NumCellsInRadius(11f * num);
			while (i < num2)
			{
				IntVec3 intVec = plant.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && intVec.GetRoom(map, RegionType.Set_Passable) == room)
				{
					Plant firstBlightableNowPlant = BlightUtility.GetFirstBlightableNowPlant(intVec, map);
					if (firstBlightableNowPlant != null && firstBlightableNowPlant.def == plant.def && Rand.Chance(this.BlightChance(firstBlightableNowPlant.Position, plant.Position, num)))
					{
						firstBlightableNowPlant.CropBlighted();
					}
				}
				i++;
			}
			base.SendStandardLetter("LetterLabelCropBlight".Translate(new NamedArgument(plant.def, "PLANTDEF")), "LetterCropBlight".Translate(new NamedArgument(plant.def, "PLANTDEF")), LetterDefOf.NegativeEvent, parms, new TargetInfo(plant.Position, map, false), Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x06003C2E RID: 15406 RVA: 0x0013DD0C File Offset: 0x0013BF0C
		private bool TryFindRandomBlightablePlant(Map map, out Plant plant)
		{
			Thing thing;
			bool result = (from x in map.listerThings.ThingsInGroup(ThingRequestGroup.Plant)
			where ((Plant)x).BlightableNow
			select x).TryRandomElement(out thing);
			plant = (Plant)thing;
			return result;
		}

		// Token: 0x06003C2F RID: 15407 RVA: 0x0013DD5C File Offset: 0x0013BF5C
		private float BlightChance(IntVec3 c, IntVec3 root, float radiusFactor)
		{
			float x = c.DistanceTo(root) / radiusFactor;
			return IncidentWorker_CropBlight.BlightChancePerRadius.Evaluate(x);
		}

		// Token: 0x0400237B RID: 9083
		private const float Radius = 11f;

		// Token: 0x0400237C RID: 9084
		private static readonly SimpleCurve BlightChancePerRadius = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(8f, 1f),
				true
			},
			{
				new CurvePoint(11f, 0.3f),
				true
			}
		};

		// Token: 0x0400237D RID: 9085
		private static readonly SimpleCurve RadiusFactorPerPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 0.6f),
				true
			},
			{
				new CurvePoint(500f, 1f),
				true
			},
			{
				new CurvePoint(2000f, 2f),
				true
			}
		};
	}
}
