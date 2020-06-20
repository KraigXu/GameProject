using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D83 RID: 3459
	public class CompDeepScanner : CompScanner
	{
		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x06005450 RID: 21584 RVA: 0x001C25FB File Offset: 0x001C07FB
		public new CompProperties_ScannerMineralsDeep Props
		{
			get
			{
				return this.props as CompProperties_ScannerMineralsDeep;
			}
		}

		// Token: 0x06005451 RID: 21585 RVA: 0x001C2608 File Offset: 0x001C0808
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.powerComp.PowerOn)
			{
				this.parent.Map.deepResourceGrid.MarkForDraw();
			}
		}

		// Token: 0x06005452 RID: 21586 RVA: 0x001C262C File Offset: 0x001C082C
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

		// Token: 0x06005453 RID: 21587 RVA: 0x001C27A4 File Offset: 0x001C09A4
		private bool CanScatterAt(IntVec3 pos, Map map)
		{
			int num = CellIndicesUtility.CellToIndex(pos, map.Size.x);
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(num);
			return (terrainDef == null || !terrainDef.IsWater || terrainDef.passability != Traversability.Impassable) && terrainDef.affordances.Contains(ThingDefOf.DeepDrill.terrainAffordanceNeeded) && !map.deepResourceGrid.GetCellBool(num);
		}

		// Token: 0x06005454 RID: 21588 RVA: 0x001C280C File Offset: 0x001C0A0C
		protected ThingDef ChooseLumpThingDef()
		{
			return DefDatabase<ThingDef>.AllDefs.RandomElementByWeight((ThingDef def) => def.deepCommonality);
		}
	}
}
