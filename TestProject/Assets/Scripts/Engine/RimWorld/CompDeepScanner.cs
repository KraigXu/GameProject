using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompDeepScanner : CompScanner
	{
		
		
		public new CompProperties_ScannerMineralsDeep Props
		{
			get
			{
				return this.props as CompProperties_ScannerMineralsDeep;
			}
		}

		
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.powerComp.PowerOn)
			{
				this.parent.Map.deepResourceGrid.MarkForDraw();
			}
		}

		
		protected override void DoFind(Pawn worker)
		{
			Map map = this.parent.Map;
			IntVec3 intVec;
			if (!CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.CanScatterAt(x, map), map, out intVec))
			{
				Log.Error("Could not find a center cell for deep scanning lump generation!", false);
			}
			ThingDef thingDef = this.ChooseLumpThingDef();
			int numCells = Mathf.CeilToInt((float)thingDef.deepLumpSizeRange.RandomInRange);
			foreach (IntVec3 intVec2 in GridShapeMaker.IrregularLump(intVec, map, numCells))
			{
				if (this.CanScatterAt(intVec2, map) && !intVec2.InNoBuildEdgeArea(map))
				{
					map.deepResourceGrid.SetAt(intVec2, thingDef, thingDef.deepCountPerCell);
				}
			}
			string key;
			if ("LetterDeepScannerFoundLump".CanTranslate())
			{
				key = "LetterDeepScannerFoundLump";
			}
			else if ("DeepScannerFoundLump".CanTranslate())
			{
				key = "DeepScannerFoundLump";
			}
			else
			{
				key = "LetterDeepScannerFoundLump";
			}
			Find.LetterStack.ReceiveLetter("LetterLabelDeepScannerFoundLump".Translate() + ": " + thingDef.LabelCap, key.Translate(thingDef.label, worker.Named("FINDER")), LetterDefOf.PositiveEvent, new LookTargets(intVec, map), null, null, null, null);
		}

		
		private bool CanScatterAt(IntVec3 pos, Map map)
		{
			int num = CellIndicesUtility.CellToIndex(pos, map.Size.x);
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(num);
			return (terrainDef == null || !terrainDef.IsWater || terrainDef.passability != Traversability.Impassable) && terrainDef.affordances.Contains(ThingDefOf.DeepDrill.terrainAffordanceNeeded) && !map.deepResourceGrid.GetCellBool(num);
		}

		
		protected ThingDef ChooseLumpThingDef()
		{
			return DefDatabase<ThingDef>.AllDefs.RandomElementByWeight((ThingDef def) => def.deepCommonality);
		}
	}
}
