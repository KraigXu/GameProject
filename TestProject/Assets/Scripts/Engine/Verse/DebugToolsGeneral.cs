﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.BaseGen;
using RimWorld.SketchGen;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200033B RID: 827
	public static class DebugToolsGeneral
	{
		// Token: 0x0600186B RID: 6251 RVA: 0x0008C71C File Offset: 0x0008A91C
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Destroy()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x0008C780 File Offset: 0x0008A980
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Kill()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.Kill(null, null);
			}
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x0008C7F0 File Offset: 0x0008A9F0
		[DebugAction("General", "10 damage", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Take10Damage()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x0008C874 File Offset: 0x0008AA74
		[DebugAction("General", "5000 damage", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Take5000Damage()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 50000f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x0008C8F8 File Offset: 0x0008AAF8
		[DebugAction("General", "5000 flame damage", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Take5000FlameDamage()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.TakeDamage(new DamageInfo(DamageDefOf.Flame, 5000f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x0008C97C File Offset: 0x0008AB7C
		[DebugAction("General", "Clear area 21x21", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ClearArea21x21()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				GenDebug.ClearArea(CellRect.CenteredOn(UI.MouseCell(), 10), Find.CurrentMap);
			}
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x0008C9F4 File Offset: 0x0008ABF4
		[DebugAction("General", "Rock 21x21", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Rock21x21()
		{
			CellRect cellRect = CellRect.CenteredOn(UI.MouseCell(), 10);
			cellRect.ClipInsideMap(Find.CurrentMap);
			foreach (IntVec3 loc in cellRect)
			{
				GenSpawn.Spawn(ThingDefOf.Granite, loc, Find.CurrentMap, WipeMode.Vanish);
			}
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x0008CA68 File Offset: 0x0008AC68
		[DebugAction("General", "Destroy trees 21x21", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DestroyTrees21x21()
		{
			CellRect cellRect = CellRect.CenteredOn(UI.MouseCell(), 10);
			cellRect.ClipInsideMap(Find.CurrentMap);
			foreach (IntVec3 c in cellRect)
			{
				List<Thing> thingList = c.GetThingList(Find.CurrentMap);
				for (int i = thingList.Count - 1; i >= 0; i--)
				{
					if (thingList[i].def.category == ThingCategory.Plant && thingList[i].def.plant.IsTree)
					{
						thingList[i].Destroy(DestroyMode.Vanish);
					}
				}
			}
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x0008CB20 File Offset: 0x0008AD20
		[DebugAction("General", "Explosion (bomb)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionBomb()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.Bomb, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x0008CB70 File Offset: 0x0008AD70
		[DebugAction("General", "Explosion (flame)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionFlame()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x0008CBC0 File Offset: 0x0008ADC0
		[DebugAction("General", "Explosion (stun)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionStun()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.Stun, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x0008CC10 File Offset: 0x0008AE10
		[DebugAction("General", "Explosion (EMP)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionEMP()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.EMP, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x0008CC60 File Offset: 0x0008AE60
		[DebugAction("General", "Explosion (extinguisher)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionExtinguisher()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 10f, DamageDefOf.Extinguish, null, -1, -1f, null, null, null, null, ThingDefOf.Filth_FireFoam, 1f, 3, true, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x0008CCB4 File Offset: 0x0008AEB4
		[DebugAction("General", "Explosion (smoke)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionSmoke()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 10f, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, ThingDefOf.Gas_Smoke, 1f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x0008CD08 File Offset: 0x0008AF08
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void LightningStrike()
		{
			Find.CurrentMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(Find.CurrentMap, UI.MouseCell()));
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x0008CD2D File Offset: 0x0008AF2D
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddSnow()
		{
			SnowUtility.AddSnowRadial(UI.MouseCell(), Find.CurrentMap, 5f, 1f);
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x0008CD48 File Offset: 0x0008AF48
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveSnow()
		{
			SnowUtility.AddSnowRadial(UI.MouseCell(), Find.CurrentMap, 5f, -1f);
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x0008CD64 File Offset: 0x0008AF64
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ClearAllSnow()
		{
			foreach (IntVec3 c in Find.CurrentMap.AllCells)
			{
				Find.CurrentMap.snowGrid.SetDepth(c, 0f);
			}
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x0008CDC4 File Offset: 0x0008AFC4
		[DebugAction("General", "Push heat (10)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PushHeat10()
		{
			foreach (IntVec3 intVec in Find.CurrentMap.AllCells)
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.CurrentMap, 10f);
			}
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x0008CE24 File Offset: 0x0008B024
		[DebugAction("General", "Push heat (1000)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PushHeat1000()
		{
			foreach (IntVec3 intVec in Find.CurrentMap.AllCells)
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.CurrentMap, 1000f);
			}
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x0008CE84 File Offset: 0x0008B084
		[DebugAction("General", "Push heat (-1000)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PushHeatNeg1000()
		{
			foreach (IntVec3 intVec in Find.CurrentMap.AllCells)
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.CurrentMap, -1000f);
			}
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x0008CEE4 File Offset: 0x0008B0E4
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FinishPlantGrowth()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				Plant plant = thing as Plant;
				if (plant != null)
				{
					plant.Growth = 1f;
				}
			}
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x0008CF4C File Offset: 0x0008B14C
		[DebugAction("General", "Grow 1 day", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Grow1Day()
		{
			IntVec3 intVec = UI.MouseCell();
			Plant plant = intVec.GetPlant(Find.CurrentMap);
			if (plant != null && plant.def.plant != null)
			{
				int num = (int)((1f - plant.Growth) * plant.def.plant.growDays);
				if (num >= 60000)
				{
					plant.Age += 60000;
				}
				else if (num > 0)
				{
					plant.Age += num;
				}
				plant.Growth += 1f / plant.def.plant.growDays;
				if ((double)plant.Growth > 1.0)
				{
					plant.Growth = 1f;
				}
				Find.CurrentMap.mapDrawer.SectionAt(intVec).RegenerateAllLayers();
			}
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x0008D024 File Offset: 0x0008B224
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GrowToMaturity()
		{
			IntVec3 intVec = UI.MouseCell();
			Plant plant = intVec.GetPlant(Find.CurrentMap);
			if (plant != null && plant.def.plant != null)
			{
				int num = (int)((1f - plant.Growth) * plant.def.plant.growDays);
				plant.Age += num;
				plant.Growth = 1f;
				Find.CurrentMap.mapDrawer.SectionAt(intVec).RegenerateAllLayers();
			}
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x0008D0A0 File Offset: 0x0008B2A0
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RegenSection()
		{
			Find.CurrentMap.mapDrawer.SectionAt(UI.MouseCell()).RegenerateAllLayers();
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x0008D0BC File Offset: 0x0008B2BC
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RandomizeColor()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				if (thing.TryGetComp<CompColorable>() != null)
				{
					thing.SetColor(GenColor.RandomColorOpaque(), true);
				}
			}
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x0008D124 File Offset: 0x0008B324
		[DebugAction("General", "Rot 1 day", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Rot1Day()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				CompRottable compRottable = thing.TryGetComp<CompRottable>();
				if (compRottable != null)
				{
					compRottable.RotProgress += 60000f;
				}
			}
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x0008D194 File Offset: 0x0008B394
		[DebugAction("General", "Force sleep", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceSleep()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				CompCanBeDormant compCanBeDormant = thing.TryGetComp<CompCanBeDormant>();
				if (compCanBeDormant != null)
				{
					compCanBeDormant.ToSleep();
				}
				else
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.LayDown, pawn.Position), JobCondition.None, null, false, true, null, null, false, false);
					}
				}
			}
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x0008D240 File Offset: 0x0008B440
		[DebugAction("General", "Fuel -20%", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FuelRemove20Percent()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				CompRefuelable compRefuelable = thing.TryGetComp<CompRefuelable>();
				if (compRefuelable != null)
				{
					compRefuelable.ConsumeFuel(compRefuelable.Props.fuelCapacity * 0.2f);
				}
			}
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x0008D2B4 File Offset: 0x0008B4B4
		[DebugAction("General", "Break down...", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BreakDown()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				CompBreakdownable compBreakdownable = thing.TryGetComp<CompBreakdownable>();
				if (compBreakdownable != null && !compBreakdownable.BrokenDown)
				{
					compBreakdownable.DoBreakdown();
				}
			}
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x0008D320 File Offset: 0x0008B520
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void UseScatterer()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_MapGen.Options_Scatterers()));
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x0008D338 File Offset: 0x0008B538
		[DebugAction("General", "BaseGen", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BaseGen()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (string text in (from x in DefDatabase<RuleDef>.AllDefs
			select x.symbol).Distinct<string>())
			{
				string localSymbol = text;
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate
				{
					DebugTool tool = null;
					IntVec3 firstCorner;
	
					tool = new DebugTool("first corner...", delegate
					{
						firstCorner = UI.MouseCell();
						string label = "second corner...";
						Action clickAction;
						clickAction = delegate
						{
							IntVec3 second = UI.MouseCell();
							CellRect rect = CellRect.FromLimits(firstCorner, second).ClipInsideMap(Find.CurrentMap);
							RimWorld.BaseGen.BaseGen.globalSettings.map = Find.CurrentMap;
							RimWorld.BaseGen.BaseGen.symbolStack.Push(localSymbol, rect, null);
							RimWorld.BaseGen.BaseGen.Generate();
							DebugTools.curTool = tool;
						};
						DebugTools.curTool = new DebugTool(label, clickAction, firstCorner);
					}, null);
					DebugTools.curTool = tool;
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x0008D3E4 File Offset: 0x0008B5E4
		[DebugAction("General", "SketchGen", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SketchGen()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (SketchResolverDef sketchResolverDef in from x in DefDatabase<SketchResolverDef>.AllDefs
			where x.isRoot
			select x)
			{
				SketchResolverDef localResolver = sketchResolverDef;
				if (localResolver == SketchResolverDefOf.Monument || localResolver == SketchResolverDefOf.MonumentRuin)
				{
					List<DebugMenuOption> sizeOpts = new List<DebugMenuOption>();
					for (int i = 1; i <= 60; i++)
					{
						int localIndex = i;
						sizeOpts.Add(new DebugMenuOption(localIndex.ToString(), DebugMenuOptionMode.Tool, delegate
						{
							RimWorld.SketchGen.ResolveParams parms = default(RimWorld.SketchGen.ResolveParams);
							parms.sketch = new Sketch();
							parms.monumentSize = new IntVec2?(new IntVec2(localIndex, localIndex));
							RimWorld.SketchGen.SketchGen.Generate(localResolver, parms).Spawn(Find.CurrentMap, UI.MouseCell(), null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, null, false, true, null, null);
						}));
					}
					list.Add(new DebugMenuOption(sketchResolverDef.defName, DebugMenuOptionMode.Action, delegate
					{
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(sizeOpts));
					}));
				}
				else
				{
					list.Add(new DebugMenuOption(sketchResolverDef.defName, DebugMenuOptionMode.Tool, delegate
					{
						RimWorld.SketchGen.ResolveParams parms = default(RimWorld.SketchGen.ResolveParams);
						parms.sketch = new Sketch();
						RimWorld.SketchGen.SketchGen.Generate(localResolver, parms).Spawn(Find.CurrentMap, UI.MouseCell(), null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, null, false, true, null, null);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x0008D548 File Offset: 0x0008B748
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeRoof()
		{
			foreach (IntVec3 c in CellRect.CenteredOn(UI.MouseCell(), 1))
			{
				Find.CurrentMap.roofGrid.SetRoof(c, RoofDefOf.RoofConstructed);
			}
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x0008D5B4 File Offset: 0x0008B7B4
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DeleteRoof()
		{
			foreach (IntVec3 c in CellRect.CenteredOn(UI.MouseCell(), 1))
			{
				Find.CurrentMap.roofGrid.SetRoof(c, null);
			}
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x0008D61C File Offset: 0x0008B81C
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestFloodUnfog()
		{
			FloodFillerFog.DebugFloodUnfog(UI.MouseCell(), Find.CurrentMap);
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x0008D630 File Offset: 0x0008B830
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashClosewalkCell30()
		{
			IntVec3 c = CellFinder.RandomClosewalkCellNear(UI.MouseCell(), Find.CurrentMap, 30, null);
			Find.CurrentMap.debugDrawer.FlashCell(c, 0f, null, 50);
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x0008D668 File Offset: 0x0008B868
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashWalkPath()
		{
			WalkPathFinder.DebugFlashWalkPath(UI.MouseCell(), 8);
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x0008D678 File Offset: 0x0008B878
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashSkygazeCell()
		{
			Pawn pawn = Find.CurrentMap.mapPawns.FreeColonists.First<Pawn>();
			IntVec3 c;
			RCellFinder.TryFindSkygazeCell(UI.MouseCell(), pawn, out c);
			Find.CurrentMap.debugDrawer.FlashCell(c, 0f, null, 50);
			MoteMaker.ThrowText(c.ToVector3Shifted(), Find.CurrentMap, "for " + pawn.Label, Color.white, -1f);
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x0008D6EC File Offset: 0x0008B8EC
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashDirectFleeDest()
		{
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn == null)
			{
				Find.CurrentMap.debugDrawer.FlashCell(UI.MouseCell(), 0f, "select a pawn", 50);
				return;
			}
			IntVec3 c;
			if (RCellFinder.TryFindDirectFleeDestination(UI.MouseCell(), 9f, pawn, out c))
			{
				Find.CurrentMap.debugDrawer.FlashCell(c, 0.5f, null, 50);
				return;
			}
			Find.CurrentMap.debugDrawer.FlashCell(UI.MouseCell(), 0.8f, "not found", 50);
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x0008D77C File Offset: 0x0008B97C
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashSpectatorsCells()
		{
			Action<bool> act = delegate(bool bestSideOnly)
			{
				DebugTool tool = null;
				IntVec3 firstCorner;
				tool = new DebugTool("first watch rect corner...", delegate
				{
					firstCorner = UI.MouseCell();
					string label = "second watch rect corner...";
					Action clickAction;
					clickAction =  delegate
					{
						IntVec3 second = UI.MouseCell();
						CellRect spectateRect = CellRect.FromLimits(firstCorner, second).ClipInsideMap(Find.CurrentMap);
						SpectateRectSide allowedSides = SpectateRectSide.All;
						if (bestSideOnly)
						{
							allowedSides = SpectatorCellFinder.FindSingleBestSide(spectateRect, Find.CurrentMap, SpectateRectSide.All, 1);
						}
						SpectatorCellFinder.DebugFlashPotentialSpectatorCells(spectateRect, Find.CurrentMap, allowedSides, 1);
						DebugTools.curTool = tool;
					};
					DebugTools.curTool = new DebugTool(label, clickAction, firstCorner);
				}, null);
				DebugTools.curTool = tool;
			};
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("All sides", DebugMenuOptionMode.Action, delegate
			{
				act(false);
			}));
			list.Add(new DebugMenuOption("Best side only", DebugMenuOptionMode.Action, delegate
			{
				act(true);
			}));
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x0008D804 File Offset: 0x0008BA04
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CheckReachability()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			TraverseMode[] array = (TraverseMode[])Enum.GetValues(typeof(TraverseMode));
			for (int i = 0; i < array.Length; i++)
			{
				TraverseMode traverseMode2 = array[i];
				TraverseMode traverseMode = traverseMode2;
				list.Add(new DebugMenuOption(traverseMode2.ToString(), DebugMenuOptionMode.Action, delegate
				{
					DebugTool tool = null;
					IntVec3 from;
					Pawn fromPawn;
					tool = new DebugTool("from...", delegate
					{
						from = UI.MouseCell();
						fromPawn = from.GetFirstPawn(Find.CurrentMap);
						string text = "to...";
						if (fromPawn != null)
						{
							text = text + " (pawn=" + fromPawn.LabelShort + ")";
						}
						string label = text;
						Action clickAction;
						clickAction = delegate
						{
							DebugTools.curTool = tool;
						};
						Action onGUIAction;
						onGUIAction =delegate
						{
							IntVec3 c = UI.MouseCell();
							bool flag;
							IntVec3 intVec;
							if (fromPawn != null)
							{
								flag = fromPawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, traverseMode);
								intVec = fromPawn.Position;
							}
							else
							{
								flag = Find.CurrentMap.reachability.CanReach(from, c, PathEndMode.OnCell, traverseMode, Danger.Deadly);
								intVec = from;
							}
							Color color = flag ? Color.green : Color.red;
							Widgets.DrawLine(intVec.ToUIPosition(), c.ToUIPosition(), color, 2f);
						};
						DebugTools.curTool = new DebugTool(label, clickAction, onGUIAction);
					}, null);
					DebugTools.curTool = tool;
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x0008D884 File Offset: 0x0008BA84
		[DebugAction("General", "Flash TryFindRandomPawnExitCell", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashTryFindRandomPawnExitCell(Pawn p)
		{
			IntVec3 intVec;
			if (CellFinder.TryFindRandomPawnExitCell(p, out intVec))
			{
				p.Map.debugDrawer.FlashCell(intVec, 0.5f, null, 50);
				p.Map.debugDrawer.FlashLine(p.Position, intVec, 50, SimpleColor.White);
				return;
			}
			p.Map.debugDrawer.FlashCell(p.Position, 0.2f, "no exit cell", 50);
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x0008D8F4 File Offset: 0x0008BAF4
		[DebugAction("General", "RandomSpotJustOutsideColony", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RandomSpotJustOutsideColony(Pawn p)
		{
			IntVec3 intVec;
			if (RCellFinder.TryFindRandomSpotJustOutsideColony(p, out intVec))
			{
				p.Map.debugDrawer.FlashCell(intVec, 0.5f, null, 50);
				p.Map.debugDrawer.FlashLine(p.Position, intVec, 50, SimpleColor.White);
				return;
			}
			p.Map.debugDrawer.FlashCell(p.Position, 0.2f, "no cell", 50);
		}
	}
}
