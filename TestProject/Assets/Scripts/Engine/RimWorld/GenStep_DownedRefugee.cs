using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A67 RID: 2663
	public class GenStep_DownedRefugee : GenStep_Scatterer
	{
		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x06003EDB RID: 16091 RVA: 0x0014E1F8 File Offset: 0x0014C3F8
		public override int SeedPart
		{
			get
			{
				return 931842770;
			}
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x0014E1FF File Offset: 0x0014C3FF
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return base.CanScatterAt(c, map) && c.Standable(map);
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x0014E214 File Offset: 0x0014C414
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			Pawn pawn;
			if (parms.sitePart != null && parms.sitePart.things != null && parms.sitePart.things.Any)
			{
				pawn = (Pawn)parms.sitePart.things.Take(parms.sitePart.things[0]);
			}
			else
			{
				DownedRefugeeComp component = map.Parent.GetComponent<DownedRefugeeComp>();
				if (component != null && component.pawn.Any)
				{
					pawn = component.pawn.Take(component.pawn[0]);
				}
				else
				{
					pawn = DownedRefugeeQuestUtility.GenerateRefugee(map.Tile);
				}
			}
			HealthUtility.DamageUntilDowned(pawn, false);
			HealthUtility.DamageLegsUntilIncapableOfMoving(pawn, false);
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			pawn.mindState.WillJoinColonyIfRescued = true;
			MapGenerator.rootsToUnfog.Add(loc);
			MapGenerator.SetVar<CellRect>("RectOfInterest", CellRect.CenteredOn(loc, 1, 1));
		}
	}
}
