using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CBC RID: 3260
	public class DropPodLeaving : Skyfaller, IActiveDropPod, IThingHolder
	{
		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x06004F07 RID: 20231 RVA: 0x001A993D File Offset: 0x001A7B3D
		// (set) Token: 0x06004F08 RID: 20232 RVA: 0x001A9955 File Offset: 0x001A7B55
		public ActiveDropPodInfo Contents
		{
			get
			{
				return ((ActiveDropPod)this.innerContainer[0]).Contents;
			}
			set
			{
				((ActiveDropPod)this.innerContainer[0]).Contents = value;
			}
		}

		// Token: 0x06004F09 RID: 20233 RVA: 0x001A9A60 File Offset: 0x001A7C60
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
			Scribe_Deep.Look<TransportPodsArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.alreadyLeft, "alreadyLeft", false, false);
		}

		// Token: 0x06004F0A RID: 20234 RVA: 0x001A9AC0 File Offset: 0x001A7CC0
		protected override void LeaveMap()
		{
			if (this.alreadyLeft)
			{
				base.LeaveMap();
				return;
			}
			if (this.groupID < 0)
			{
				Log.Error("Drop pod left the map, but its group ID is " + this.groupID, false);
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.destinationTile < 0)
			{
				Log.Error("Drop pod left the map, but its destination tile is " + this.destinationTile, false);
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			Lord lord = TransporterUtility.FindLord(this.groupID, base.Map);
			if (lord != null)
			{
				base.Map.lordManager.RemoveLord(lord);
			}
			TravelingTransportPods travelingTransportPods = (TravelingTransportPods)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.TravelingTransportPods);
			travelingTransportPods.Tile = base.Map.Tile;
			travelingTransportPods.SetFaction(Faction.OfPlayer);
			travelingTransportPods.destinationTile = this.destinationTile;
			travelingTransportPods.arrivalAction = this.arrivalAction;
			Find.WorldObjects.Add(travelingTransportPods);
			DropPodLeaving.tmpActiveDropPods.Clear();
			DropPodLeaving.tmpActiveDropPods.AddRange(base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ActiveDropPod));
			for (int i = 0; i < DropPodLeaving.tmpActiveDropPods.Count; i++)
			{
				DropPodLeaving dropPodLeaving = DropPodLeaving.tmpActiveDropPods[i] as DropPodLeaving;
				if (dropPodLeaving != null && dropPodLeaving.groupID == this.groupID)
				{
					dropPodLeaving.alreadyLeft = true;
					travelingTransportPods.AddPod(dropPodLeaving.Contents, true);
					dropPodLeaving.Contents = null;
					dropPodLeaving.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x04002C5A RID: 11354
		public int groupID = -1;

		// Token: 0x04002C5B RID: 11355
		public int destinationTile = -1;

		// Token: 0x04002C5C RID: 11356
		public TransportPodsArrivalAction arrivalAction;

		// Token: 0x04002C5D RID: 11357
		private bool alreadyLeft;

		// Token: 0x04002C5E RID: 11358
		private static List<Thing> tmpActiveDropPods = new List<Thing>();
	}
}
