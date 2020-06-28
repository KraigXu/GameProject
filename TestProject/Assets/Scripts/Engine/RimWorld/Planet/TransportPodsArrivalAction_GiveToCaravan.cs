using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200126C RID: 4716
	public class TransportPodsArrivalAction_GiveToCaravan : TransportPodsArrivalAction
	{
		// Token: 0x06006E55 RID: 28245 RVA: 0x0026813D File Offset: 0x0026633D
		public TransportPodsArrivalAction_GiveToCaravan()
		{
		}

		// Token: 0x06006E56 RID: 28246 RVA: 0x002687D1 File Offset: 0x002669D1
		public TransportPodsArrivalAction_GiveToCaravan(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06006E57 RID: 28247 RVA: 0x002687E0 File Offset: 0x002669E0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Caravan>(ref this.caravan, "caravan", false);
		}

		// Token: 0x06006E58 RID: 28248 RVA: 0x002687FC File Offset: 0x002669FC
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.caravan != null && !Find.WorldGrid.IsNeighborOrSame(this.caravan.Tile, destinationTile))
			{
				return false;
			}
			return TransportPodsArrivalAction_GiveToCaravan.CanGiveTo(pods, this.caravan);
		}

		// Token: 0x06006E59 RID: 28249 RVA: 0x00268850 File Offset: 0x00266A50
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			for (int i = 0; i < pods.Count; i++)
			{
				TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings.Clear();
				TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings.AddRange(pods[i].innerContainer);
				for (int j = 0; j < TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings.Count; j++)
				{
					pods[i].innerContainer.Remove(TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings[j]);
					this.caravan.AddPawnOrItem(TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings[j], true);
				}
			}
			TransportPodsArrivalAction_GiveToCaravan.tmpContainedThings.Clear();
			Messages.Message("MessageTransportPodsArrivedAndAddedToCaravan".Translate(this.caravan.Name), this.caravan, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x06006E5A RID: 28250 RVA: 0x00268916 File Offset: 0x00266B16
		public static FloatMenuAcceptanceReport CanGiveTo(IEnumerable<IThingHolder> pods, Caravan caravan)
		{
			return caravan != null && caravan.Spawned && caravan.IsPlayerControlled;
		}

		// Token: 0x06006E5B RID: 28251 RVA: 0x00268934 File Offset: 0x00266B34
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Caravan caravan)
		{
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_GiveToCaravan>(() => TransportPodsArrivalAction_GiveToCaravan.CanGiveTo(pods, caravan), () => new TransportPodsArrivalAction_GiveToCaravan(caravan), "GiveToCaravan".Translate(caravan.Label), representative, caravan.Tile, null);
		}

		// Token: 0x04004423 RID: 17443
		private Caravan caravan;

		// Token: 0x04004424 RID: 17444
		private static List<Thing> tmpContainedThings = new List<Thing>();
	}
}
