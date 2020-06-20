using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009F1 RID: 2545
	public class IncidentWorker_ShipChunkDrop : IncidentWorker
	{
		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06003C89 RID: 15497 RVA: 0x0013FDEC File Offset: 0x0013DFEC
		private int RandomCountToDrop
		{
			get
			{
				float x2 = (float)Find.TickManager.TicksGame / 3600000f;
				float timePassedFactor = Mathf.Clamp(GenMath.LerpDouble(0f, 1.2f, 1f, 0.1f, x2), 0.1f, 1f);
				return IncidentWorker_ShipChunkDrop.CountChance.RandomElementByWeight(delegate(Pair<int, float> x)
				{
					if (x.First == 1)
					{
						return x.Second;
					}
					return x.Second * timePassedFactor;
				}).First;
			}
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x0013FE60 File Offset: 0x0013E060
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return this.TryFindShipChunkDropCell(map.Center, map, 999999, out intVec);
		}

		// Token: 0x06003C8B RID: 15499 RVA: 0x0013FE98 File Offset: 0x0013E098
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			if (!this.TryFindShipChunkDropCell(map.Center, map, 999999, out intVec))
			{
				return false;
			}
			this.SpawnShipChunks(intVec, map, this.RandomCountToDrop);
			Messages.Message("MessageShipChunkDrop".Translate(), new TargetInfo(intVec, map, false), MessageTypeDefOf.NeutralEvent, true);
			return true;
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x0013FF00 File Offset: 0x0013E100
		private void SpawnShipChunks(IntVec3 firstChunkPos, Map map, int count)
		{
			this.SpawnChunk(firstChunkPos, map);
			for (int i = 0; i < count - 1; i++)
			{
				IntVec3 pos;
				if (this.TryFindShipChunkDropCell(firstChunkPos, map, 5, out pos))
				{
					this.SpawnChunk(pos, map);
				}
			}
		}

		// Token: 0x06003C8D RID: 15501 RVA: 0x0013FF38 File Offset: 0x0013E138
		private void SpawnChunk(IntVec3 pos, Map map)
		{
			SkyfallerMaker.SpawnSkyfaller(ThingDefOf.ShipChunkIncoming, ThingDefOf.ShipChunk, pos, map);
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x0013FF4C File Offset: 0x0013E14C
		private bool TryFindShipChunkDropCell(IntVec3 nearLoc, Map map, int maxDist, out IntVec3 pos)
		{
			return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.ShipChunkIncoming, map, out pos, 10, nearLoc, maxDist, true, false, false, false, true, false, null);
		}

		// Token: 0x04002392 RID: 9106
		private static readonly Pair<int, float>[] CountChance = new Pair<int, float>[]
		{
			new Pair<int, float>(1, 1f),
			new Pair<int, float>(2, 0.95f),
			new Pair<int, float>(3, 0.7f),
			new Pair<int, float>(4, 0.4f)
		};
	}
}
