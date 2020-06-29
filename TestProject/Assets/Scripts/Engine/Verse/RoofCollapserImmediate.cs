using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse.Sound;

namespace Verse
{
	
	public static class RoofCollapserImmediate
	{
		
		public static void DropRoofInCells(IntVec3 c, Map map, List<Thing> outCrushedThings = null)
		{
			if (!c.Roofed(map))
			{
				return;
			}
			RoofCollapserImmediate.DropRoofInCellPhaseOne(c, map, outCrushedThings);
			RoofCollapserImmediate.DropRoofInCellPhaseTwo(c, map);
			SoundDefOf.Roof_Collapse.PlayOneShot(new TargetInfo(c, map, false));
		}

		
		public static void DropRoofInCells(IEnumerable<IntVec3> cells, Map map, List<Thing> outCrushedThings = null)
		{
			IntVec3 cell = IntVec3.Invalid;
			foreach (IntVec3 c in cells)
			{
				if (c.Roofed(map))
				{
					RoofCollapserImmediate.DropRoofInCellPhaseOne(c, map, outCrushedThings);
				}
			}
			foreach (IntVec3 intVec in cells)
			{
				if (intVec.Roofed(map))
				{
					RoofCollapserImmediate.DropRoofInCellPhaseTwo(intVec, map);
					cell = intVec;
				}
			}
			if (cell.IsValid)
			{
				SoundDefOf.Roof_Collapse.PlayOneShot(new TargetInfo(cell, map, false));
			}
		}

		
		public static void DropRoofInCells(List<IntVec3> cells, Map map, List<Thing> outCrushedThings = null)
		{
			if (cells.NullOrEmpty<IntVec3>())
			{
				return;
			}
			IntVec3 cell = IntVec3.Invalid;
			for (int i = 0; i < cells.Count; i++)
			{
				if (cells[i].Roofed(map))
				{
					RoofCollapserImmediate.DropRoofInCellPhaseOne(cells[i], map, outCrushedThings);
				}
			}
			for (int j = 0; j < cells.Count; j++)
			{
				if (cells[j].Roofed(map))
				{
					RoofCollapserImmediate.DropRoofInCellPhaseTwo(cells[j], map);
					cell = cells[j];
				}
			}
			if (cell.IsValid)
			{
				SoundDefOf.Roof_Collapse.PlayOneShot(new TargetInfo(cell, map, false));
			}
		}

		
		private static void DropRoofInCellPhaseOne(IntVec3 c, Map map, List<Thing> outCrushedThings)
		{
			RoofDef roofDef = map.roofGrid.RoofAt(c);
			if (roofDef == null)
			{
				return;
			}
			if (roofDef.collapseLeavingThingDef != null && roofDef.collapseLeavingThingDef.passability == Traversability.Impassable)
			{
				for (int i = 0; i < 2; i++)
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int j = thingList.Count - 1; j >= 0; j--)
					{
						Thing thing = thingList[j];
						RoofCollapserImmediate.TryAddToCrushedThingsList(thing, outCrushedThings);
						Pawn pawn = thing as Pawn;
						DamageInfo dinfo;
						if (pawn != null)
						{
							dinfo = new DamageInfo(DamageDefOf.Crush, 99999f, 999f, -1f, null, pawn.health.hediffSet.GetBrain(), null, DamageInfo.SourceCategory.Collapse, null);
						}
						else
						{
							dinfo = new DamageInfo(DamageDefOf.Crush, 99999f, 999f, -1f, null, null, null, DamageInfo.SourceCategory.Collapse, null);
							dinfo.SetBodyRegion(BodyPartHeight.Top, BodyPartDepth.Outside);
						}
						BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = null;
						if (i == 0 && pawn != null)
						{
							battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(pawn, RulePackDefOf.DamageEvent_Ceiling, null);
							Find.BattleLog.Add(battleLogEntry_DamageTaken);
						}
						thing.TakeDamage(dinfo).AssociateWithLog(battleLogEntry_DamageTaken);
						if (!thing.Destroyed && thing.def.destroyable)
						{
							thing.Kill(new DamageInfo?(new DamageInfo(DamageDefOf.Crush, 99999f, 999f, -1f, null, null, null, DamageInfo.SourceCategory.Collapse, null)), null);
						}
					}
				}
			}
			else
			{
				List<Thing> thingList2 = c.GetThingList(map);
				for (int k = thingList2.Count - 1; k >= 0; k--)
				{
					Thing thing2 = thingList2[k];
					if (thing2.def.category == ThingCategory.Item || thing2.def.category == ThingCategory.Plant || thing2.def.category == ThingCategory.Building || thing2.def.category == ThingCategory.Pawn)
					{
						RoofCollapserImmediate.TryAddToCrushedThingsList(thing2, outCrushedThings);
						float num = (float)RoofCollapserImmediate.ThinRoofCrushDamageRange.RandomInRange;
						if (thing2.def.building != null)
						{
							num *= thing2.def.building.roofCollapseDamageMultiplier;
						}
						BattleLogEntry_DamageTaken battleLogEntry_DamageTaken2 = null;
						if (thing2 is Pawn)
						{
							battleLogEntry_DamageTaken2 = new BattleLogEntry_DamageTaken((Pawn)thing2, RulePackDefOf.DamageEvent_Ceiling, null);
							Find.BattleLog.Add(battleLogEntry_DamageTaken2);
						}
						DamageInfo dinfo2 = new DamageInfo(DamageDefOf.Crush, (float)GenMath.RoundRandom(num), 0f, -1f, null, null, null, DamageInfo.SourceCategory.Collapse, null);
						dinfo2.SetBodyRegion(BodyPartHeight.Top, BodyPartDepth.Outside);
						thing2.TakeDamage(dinfo2).AssociateWithLog(battleLogEntry_DamageTaken2);
					}
				}
			}
			if (roofDef.collapseLeavingThingDef != null)
			{
				Thing thing3 = GenSpawn.Spawn(roofDef.collapseLeavingThingDef, c, map, WipeMode.Vanish);
				if (thing3.def.rotatable)
				{
					thing3.Rotation = Rot4.Random;
				}
			}
			for (int l = 0; l < 1; l++)
			{
				MoteMaker.ThrowDustPuff(c.ToVector3Shifted() + Gen.RandomHorizontalVector(0.6f), map, 2f);
			}
		}

		
		private static void DropRoofInCellPhaseTwo(IntVec3 c, Map map)
		{
			RoofDef roofDef = map.roofGrid.RoofAt(c);
			if (roofDef == null)
			{
				return;
			}
			if (roofDef.filthLeaving != null)
			{
				FilthMaker.TryMakeFilth(c, map, roofDef.filthLeaving, 1, FilthSourceFlags.None);
			}
			if (roofDef.VanishOnCollapse)
			{
				map.roofGrid.SetRoof(c, null);
			}
			CellRect bound = CellRect.CenteredOn(c, 2);
			IEnumerable<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
			
			Func<Pawn, bool> predicate;
			if ((predicate=default ) == null)
			{
				predicate = ( ((Pawn pawn) => bound.Contains(pawn.Position)));
			}
			foreach (Pawn pawn2 in allPawnsSpawned.Where(predicate))
			{
				TaleRecorder.RecordTale(TaleDefOf.CollapseDodged, new object[]
				{
					pawn2
				});
			}
		}

		
		private static void TryAddToCrushedThingsList(Thing t, List<Thing> outCrushedThings)
		{
			if (outCrushedThings == null)
			{
				return;
			}
			if (!outCrushedThings.Contains(t) && RoofCollapserImmediate.WorthMentioningInCrushLetter(t))
			{
				outCrushedThings.Add(t);
			}
		}

		
		private static bool WorthMentioningInCrushLetter(Thing t)
		{
			if (!t.def.destroyable)
			{
				return false;
			}
			switch (t.def.category)
			{
			case ThingCategory.Pawn:
				return true;
			case ThingCategory.Item:
				return t.MarketValue > 0.01f;
			case ThingCategory.Building:
				return true;
			default:
				return false;
			}
		}

		
		private static readonly IntRange ThinRoofCrushDamageRange = new IntRange(15, 30);
	}
}
