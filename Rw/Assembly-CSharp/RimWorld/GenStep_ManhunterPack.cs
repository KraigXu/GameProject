using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A69 RID: 2665
	public class GenStep_ManhunterPack : GenStep
	{
		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06003EE3 RID: 16099 RVA: 0x0014E4DE File Offset: 0x0014C6DE
		public override int SeedPart
		{
			get
			{
				return 457293335;
			}
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x0014E4E8 File Offset: 0x0014C6E8
		public override void Generate(Map map, GenStepParams parms)
		{
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false);
			IntVec3 root;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map) && map.reachability.CanReachMapEdge(x, traverseParams) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= this.MinRoomCells, map, out root))
			{
				float points = (parms.sitePart != null) ? parms.sitePart.parms.threatPoints : this.defaultPointsRange.RandomInRange;
				PawnKindDef animalKind;
				if (parms.sitePart != null && parms.sitePart.parms.animalKind != null)
				{
					animalKind = parms.sitePart.parms.animalKind;
				}
				else if (!ManhunterPackGenStepUtility.TryGetAnimalsKind(points, map.Tile, out animalKind))
				{
					return;
				}
				List<Pawn> list = ManhunterPackIncidentUtility.GenerateAnimals_NewTmp(animalKind, map.Tile, points, 0);
				for (int i = 0; i < list.Count; i++)
				{
					IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(root, map, 10);
					GenSpawn.Spawn(list[i], loc, map, Rot4.Random, WipeMode.Vanish, false);
					list[i].health.AddHediff(HediffDefOf.Scaria, null, null, null);
					list[i].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, null, false);
				}
			}
		}

		// Token: 0x0400249B RID: 9371
		public FloatRange defaultPointsRange = new FloatRange(300f, 500f);

		// Token: 0x0400249C RID: 9372
		private int MinRoomCells = 225;
	}
}
