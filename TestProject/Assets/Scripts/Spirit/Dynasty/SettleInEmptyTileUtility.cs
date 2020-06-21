using System;
using System.Text;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02001247 RID: 4679
	public static class SettleInEmptyTileUtility
	{
		// Token: 0x06006D05 RID: 27909 RVA: 0x00262AD0 File Offset: 0x00260CD0
		public static void Settle(Caravan caravan)
		{
			Faction faction = caravan.Faction;
			if (faction != Faction.OfPlayer)
			{
				Log.Error("Cannot settle with non-player faction.", false);
				return;
			}
			Settlement newHome = SettleUtility.AddNewHome(caravan.Tile, faction);
			LongEventHandler.QueueLongEvent(delegate
			{
				GetOrGenerateMapUtility.GetOrGenerateMap(caravan.Tile, Find.World.info.initialMapSize, null);
			}, "GeneratingMap", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true);
			LongEventHandler.QueueLongEvent(delegate
			{
				Map map = newHome.Map;
				Thing t = caravan.PawnsListForReading[0];
				CaravanEnterMapUtility.Enter(caravan, map, CaravanEnterMode.Center, CaravanDropInventoryMode.DropInstantly, false, (IntVec3 x) => x.GetRoom(map, RegionType.Set_Passable).CellCount >= 600);
				CameraJumper.TryJump(t);
			}, "SpawningColonists", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true);
		}

		// Token: 0x06006D06 RID: 27910 RVA: 0x00262B6C File Offset: 0x00260D6C
		public static Command SettleCommand(Caravan caravan)
		{
			Command_Settle command_Settle = new Command_Settle();
			command_Settle.defaultLabel = "CommandSettle".Translate();
			command_Settle.defaultDesc = "CommandSettleDesc".Translate();
			command_Settle.icon = SettleUtility.SettleCommandTex;
			Action <>9__1;
			command_Settle.action = delegate
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				int tile = caravan.Tile;
				Action settleAction;
				if ((settleAction = <>9__1) == null)
				{
					settleAction = (<>9__1 = delegate
					{
						SettleInEmptyTileUtility.Settle(caravan);
					});
				}
				SettlementProximityGoodwillUtility.CheckConfirmSettle(tile, settleAction);
			};
			SettleInEmptyTileUtility.tmpSettleFailReason.Length = 0;
			if (!TileFinder.IsValidTileForNewSettlement(caravan.Tile, SettleInEmptyTileUtility.tmpSettleFailReason))
			{
				command_Settle.Disable(SettleInEmptyTileUtility.tmpSettleFailReason.ToString());
			}
			else if (SettleUtility.PlayerSettlementsCountLimitReached)
			{
				if (Prefs.MaxNumberOfPlayerSettlements > 1)
				{
					command_Settle.Disable("CommandSettleFailReachedMaximumNumberOfBases".Translate());
				}
				else
				{
					command_Settle.Disable("CommandSettleFailAlreadyHaveBase".Translate());
				}
			}
			return command_Settle;
		}

		// Token: 0x040043C7 RID: 17351
		private const int MinStartingLocCellsCount = 600;

		// Token: 0x040043C8 RID: 17352
		private static StringBuilder tmpSettleFailReason = new StringBuilder();
	}
}
