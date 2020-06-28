using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200126D RID: 4717
	public class TransportPodsArrivalAction_LandInSpecificCell : TransportPodsArrivalAction
	{
		// Token: 0x06006E5D RID: 28253 RVA: 0x0026813D File Offset: 0x0026633D
		public TransportPodsArrivalAction_LandInSpecificCell()
		{
		}

		// Token: 0x06006E5E RID: 28254 RVA: 0x002689AA File Offset: 0x00266BAA
		public TransportPodsArrivalAction_LandInSpecificCell(MapParent mapParent, IntVec3 cell)
		{
			this.mapParent = mapParent;
			this.cell = cell;
		}

		// Token: 0x06006E5F RID: 28255 RVA: 0x002689C0 File Offset: 0x00266BC0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
		}

		// Token: 0x06006E60 RID: 28256 RVA: 0x00268A00 File Offset: 0x00266C00
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.mapParent != null && this.mapParent.Tile != destinationTile)
			{
				return false;
			}
			return TransportPodsArrivalAction_LandInSpecificCell.CanLandInSpecificCell(pods, this.mapParent);
		}

		// Token: 0x06006E61 RID: 28257 RVA: 0x00268A50 File Offset: 0x00266C50
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			Thing lookTarget = TransportPodsArrivalActionUtility.GetLookTarget(pods);
			TransportPodsArrivalActionUtility.DropTravelingTransportPods(pods, this.cell, this.mapParent.Map);
			Messages.Message("MessageTransportPodsArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x06006E62 RID: 28258 RVA: 0x00268A9C File Offset: 0x00266C9C
		public static bool CanLandInSpecificCell(IEnumerable<IThingHolder> pods, MapParent mapParent)
		{
			return mapParent != null && mapParent.Spawned && mapParent.HasMap && (!mapParent.EnterCooldownBlocksEntering() || FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(mapParent.EnterCooldownDaysLeft().ToString("0.#"))));
		}

		// Token: 0x04004425 RID: 17445
		private MapParent mapParent;

		// Token: 0x04004426 RID: 17446
		private IntVec3 cell;
	}
}
