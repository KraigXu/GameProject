using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.SketchGen
{
	public class SketchResolver_Monument : SketchResolver
	{
		// Token: 0x0600648F RID: 25743 RVA: 0x0022EFA8 File Offset: 0x0022D1A8
		protected override void ResolveInt(ResolveParams parms)
		{
			//SketchResolver_Monument.<>c__DisplayClass1_0 <>c__DisplayClass1_ = new SketchResolver_Monument.<>c__DisplayClass1_0();
			//<>c__DisplayClass1_.parms = parms;
			//IntVec2 value;
			//if (<>c__DisplayClass1_.parms.monumentSize != null)
			//{
			//	value = <>c__DisplayClass1_.parms.monumentSize.Value;
			//}
			//else
			//{
			//	int num = Rand.Range(10, 50);
			//	value = new IntVec2(num, num);
			//}
			//<>c__DisplayClass1_.width = value.x;
			//<>c__DisplayClass1_.height = value.z;
			//bool flag;
			//if (<>c__DisplayClass1_.parms.monumentOpen != null)
			//{
			//	flag = <>c__DisplayClass1_.parms.monumentOpen.Value;
			//}
			//else
			//{
			//	flag = Rand.Chance(SketchResolver_Monument.OpenChancePerSizeCurve.Evaluate((float)Mathf.Max(<>c__DisplayClass1_.width, <>c__DisplayClass1_.height)));
			//}
			//<>c__DisplayClass1_.monument = new Sketch();
			//<>c__DisplayClass1_.onlyBuildableByPlayer = (<>c__DisplayClass1_.parms.onlyBuildableByPlayer ?? false);
			//<>c__DisplayClass1_.filterAllowsAll = (<>c__DisplayClass1_.parms.allowedMonumentThings == null);
			//List<IntVec3> list = new List<IntVec3>();
			//if (flag)
			//{
			//	<>c__DisplayClass1_.horizontalSymmetry = true;
			//	<>c__DisplayClass1_.verticalSymmetry = true;
			//	bool[,] array = AbstractShapeGenerator.Generate(<>c__DisplayClass1_.width, <>c__DisplayClass1_.height, <>c__DisplayClass1_.horizontalSymmetry, <>c__DisplayClass1_.verticalSymmetry, false, false, true, 0f);
			//	for (int i = 0; i < array.GetLength(0); i++)
			//	{
			//		for (int j = 0; j < array.GetLength(1); j++)
			//		{
			//			if (array[i, j])
			//			{
			//				<>c__DisplayClass1_.monument.AddThing(ThingDefOf.Wall, new IntVec3(i, 0, j), Rot4.North, ThingDefOf.WoodLog, 1, null, null, true);
			//			}
			//		}
			//	}
			//}
			//else
			//{
			//	<>c__DisplayClass1_.horizontalSymmetry = Rand.Bool;
			//	<>c__DisplayClass1_.verticalSymmetry = (!<>c__DisplayClass1_.horizontalSymmetry || Rand.Bool);
			//	bool[,] shape = AbstractShapeGenerator.Generate(<>c__DisplayClass1_.width - 2, <>c__DisplayClass1_.height - 2, <>c__DisplayClass1_.horizontalSymmetry, <>c__DisplayClass1_.verticalSymmetry, true, true, false, 0f);
			//	Func<int, int, bool> func = (int x, int z) => x >= 0 && z >= 0 && x < shape.GetLength(0) && z < shape.GetLength(1) && shape[x, z];
			//	for (int k = -1; k < shape.GetLength(0) + 1; k++)
			//	{
			//		for (int l = -1; l < shape.GetLength(1) + 1; l++)
			//		{
			//			if (!func(k, l) && (func(k - 1, l) || func(k, l - 1) || func(k, l + 1) || func(k + 1, l) || func(k - 1, l - 1) || func(k - 1, l + 1) || func(k + 1, l - 1) || func(k + 1, l + 1)))
			//			{
			//				int newX = k + 1;
			//				int newZ = l + 1;
			//				<>c__DisplayClass1_.monument.AddThing(ThingDefOf.Wall, new IntVec3(newX, 0, newZ), Rot4.North, ThingDefOf.WoodLog, 1, null, null, true);
			//			}
			//		}
			//	}
			//	for (int m = -1; m < shape.GetLength(0) + 1; m++)
			//	{
			//		for (int n = -1; n < shape.GetLength(1) + 1; n++)
			//		{
			//			if (!func(m, n) && (func(m - 1, n) || func(m, n - 1) || func(m, n + 1) || func(m + 1, n)))
			//			{
			//				int num2 = m + 1;
			//				int num3 = n + 1;
			//				if ((!func(m - 1, n) && <>c__DisplayClass1_.monument.Passable(new IntVec3(num2 - 1, 0, num3))) || (!func(m, n - 1) && <>c__DisplayClass1_.monument.Passable(new IntVec3(num2, 0, num3 - 1))) || (!func(m, n + 1) && <>c__DisplayClass1_.monument.Passable(new IntVec3(num2, 0, num3 + 1))) || (!func(m + 1, n) && <>c__DisplayClass1_.monument.Passable(new IntVec3(num2 + 1, 0, num3))))
			//				{
			//					list.Add(new IntVec3(num2, 0, num3));
			//				}
			//			}
			//		}
			//	}
			//}
			//ResolveParams parms2 = <>c__DisplayClass1_.parms;
			//parms2.sketch = <>c__DisplayClass1_.monument;
			//parms2.connectedGroupsSameStuff = new bool?(true);
			//parms2.assignRandomStuffTo = ThingDefOf.Wall;
			//SketchResolverDefOf.AssignRandomStuff.Resolve(parms2);
			//if (<>c__DisplayClass1_.parms.addFloors ?? true)
			//{
			//	ResolveParams parms3 = <>c__DisplayClass1_.parms;
			//	parms3.singleFloorType = new bool?(true);
			//	parms3.sketch = <>c__DisplayClass1_.monument;
			//	parms3.floorFillRoomsOnly = new bool?(!flag);
			//	parms3.onlyStoneFloors = new bool?(<>c__DisplayClass1_.parms.onlyStoneFloors ?? true);
			//	parms3.allowConcrete = new bool?(<>c__DisplayClass1_.parms.allowConcrete ?? false);
			//	parms3.rect = new CellRect?(new CellRect(0, 0, <>c__DisplayClass1_.width, <>c__DisplayClass1_.height));
			//	SketchResolverDefOf.FloorFill.Resolve(parms3);
			//}
			//if (<>c__DisplayClass1_.<ResolveInt>g__CanUse|0(ThingDefOf.Column))
			//{
			//	ResolveParams parms4 = <>c__DisplayClass1_.parms;
			//	parms4.rect = new CellRect?(new CellRect(0, 0, <>c__DisplayClass1_.width, <>c__DisplayClass1_.height));
			//	parms4.sketch = <>c__DisplayClass1_.monument;
			//	parms4.requireFloor = new bool?(true);
			//	SketchResolverDefOf.AddColumns.Resolve(parms4);
			//}
			//this.TryPlaceFurniture(<>c__DisplayClass1_.parms, <>c__DisplayClass1_.monument, (ThingDef def) => (!<>c__DisplayClass1_.onlyBuildableByPlayer || SketchGenUtility.PlayerCanBuildNow(def)) && (<>c__DisplayClass1_.filterAllowsAll || <>c__DisplayClass1_.parms.allowedMonumentThings.Allows(def)));
			//for (int num4 = 0; num4 < 2; num4++)
			//{
			//	ResolveParams parms5 = <>c__DisplayClass1_.parms;
			//	parms5.addFloors = new bool?(false);
			//	parms5.sketch = <>c__DisplayClass1_.monument;
			//	parms5.rect = new CellRect?(new CellRect(0, 0, <>c__DisplayClass1_.width, <>c__DisplayClass1_.height));
			//	SketchResolverDefOf.AddInnerMonuments.Resolve(parms5);
			//}
			//bool flag2 = <>c__DisplayClass1_.parms.allowMonumentDoors ?? (<>c__DisplayClass1_.filterAllowsAll || <>c__DisplayClass1_.parms.allowedMonumentThings.Allows(ThingDefOf.Door));
			//IntVec3 pos;
			//if (flag2 && list.Where(delegate(IntVec3 x)
			//{
			//	if ((!<>c__DisplayClass1_.horizontalSymmetry || x.x < <>c__DisplayClass1_.width / 2) && (!<>c__DisplayClass1_.verticalSymmetry || x.z < <>c__DisplayClass1_.height / 2))
			//	{
			//		if (<>c__DisplayClass1_.monument.ThingsAt(x).Any((SketchThing y) => y.def == ThingDefOf.Wall))
			//		{
			//			return (!<>c__DisplayClass1_.monument.ThingsAt(new IntVec3(x.x - 1, x.y, x.z)).Any<SketchThing>() && !<>c__DisplayClass1_.monument.ThingsAt(new IntVec3(x.x + 1, x.y, x.z)).Any<SketchThing>()) || (!<>c__DisplayClass1_.monument.ThingsAt(new IntVec3(x.x, x.y, x.z - 1)).Any<SketchThing>() && !<>c__DisplayClass1_.monument.ThingsAt(new IntVec3(x.x, x.y, x.z + 1)).Any<SketchThing>());
			//		}
			//	}
			//	return false;
			//}).TryRandomElement(out pos))
			//{
			//	SketchThing sketchThing = <>c__DisplayClass1_.monument.ThingsAt(pos).FirstOrDefault((SketchThing x) => x.def == ThingDefOf.Wall);
			//	if (sketchThing != null)
			//	{
			//		<>c__DisplayClass1_.monument.Remove(sketchThing);
			//		<>c__DisplayClass1_.monument.AddThing(ThingDefOf.Door, pos, Rot4.North, sketchThing.Stuff, 1, null, null, true);
			//	}
			//}
			//this.TryPlaceFurniture(<>c__DisplayClass1_.parms, <>c__DisplayClass1_.monument, (ThingDef def) => (!<>c__DisplayClass1_.onlyBuildableByPlayer || SketchGenUtility.PlayerCanBuildNow(def)) && (<>c__DisplayClass1_.filterAllowsAll || <>c__DisplayClass1_.parms.allowedMonumentThings.Allows(def)));
			//this.ApplySymmetry(<>c__DisplayClass1_.parms, <>c__DisplayClass1_.horizontalSymmetry, <>c__DisplayClass1_.verticalSymmetry, <>c__DisplayClass1_.monument, <>c__DisplayClass1_.width, <>c__DisplayClass1_.height);
			//if (flag2 && !flag)
			//{
			//	SketchThing sketchThing2;
			//	if (!<>c__DisplayClass1_.monument.Things.Any((SketchThing x) => x.def == ThingDefOf.Door) && (from x in <>c__DisplayClass1_.monument.Things
			//	where x.def == ThingDefOf.Wall && ((<>c__DisplayClass1_.monument.Passable(x.pos.x - 1, x.pos.z) && <>c__DisplayClass1_.monument.Passable(x.pos.x + 1, x.pos.z) && <>c__DisplayClass1_.monument.AnyTerrainAt(x.pos.x - 1, x.pos.z) != <>c__DisplayClass1_.monument.AnyTerrainAt(x.pos.x + 1, x.pos.z)) || (<>c__DisplayClass1_.monument.Passable(x.pos.x, x.pos.z - 1) && <>c__DisplayClass1_.monument.Passable(x.pos.x, x.pos.z + 1) && <>c__DisplayClass1_.monument.AnyTerrainAt(x.pos.x, x.pos.z - 1) != <>c__DisplayClass1_.monument.AnyTerrainAt(x.pos.x, x.pos.z + 1)))
			//	select x).TryRandomElement(out sketchThing2))
			//	{
			//		SketchThing sketchThing3 = <>c__DisplayClass1_.monument.ThingsAt(sketchThing2.pos).FirstOrDefault((SketchThing x) => x.def == ThingDefOf.Wall);
			//		if (sketchThing3 != null)
			//		{
			//			<>c__DisplayClass1_.monument.Remove(sketchThing3);
			//		}
			//		<>c__DisplayClass1_.monument.AddThing(ThingDefOf.Door, sketchThing2.pos, Rot4.North, sketchThing2.Stuff, 1, null, null, true);
			//	}
			//}
			//List<SketchThing> things = <>c__DisplayClass1_.monument.Things;
			//for (int num5 = 0; num5 < things.Count; num5++)
			//{
			//	if (things[num5].def == ThingDefOf.Wall)
			//	{
			//		<>c__DisplayClass1_.monument.RemoveTerrain(things[num5].pos);
			//	}
			//}
			//<>c__DisplayClass1_.parms.sketch.MergeAt(<>c__DisplayClass1_.monument, default(IntVec3), Sketch.SpawnPosType.OccupiedCenter, true);
		}

		// Token: 0x06006490 RID: 25744 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		// Token: 0x06006491 RID: 25745 RVA: 0x0022F8AC File Offset: 0x0022DAAC
		private void ApplySymmetry(ResolveParams parms, bool horizontalSymmetry, bool verticalSymmetry, Sketch monument, int width, int height)
		{
			if (horizontalSymmetry)
			{
				ResolveParams parms2 = parms;
				parms2.sketch = monument;
				parms2.symmetryVertical = new bool?(false);
				parms2.symmetryOrigin = new int?(width / 2);
				parms2.symmetryOriginIncluded = new bool?(width % 2 == 1);
				SketchResolverDefOf.Symmetry.Resolve(parms2);
			}
			if (verticalSymmetry)
			{
				ResolveParams parms3 = parms;
				parms3.sketch = monument;
				parms3.symmetryVertical = new bool?(true);
				parms3.symmetryOrigin = new int?(height / 2);
				parms3.symmetryOriginIncluded = new bool?(height % 2 == 1);
				SketchResolverDefOf.Symmetry.Resolve(parms3);
			}
		}

		// Token: 0x06006492 RID: 25746 RVA: 0x0022F94C File Offset: 0x0022DB4C
		private void TryPlaceFurniture(ResolveParams parms, Sketch monument, Func<ThingDef, bool> canUseValidator)
		{
			if (canUseValidator == null || canUseValidator(ThingDefOf.Urn))
			{
				ResolveParams parms2 = parms;
				parms2.sketch = monument;
				parms2.cornerThing = ThingDefOf.Urn;
				parms2.requireFloor = new bool?(true);
				SketchResolverDefOf.AddCornerThings.Resolve(parms2);
			}
			if (canUseValidator == null || canUseValidator(ThingDefOf.SteleLarge))
			{
				ResolveParams parms3 = parms;
				parms3.sketch = monument;
				parms3.thingCentral = ThingDefOf.SteleLarge;
				parms3.requireFloor = new bool?(true);
				SketchResolverDefOf.AddThingsCentral.Resolve(parms3);
			}
			if (canUseValidator == null || canUseValidator(ThingDefOf.SteleGrand))
			{
				ResolveParams parms4 = parms;
				parms4.sketch = monument;
				parms4.thingCentral = ThingDefOf.SteleGrand;
				parms4.requireFloor = new bool?(true);
				SketchResolverDefOf.AddThingsCentral.Resolve(parms4);
			}
			if (canUseValidator == null || canUseValidator(ThingDefOf.Table1x2c))
			{
				ResolveParams parms5 = parms;
				parms5.sketch = monument;
				parms5.wallEdgeThing = ThingDefOf.Table1x2c;
				parms5.requireFloor = new bool?(true);
				SketchResolverDefOf.AddWallEdgeThings.Resolve(parms5);
			}
			if (canUseValidator == null || canUseValidator(ThingDefOf.Table2x2c))
			{
				ResolveParams parms6 = parms;
				parms6.sketch = monument;
				parms6.thingCentral = ThingDefOf.Table2x2c;
				parms6.requireFloor = new bool?(true);
				SketchResolverDefOf.AddThingsCentral.Resolve(parms6);
			}
			if (canUseValidator == null || canUseValidator(ThingDefOf.Sarcophagus))
			{
				ResolveParams parms7 = parms;
				parms7.sketch = monument;
				parms7.wallEdgeThing = ThingDefOf.Sarcophagus;
				parms7.requireFloor = new bool?(true);
				parms7.thingCentral = ThingDefOf.Sarcophagus;
				SketchResolverDefOf.AddWallEdgeThings.Resolve(parms7);
				SketchResolverDefOf.AddThingsCentral.Resolve(parms7);
			}
		}

		// Token: 0x04003D3C RID: 15676
		private static readonly SimpleCurve OpenChancePerSizeCurve = new SimpleCurve
		{
			{
				0f,
				1f,
				true
			},
			{
				3f,
				0.85f,
				true
			},
			{
				6f,
				0.2f,
				true
			},
			{
				8f,
				0f,
				true
			}
		};
	}
}
