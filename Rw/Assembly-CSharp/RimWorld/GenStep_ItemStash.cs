using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A68 RID: 2664
	public class GenStep_ItemStash : GenStep_Scatterer
	{
		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x06003EDF RID: 16095 RVA: 0x0014E2F4 File Offset: 0x0014C4F4
		public override int SeedPart
		{
			get
			{
				return 913432591;
			}
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x0014E2FC File Offset: 0x0014C4FC
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)))
			{
				return false;
			}
			CellRect rect = CellRect.CenteredOn(c, 7, 7);
			List<CellRect> list;
			if (MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list) && list.Any((CellRect x) => x.Overlaps(rect)))
			{
				return false;
			}
			foreach (IntVec3 c2 in rect)
			{
				if (!c2.InBounds(map) || c2.GetEdifice(map) != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x0014E3D0 File Offset: 0x0014C5D0
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			CellRect cellRect = CellRect.CenteredOn(loc, 7, 7).ClipInsideMap(map);
			List<CellRect> list;
			if (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list))
			{
				list = new List<CellRect>();
				MapGenerator.SetVar<List<CellRect>>("UsedRects", list);
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = cellRect;
			resolveParams.faction = map.ParentFaction;
			if (parms.sitePart != null && parms.sitePart.things != null && parms.sitePart.things.Any)
			{
				resolveParams.stockpileConcreteContents = parms.sitePart.things;
			}
			else
			{
				ItemStashContentsComp component = map.Parent.GetComponent<ItemStashContentsComp>();
				if (component != null && component.contents.Any)
				{
					resolveParams.stockpileConcreteContents = component.contents;
				}
				else
				{
					resolveParams.thingSetMakerDef = (this.thingSetMakerDef ?? ThingSetMakerDefOf.MapGen_DefaultStockpile);
				}
			}
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("storage", resolveParams, null);
			BaseGen.Generate();
			MapGenerator.SetVar<CellRect>("RectOfInterest", cellRect);
			list.Add(cellRect);
		}

		// Token: 0x04002499 RID: 9369
		public ThingSetMakerDef thingSetMakerDef;

		// Token: 0x0400249A RID: 9370
		private const int Size = 7;
	}
}
