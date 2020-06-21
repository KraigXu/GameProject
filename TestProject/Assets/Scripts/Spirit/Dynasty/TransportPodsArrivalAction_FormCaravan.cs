using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200126A RID: 4714
	public class TransportPodsArrivalAction_FormCaravan : TransportPodsArrivalAction
	{
		// Token: 0x06006E49 RID: 28233 RVA: 0x0026835C File Offset: 0x0026655C
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			return TransportPodsArrivalAction_FormCaravan.CanFormCaravanAt(pods, destinationTile);
		}

		// Token: 0x06006E4A RID: 28234 RVA: 0x00268388 File Offset: 0x00266588
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			TransportPodsArrivalAction_FormCaravan.tmpPawns.Clear();
			for (int i = 0; i < pods.Count; i++)
			{
				ThingOwner innerContainer = pods[i].innerContainer;
				for (int j = innerContainer.Count - 1; j >= 0; j--)
				{
					Pawn pawn = innerContainer[j] as Pawn;
					if (pawn != null)
					{
						TransportPodsArrivalAction_FormCaravan.tmpPawns.Add(pawn);
						innerContainer.Remove(pawn);
					}
				}
			}
			int startingTile;
			if (!GenWorldClosest.TryFindClosestPassableTile(tile, out startingTile))
			{
				startingTile = tile;
			}
			Caravan caravan = CaravanMaker.MakeCaravan(TransportPodsArrivalAction_FormCaravan.tmpPawns, Faction.OfPlayer, startingTile, true);
			for (int k = 0; k < pods.Count; k++)
			{
				TransportPodsArrivalAction_FormCaravan.tmpContainedThings.Clear();
				TransportPodsArrivalAction_FormCaravan.tmpContainedThings.AddRange(pods[k].innerContainer);
				for (int l = 0; l < TransportPodsArrivalAction_FormCaravan.tmpContainedThings.Count; l++)
				{
					pods[k].innerContainer.Remove(TransportPodsArrivalAction_FormCaravan.tmpContainedThings[l]);
					CaravanInventoryUtility.GiveThing(caravan, TransportPodsArrivalAction_FormCaravan.tmpContainedThings[l]);
				}
			}
			TransportPodsArrivalAction_FormCaravan.tmpPawns.Clear();
			TransportPodsArrivalAction_FormCaravan.tmpContainedThings.Clear();
			Messages.Message("MessageTransportPodsArrived".Translate(), caravan, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x06006E4B RID: 28235 RVA: 0x002684CE File Offset: 0x002666CE
		public static bool CanFormCaravanAt(IEnumerable<IThingHolder> pods, int tile)
		{
			return TransportPodsArrivalActionUtility.AnyPotentialCaravanOwner(pods, Faction.OfPlayer) && !Find.World.Impassable(tile);
		}

		// Token: 0x04004420 RID: 17440
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04004421 RID: 17441
		private static List<Thing> tmpContainedThings = new List<Thing>();
	}
}
