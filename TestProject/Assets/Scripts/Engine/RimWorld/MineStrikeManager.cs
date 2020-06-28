using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A74 RID: 2676
	public class MineStrikeManager : IExposable
	{
		// Token: 0x06003F08 RID: 16136 RVA: 0x0014F71D File Offset: 0x0014D91D
		public void ExposeData()
		{
			Scribe_Collections.Look<StrikeRecord>(ref this.strikeRecords, "strikeRecords", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06003F09 RID: 16137 RVA: 0x0014F738 File Offset: 0x0014D938
		public void CheckStruckOre(IntVec3 justMinedPos, ThingDef justMinedDef, Thing miner)
		{
			if (miner.Faction != Faction.OfPlayer)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = justMinedPos + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(miner.Map))
				{
					Building edifice = intVec.GetEdifice(miner.Map);
					if (edifice != null && edifice.def != justMinedDef && MineStrikeManager.MineableIsValuable(edifice.def) && !this.AlreadyVisibleNearby(intVec, miner.Map, edifice.def) && !this.RecentlyStruck(intVec, edifice.def))
					{
						StrikeRecord item = default(StrikeRecord);
						item.cell = intVec;
						item.def = edifice.def;
						item.ticksGame = Find.TickManager.TicksGame;
						this.strikeRecords.Add(item);
						Messages.Message("StruckMineable".Translate(edifice.def.label), edifice, MessageTypeDefOf.PositiveEvent, true);
						TaleRecorder.RecordTale(TaleDefOf.StruckMineable, new object[]
						{
							miner,
							edifice
						});
					}
				}
			}
		}

		// Token: 0x06003F0A RID: 16138 RVA: 0x0014F864 File Offset: 0x0014DA64
		public bool AlreadyVisibleNearby(IntVec3 center, Map map, ThingDef mineableDef)
		{
			CellRect cellRect = CellRect.CenteredOn(center, 1);
			for (int i = 1; i < MineStrikeManager.RadialVisibleCells; i++)
			{
				IntVec3 c = center + GenRadial.RadialPattern[i];
				if (c.InBounds(map) && !c.Fogged(map) && !cellRect.Contains(c))
				{
					Building edifice = c.GetEdifice(map);
					if (edifice != null && edifice.def == mineableDef)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003F0B RID: 16139 RVA: 0x0014F8D0 File Offset: 0x0014DAD0
		private bool RecentlyStruck(IntVec3 cell, ThingDef def)
		{
			for (int i = this.strikeRecords.Count - 1; i >= 0; i--)
			{
				StrikeRecord strikeRecord = this.strikeRecords[i];
				if (strikeRecord.Expired)
				{
					this.strikeRecords.RemoveAt(i);
				}
				else if (this.strikeRecords[i].def == def)
				{
					strikeRecord = this.strikeRecords[i];
					if (strikeRecord.cell.InHorDistOf(cell, 12f))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003F0C RID: 16140 RVA: 0x0014F954 File Offset: 0x0014DB54
		public static bool MineableIsValuable(ThingDef mineableDef)
		{
			return mineableDef.mineable && mineableDef.building.mineableThing != null && mineableDef.building.mineableThing.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)mineableDef.building.mineableYield > 10f;
		}

		// Token: 0x06003F0D RID: 16141 RVA: 0x0014F9A4 File Offset: 0x0014DBA4
		public static bool MineableIsVeryValuable(ThingDef mineableDef)
		{
			return mineableDef.mineable && mineableDef.building.mineableThing != null && mineableDef.building.mineableThing.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)mineableDef.building.mineableYield > 100f;
		}

		// Token: 0x06003F0E RID: 16142 RVA: 0x0014F9F4 File Offset: 0x0014DBF4
		public string DebugStrikeRecords()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (StrikeRecord strikeRecord in this.strikeRecords)
			{
				stringBuilder.AppendLine(strikeRecord.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040024B6 RID: 9398
		private List<StrikeRecord> strikeRecords = new List<StrikeRecord>();

		// Token: 0x040024B7 RID: 9399
		private const int RecentStrikeIgnoreRadius = 12;

		// Token: 0x040024B8 RID: 9400
		private static readonly int RadialVisibleCells = GenRadial.NumCellsInRadius(5.9f);
	}
}
